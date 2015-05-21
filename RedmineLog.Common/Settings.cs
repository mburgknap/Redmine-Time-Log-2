using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog.Common
{
    public static class Settings
    {
        public const string Load = "topic://Load";

        public const string Connect = "topic://Connect";

        public interface IView
        {
        }

        public interface IModel
        {
            string Url { get; set; }
            string ApiKey { get; set; }
            int IdUser { get; set; }
        }
    }
}
