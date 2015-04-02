using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.IO;
using System.Text;
using RedmineLog.Properties;
using System.Windows.Forms;

namespace Redmine
{
    public enum Tracker
    {
        //Bug = 1
        Feature = 2,
        //Support = 3
        //TestCase = 4
        //Management = 5
        Design = 6,
        //CR = 7
        //SR = 8
        //Enhancement = 9
        Task = 10,
        Question = 11
    }

    public enum Status
    {
        New = 1,
        //InProgress = 2
        //Resolved = 3
        Feedback = 4
        //Closed = 5
        //Rejected = 6
        //Obsolete = 7
        //Passed = 8
        //Failed = 9
        //NotTested = 10
        //Pending = 11
    }

    namespace Enumerations
    {
        public class Activity
        {
            private string _name;

            private int _id;
            public int ID
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public Activity(int id, string name)
            {
                _id = id;
                _name = name;
            }
        }

        public class Activities
        {

            private UploadManager _objUploadManager = new UploadManager();

            private ArrayList _activities = new ArrayList();

            public ArrayList Activites
            {
                get { return _activities; }
            }

            public Activities(bool force = false)
            {
                if (_activities.Count == 0)
                {
                    refresh();
                }
                else
                {
                    if (force == true)
                    {
                        refresh();
                    }
                }
            }


            private void refresh()
            {
                string strUrl = string.Format("{0}{1}", Settings.Default.RedmineURL, Settings.Default.Enumeration, Settings.Default.ApiKey);
                string strXMLOutput = null;
                XmlDocument objXMLDoc = new XmlDocument();
                XPathNavigator objNav = null;
                XPathNodeIterator objNodeIterator = null;
                System.Collections.Specialized.NameValueCollection param = new System.Collections.Specialized.NameValueCollection();
                try
                {
                    param.Add("key", Settings.Default.ApiKey);
                    _objUploadManager.Timeout = 99999;
                    strXMLOutput = _objUploadManager.SendData(strUrl, UploadManager.HttpMethod.GET, param);
                    _activities.Clear();
                    objXMLDoc.LoadXml(strXMLOutput);
                    objNav = objXMLDoc.CreateNavigator();
                    objNodeIterator = objNav.Select("//time_entry_activity");
                    while (objNodeIterator.MoveNext())
                    {
                        Activity entry = new Activity(Int32.Parse(objNodeIterator.Current.SelectSingleNode("id").ToString()), objNodeIterator.Current.SelectSingleNode("name").ToString());
                        _activities.Add(entry);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }

        }

    }

    public class Issues
    {
        private UploadManager _objUploadManager = new UploadManager();

        private int _projectID;
        public Issues(int projectID)
        {
            _projectID = projectID;
        }

        private XmlElement element(XmlDocument objDoc, string key, string value)
        {
            XmlElement obj = null;
            obj = objDoc.CreateElement(key);
            obj.InnerText = value;
            return obj;
        }

        public bool CreateIssue(Tracker trackerID, Status statusID, string subject, string description, int assignTo)
        {
            string strUrl = string.Format("{0}{1}?key={2}", Settings.Default.RedmineURL, Settings.Default.Issues, Settings.Default.ApiKey);
            XmlDocument objDoc = new XmlDocument();
            XmlElement objRootElement = null;
            objRootElement = objDoc.CreateElement("issue");
            objDoc.AppendChild(objRootElement);
            var _with1 = objRootElement;
            _with1.AppendChild(element(objDoc, "project_id", _projectID.ToString()));
            _with1.AppendChild(element(objDoc, "tracker_id", trackerID.ToString()));
            _with1.AppendChild(element(objDoc, "status_id", statusID.ToString()));
            _with1.AppendChild(element(objDoc, "subject", subject));
            _with1.AppendChild(element(objDoc, "description", description));
            _with1.AppendChild(element(objDoc, "assigned_to_id", assignTo.ToString()));
            _with1.AppendChild(element(objDoc, "priority_id", 4.ToString()));
            //.AppendChild(element(objDoc, "category_id", _projectID))
            //.AppendChild(element(objDoc, "parent_issue_id", _projectID))
            //.AppendChild(element(objDoc, "custom_fields", _projectID))
            //.AppendChild(element(objDoc, "watcher_user_ids", _projectID))

            return contactRedmine(objDoc.OuterXml, strUrl);

        }

        private bool contactRedmine(string strXML, string strUrl)
        {
            try
            {
                HttpWebRequest objRequest = null;
                HttpWebResponse objResponse = null;
                StreamReader objReader = null;
                Stream objStream = null;
                byte[] arrFileBytes = null;
                string strResponse = null;

                objRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                objRequest.Method = "POST";
                objRequest.ContentType = "application/xml";
                arrFileBytes = System.Text.Encoding.ASCII.GetBytes(strXML);
                objStream = objRequest.GetRequestStream();
                objStream.Write(arrFileBytes, 0, arrFileBytes.Length);
                objStream.Close();

                objResponse = (HttpWebResponse)objRequest.GetResponse();
                objReader = new StreamReader(objResponse.GetResponseStream());
                strResponse = objReader.ReadToEnd();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + strUrl);
                return false;
            }
        }

    }

    public class Users
    {
        private UploadManager _objUploadManager = new UploadManager();

        private User[] _users;
        public ArrayList List()
        {
            ArrayList arrUsers = new ArrayList();
            string strXMLOutput = null;
            XmlDocument objXMLDoc = new XmlDocument();
            XPathNavigator objNav = null;
            XPathNodeIterator objNodeIterator = null;
            User objUser = null;
            string strUrl = string.Format("{0}{1}", Settings.Default.RedmineURL, Settings.Default.Users);
            System.Collections.Specialized.NameValueCollection param = new System.Collections.Specialized.NameValueCollection();

            try
            {
                param.Add("key", Settings.Default.ApiKey);
                strXMLOutput = _objUploadManager.SendData(strUrl, UploadManager.HttpMethod.GET, param);
                objXMLDoc.LoadXml(strXMLOutput);
                objNav = objXMLDoc.CreateNavigator();
                objNodeIterator = objNav.Select("//user");
                while ((objNodeIterator.MoveNext()))
                {
                    objUser = new User(objNodeIterator.Current);
                    arrUsers.Add(objUser);
                }
                return arrUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class User
    {
        private string _username;
        private string _firstName;
        private string _lastName;
        private string _email;

        private int _id;
        #region "Properties"
        public object Username
        {
            get { return _username; }
        }
        public object FirstName
        {
            get { return _firstName; }
        }
        public object LastName
        {
            get { return _lastName; }
        }

        public object FullName
        {
            get { return _firstName + " " + _lastName; }
        }
        public object Email
        {
            get { return _email; }
        }
        public object ID
        {
            get { return _id; }
        }

        #endregion

        public User(XPathNavigator node)
        {
            _id = Int32.Parse(node.SelectSingleNode("id").ToString());
            _username = node.SelectSingleNode("login").ToString();
            _firstName = node.SelectSingleNode("firstname").ToString();
            _lastName = node.SelectSingleNode("lastname").ToString();
            _email = node.SelectSingleNode("mail").ToString();
        }

        public User(string username, string firstName, string lastName, string email, int id)
        {
            _username = username;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _id = id;
        }

    }

    public class TimeEntry
    {
        private UploadManager _objUploadManager = new UploadManager();

        private int _entryID;
        public enum Type
        {
            Project,
            Issue
        }

        public TimeEntry(int entryId)
        {
            _entryID = entryId;
        }

        private XmlElement element(XmlDocument objDoc, string key, string value)
        {
            XmlElement obj = null;
            obj = objDoc.CreateElement(key);
            obj.InnerText = value;
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueID">the issue id or project id to log time on</param>
        /// <param name="spentOn">the date the time was spent (default to the current date)</param>
        /// <param name="hours">the number of spent hours</param>
        /// <param name="activityID">the id of the time activity. This parameter is required unless a default activity is defined in Redmine.</param>
        /// <param name="comment">short description for the entry (255 characters max)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool RecordEntry(System.DateTime spentOn, float hours, int activityID, string comment, Type eType = Type.Project)
        {
            string strUrl = string.Format("{0}{1}?key={2}", Settings.Default.RedmineURL, Settings.Default.TimeEntry, Settings.Default.ApiKey);
            XmlDocument objDoc = new XmlDocument();
            XmlElement objRootElement = null;
            objRootElement = objDoc.CreateElement("time_entry");
            objDoc.AppendChild(objRootElement);
            var _with2 = objRootElement;
            if (eType == Type.Project)
            {
                _with2.AppendChild(element(objDoc, "project_id", _entryID.ToString()));
            }
            else
            {
                _with2.AppendChild(element(objDoc, "issue_id", _entryID.ToString()));
            }
            _with2.AppendChild(element(objDoc, "spent_on", spentOn.ToString("yyyy-MM-dd")));
            _with2.AppendChild(element(objDoc, "activity_id", activityID.ToString()));
            _with2.AppendChild(element(objDoc, "hours", hours.ToString()));
            _with2.AppendChild(element(objDoc, "comments", comment));
            return contactRedmine(objDoc.OuterXml, strUrl);
        }

        private bool contactRedmine(string strXML, string strUrl)
        {
            try
            {
                HttpWebRequest objRequest = null;
                HttpWebResponse objResponse = null;
                StreamReader objReader = null;
                Stream objStream = null;
                byte[] arrFileBytes = null;
                string strResponse = null;

                objRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                objRequest.Method = "POST";
                objRequest.ContentType = "application/xml";
                arrFileBytes = System.Text.Encoding.ASCII.GetBytes(strXML);
                objStream = objRequest.GetRequestStream();
                objStream.Write(arrFileBytes, 0, arrFileBytes.Length);
                objStream.Close();

                objResponse = (HttpWebResponse)objRequest.GetResponse();
                objReader = new StreamReader(objResponse.GetResponseStream());
                strResponse = objReader.ReadToEnd();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + strUrl);
                return false;
            }
        }

    }


}
