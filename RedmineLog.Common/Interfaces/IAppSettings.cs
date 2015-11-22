using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public class DisplayData
    {
        public string Name;
        public int X, Y, Width, Height;

        public override string ToString()
        {
            return Name;
        }
    }

    public interface IAppSettings
    {
        DisplayData Display { get; }
    }
}
