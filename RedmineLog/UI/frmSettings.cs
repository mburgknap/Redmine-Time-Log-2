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
            inModel.Timer.OnUpdate.Subscribe(OnUpdateTimer);
            inModel.Url.OnUpdate.Subscribe(OnUpdateUrl);
            inModel.WorkDayHours.OnUpdate.Subscribe(OnUpdateWorkDayHours);
        }

        private void OnUpdateWorkDayHours(int obj)
        {
            Form.tbWorkHours.Text = obj.ToString();
        }

        private void OnUpdateUrl(StringBuilder obj)
        {
            Form.tbRedmineURL.Set(obj,
                          (ui, data) =>
                          {
                              ui.Text = data.ToString();
                          });
        }

        private void OnUpdateTimer(TimerType obj)
        {
            Form.cbTimer.Set(obj,
                    (ui, data) =>
                    {
                        timerChanged.Dispose();
                        Form.cbTimer.Items.Clear();

                        Form.cbTimer.Items.Add(TimerType.System);
                        Form.cbTimer.Items.Add(TimerType.Thread);


                        if (data == TimerType.System)
                            ui.SelectedItem = Form.cbTimer.Items[0];
                        else
                            if (data == TimerType.Thread)
                                ui.SelectedItem = Form.cbTimer.Items[1];


                        timerChanged.Subscribe(Observer.Create<EventPattern<EventArgs>>(OnNotifyTimer));
                    });
        }

        private void OnNotifyTimer(EventPattern<EventArgs> obj)
        {
            model.Timer.Notify(((TimerType)Form.cbTimer.SelectedItem));
            NotifyBox.Show("Please restart application", "Info");
            Form.Close();
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

        [EventPublication(Settings.Events.Save, typeof(OnPublisher))]
        public event EventHandler SaveEvent;

        [EventPublication(Settings.Events.Load, typeof(OnPublisher))]
        public event EventHandler LoadEvent;

        [EventPublication(Settings.Events.ReloadCache, typeof(OnPublisher))]
        public event EventHandler ReloadCacheEvent;

        EventProperty<EventArgs> displayChanged = new EventProperty<EventArgs>();
        EventProperty<EventArgs> timerChanged = new EventProperty<EventArgs>();

        public void Init(frmSettings frmSettings)
        {
            Form = frmSettings;

            displayChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbDisplay, "SelectedIndexChanged"));
            timerChanged.Build(Observable.FromEventPattern<EventArgs>(Form.cbTimer, "SelectedIndexChanged"));

            Observable.FromEventPattern<EventArgs>(Form.btnSave, "Click").Subscribe(OnActionSave);
            Observable.FromEventPattern<EventArgs>(Form.btnReloadCache, "Click").Subscribe(OnActionReloadCache);

            Load();
        }


        private void OnActionReloadCache(EventPattern<EventArgs> obj)
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

        private void OnActionSave(EventPattern<EventArgs> obj)
        {
            model.Url.Notify(Form.tbRedmineURL.Text);
            model.ApiKey.Notify(Form.tbApiKey.Text);

            int tmp = 0;
            if (Int32.TryParse(Form.tbWorkHours.Text, out tmp))
                model.WorkDayHours.Notify(tmp);

            new frmProcessing().Show(Form,
              () =>
              {
                  SaveEvent.Fire(this);

                  Form.Set(model,
                     (ui, data) =>
                     {
                         ui.DialogResult = System.Windows.Forms.DialogResult.OK;
                     });
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

    }
}