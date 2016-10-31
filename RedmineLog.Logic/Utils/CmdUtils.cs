using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace RedmineLog.Logic.Utils
{
    public static class CmdUtils
    {
        public static bool Execute(string inParams)
        {
            try
            {
                var tmp = inParams.Split(' ');
                if (tmp.Count() < 2) return false;

                var parameters = tmp[1].Split(';');

                if ("/export".Equals(parameters[0]))
                {

                    var typeList = Assembly.LoadFrom("Redmine.Net.Api.dll").GetTypes()
                                          .Where(t => t.Namespace == "Redmine.Net.Api.Types")
                                          .ToList();

                    var type = typeList.Where(x => x.Name.Equals(parameters[1])).FirstOrDefault();

                    if (type != null)
                    {
                        XmlDocument doc = new XmlDocument();
                        XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
                        MemoryStream stream = new System.IO.MemoryStream();
                        StreamWriter file = null;

                        try
                        {
                            serializer.Serialize(stream, EmptyModel(type));
                            stream.Position = 0;
                            doc.Load(stream);

                            file = new System.IO.StreamWriter(parameters[2], false);
                            file.WriteLine(doc.InnerXml);
                        }

                        catch
                        {
                            throw;
                        }

                        finally
                        {
                            if (stream != null)
                            {
                                stream.Close();
                                stream.Dispose();
                            }
                            if (file != null)
                            {
                                file.Close();
                            }
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public static Object EmptyModel(Type inType)
        {
            var model = Activator.CreateInstance(inType);

            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                Type myType = property.PropertyType;
                var constructor = myType.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    // will initialize to a new copy of property type
                    property.SetValue(model, Activator.CreateInstance(myType));
                }
                else
                {
                    // will initialize to the default value of property type

                    if (myType.GenericTypeArguments.Count() == 1)
                    {
                        Type gType = myType.GenericTypeArguments[0];
                        if (myType.UnderlyingSystemType.Name.Contains("Nullable"))
                        {
                            if (gType == typeof(DateTime))
                                property.SetValue(model, DateTime.Now);
                            else
                                property.SetValue(model, Activator.CreateInstance(gType));
                        }
                        else
                            if (myType.UnderlyingSystemType.Name.Contains("IList"))
                            {
                                var listType = typeof(List<>);
                                var genericArgs = myType.GetGenericArguments();
                                var concreteType = listType.MakeGenericType(genericArgs);
                                var newList = Activator.CreateInstance(concreteType);

                                ((IList)newList).Add(EmptyModel(gType));
                                property.SetValue(model, newList);
                            }

                    }
                    else
                    {
                        if (myType == typeof(string))
                            property.SetValue(model, " ");
                        else
                            property.SetValue(model, null);
                    }
                }
            }
            return model;
        }
    }
}
