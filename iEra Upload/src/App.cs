using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    internal partial class App : Form
    {
        public readonly string Url = "https://eraupload.graalonline.com";
        public readonly Resource CacheFile = new Resource(Resource.ResourceType.File, Resource.Root + ".graal_id");
        public GraalID GraalID = new GraalID(string.Empty);

        public System.Timers.Timer JavascriptPreexecutionTimer = new();

        public App()
        {
            this.InitializeComponent();
            this.InitializeCefSharp();
            this.InitializeNonFormControls();
            this.MapEvents();
        }

        private async void InitializeCefSharp()
        {
            CefSettings settings = new();
            Cef.Initialize(settings);

            await this.WebBrowser.LoadUrlAsync(this.Url);
            this.RefreshButton.Show();
            this.PreviewButton.Show();
        }

        private void InitializeNonFormControls()
        {
            this.JavascriptPreexecutionTimer.Interval = 500;
            this.JavascriptPreexecutionTimer.Start();
        }

        private void MapEvents()
        {
            EventHandler.Initialize(this);

            this.Load += EventHandler.App_Load;
            this.FormClosing += EventHandler.App_FormClosing;
            this.RefreshButton.Click += EventHandler.RefreshButton_Click;
            this.PreviewButton.Click += EventHandler.PreviewButton_Click;
            this.JavascriptPreexecutionTimer.Elapsed += EventHandler.JavascriptPreexecutionTimer_Elapsed;
        }
    }
}