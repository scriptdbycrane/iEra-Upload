using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    public partial class App : Form
    {
        private String URL = "https://eraupload.graalonline.com";
        private ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser();
        private System.Timers.Timer JSTimer = new System.Timers.Timer();
        private String CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\";
        private String CacheFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\.graal_id";
        private GraalID GraalID = new GraalID("");

        public App()
        {
            this.InitializeComponent();
            this.InitializeChromium();
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            this.WebBrowser.Load(this.URL);
            this.Controls.Add(this.WebBrowser);

            this.WebBrowser.Dock = DockStyle.Fill;
            this.WebBrowser.Show();

            // This timer determines when any custom JavaScript should begin executing in the web browser.
            this.JSTimer = new System.Timers.Timer();
            this.JSTimer.Interval = 500;
            this.JSTimer.Elapsed += this.JSTimer_Elapsed;
            this.JSTimer.Start();
        }

        private void JSTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
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

        private void App_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = this.MinimumSize;

            if (File.Exists(this.CacheFile))
            {
                String[] contents = File.ReadAllLines(this.CacheFile);
                GraalID? graalID = contents.Length > 0 ? new GraalID(contents.First()) : null;
                this.GraalID.ID = graalID != null ? (graalID.IsValid() ? graalID.ID : "") : "";
            }
        }

        private async void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            JavascriptResponse response = await this.WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            this.GraalID.ID = (String)response.Result;

            if (!Directory.Exists(this.CachePath))
            {
                Directory.CreateDirectory(this.CachePath);
            }

            if (this.GraalID.IsValid())
            {
                File.WriteAllText(this.CacheFile, this.GraalID.ID);
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            await this.WebBrowser.LoadUrlAsync(this.URL);
            this.JSTimer.Start();
        }
    }
}