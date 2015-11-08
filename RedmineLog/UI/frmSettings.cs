using Appccelerate.EventBroker;
using Ninject;
using RedmineLog.Common;
using RedmineLog.UI;
using RedmineLog.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Forms;

namespace RedmineLog
{
    public partial class frmSettings : Form, ISetup
    {
        private IAppSettings settings;
        public frmSettings()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            this.Initialize<Settings.IView, frmSettings>();
            Load += OnSettingsLoad;
        }

        void OnSettingsLoad(object sender, EventArgs e)
        {
            this.SetupLocation(settings.Display, 0, -100);
        }

        public void Setup(IAppSettings inSettings)
        {
            settings = inSettings;
        }
    }

    internal class SettingsView : Settings.IView, IView<frmSettings>
    {
        private Settings.IModel model;

        private frmSettings Form;

        [Inject]
        public SettingsView(Settings.IModel inModel)
        {
            model = inModel;
            inModel.ApiKey.OnUpdate.Subscribe(OnUpdateApiKey);
            inModel.Display.OnUpdate.Subscribe(OnUpdateDisplay);
            inModel.Url.OnUpdate.Subscribe(OnUpdateUrl);
        }

        private void OnUpdateUrl(StringBuilder obj)
        {
            Form.tbRedmineURL.Set(obj,
                          (ui, data) =>
                          {
                              ui.Text = data.ToString();
                          });
        }

        private void OnUpdateDisplay(DisplayData obj)
        {
        
            Form.cbDisplay.Set(obj,
                     (ui, data) =>
                     {
                         displayChanged.Dispose();
                         Form.cbDisplay.Items.Clear();

                         foreach (var display in Screen.AllScreens)
                         {
                             Form.cbDisplay.Items.Add(new DisplayData()
                             {
                                 Name = display.DeviceName,
                                 X = display.Bounds.X,
                                 Y = display.Bounds.Y,
                                 Width = display.Bounds.Width,
                                 Height = display.Bounds.Height,
                             });

                             if (data != null && data.Name == display.DeviceName)
                                 ui.SelectedItem = Form.cbDisplay.Items[Form.cbDisplay.Items.Count - 1];
                         }

                         if (ui.SelectedItem == null)
                             ui.SelectedItem = Form.cbDisplay.Items[0];

                         displayChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyDisplay));
                     });
        }

        private void OnNotifyDisplay(EventPattern<EventArgs> obj)
        {
            model.Display.Notify(((DisplayData)Form.cbDisplay.SelectedItem));
            NotifyBox.Show("Please restart application", "Info");
            Form.Close();
        }

        private void OnUpdateApiKey(StringBuilder obj)
        {
            Form.tbApiKey.Set(obj,
                   (ui, data) =>
                   {
                       ui.Text = data.ToString();
                   });
        }

        [EventPublication(Settings.Events.Connect, typeof(Publish<Settings.IView>))]
        public event EventHandler ConnectEvent;

        [EventPublication(Settings.Events.Load, typeof(Publish<Settings.IView>))]
        public event EventHandler LoadEvent;

        [EventPublication(Settings.Events.ReloadCache, typeof(Publish<Settings.IView>))]
        public event EventHandler ReloadCacheEvent;

        EventProperty<EventArgs> displayChanged = new EventProperty<EventArgs>();

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            displayChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbDisplay, "SelectedIndexChanged"));

            Form.btnConnect.Click += OnConnectClick;
            Form.btnReloadCache.Click += OnReloadCacheClick;
            Load();
        }


        private void OnReloadCacheClick(object sender, EventArgs e)
        {
            new frmProcessing().Show(Form,
                 () =>
                 {
                     ReloadCacheEvent.Fire(this);
                 }, () =>
                 {
                     NotifyBox.Show("Cache reloaded.", "Info");
                 });
        }

        public void Load()
        {
            new frmProcessing().Show(Form,
               () =>
               {
                   LoadEvent.Fire(this);
               });
        }

        private void OnConnectClick(System.Object sender, System.EventArgs e)
        {
            model.Url.Notify(Form.tbRedmineURL.Text);
            model.ApiKey.Notify(Form.tbApiKey.Text);

            new frmProcessing().Show(Form,
              () =>
              {
                  ConnectEvent.Fire(this);

                  Form.Set(model,
                     (ui, data) =>
                     {
                         ui.DialogResult = System.Windows.Forms.DialogResult.OK;
                     });
              });
        }

    }
}