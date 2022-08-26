using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    public partial class App : Form
    {
        private string URL = "https://eraupload.graalonline.com";
        private ChromiumWebBrowser WebBrowser = new();
        private System.Timers.Timer JSTimer = new();
        private Resource CacheDirectory = new Resource(Resource.ResourceType.Directory, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\");
        private Resource CacheFile = new Resource(Resource.ResourceType.File, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\.graal_id");
        private GraalID GraalID = new GraalID(string.Empty);

        public App()
        {
            this.InitializeComponent();
            this.InitializeChromium();
        }

        public void InitializeChromium()
        {
            CefSettings settings = new();
            Cef.Initialize(settings);

            this.WebBrowser.Load(URL);
            this.Controls.Add(this.WebBrowser);

            this.WebBrowser.Dock = DockStyle.Fill;
            this.WebBrowser.Show();

            // This timer determines when any custom JavaScript should begin executing in the web browser.
            this.JSTimer = new System.Timers.Timer();
            this.JSTimer.Interval = 500;
            this.JSTimer.Elapsed += JSTimer_Elapsed;
            this.JSTimer.Start();
        }

        public void JSTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (this.WebBrowser.IsBrowserInitialized)
                {
                    this.WebBrowser.EvaluateScriptAsync($"document.getElementById(\"email\").value = \"{this.GraalID.ID}\";");
                    this.JSTimer.Stop();

                    if (!this.RefreshButton.Visible)
                    {
                        this.RefreshButton.Show();
                    }
                }
            }));
        }

        public void App_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = this.MinimumSize;

            if (this.CacheFile.Exists())
            {
                string[] contents = File.ReadAllLines(CacheFile.Path);
                GraalID? graalID = contents.Length > 0 ? new GraalID(contents.First()) : null;
                this.GraalID.ID = graalID != null ? (graalID.IsValid() ? graalID.ID : "") : "";
            }
        }

        public async void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            JavascriptResponse response = await this.WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            this.GraalID.ID = (string)response.Result;

            if (!this.CacheDirectory.Exists())
            {
                Directory.CreateDirectory(this.CacheDirectory.Path);
            }

            if (this.GraalID.IsValid())
            {
                File.WriteAllText(this.CacheFile.Path, this.GraalID.ID);
            }
        }

        public async void RefreshButton_Click(object sender, EventArgs e)
        {
            await this.WebBrowser.LoadUrlAsync(this.URL);
            this.JSTimer.Start();
        }
    }
}