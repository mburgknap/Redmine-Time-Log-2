using Newtonsoft.Json;
using Ninject;
using NLog;
using NLog.Internal;
using RedmineLog.Common;
using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RedmineLog
{
    public class SlackClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackClient(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }

        //Post a message using a Payload object
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }

    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }


    class AppLog : ILog
    {
        private static Logger Log;

        [Inject]
        public AppLog()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        public void Error(String inMessage, Exception ex)
        {
            Log.Error(inMessage, ex);
        }


        public void Error(string inTag, Exception ex, string inMessage, string inTitle)
        {
            Log.Error(inTag, ex);

            try
            {
                NotifyBox.Show(inMessage, inTitle);

                string urlWithAccessToken = System.Configuration.ConfigurationManager.AppSettings["SlackChannel"];

                SlackClient client = new SlackClient(urlWithAccessToken);

                String msg = "Error " + ex.Message + Environment.NewLine + ex.StackTrace;

                client.PostMessage(username: "RedmineLog ",
                           text: msg,
                           channel: "#redmine_log");
            }
            catch (Exception ex2)
            {
                Log.Error(inTag, ex2);
            }
        }


        public void Info(string inMessage, string inTitle)
        {
            try
            {
                NotifyBox.Show(inMessage, inTitle);
            }
            catch (Exception ex)
            {
                Log.Error("Info", ex);
            }
        }
    }
}
