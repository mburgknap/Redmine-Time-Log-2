﻿namespace RedmineLog.Common
{
    public static class Settings
    {
        public static class Events
        {
            public const string Load = "topic://Settings/Load";

            public const string Connect = "topic://Settings/Connect";
        }

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