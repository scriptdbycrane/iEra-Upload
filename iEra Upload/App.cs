using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    public partial class App : Form
    {
        private string URL = "https://eraupload.graalonline.com";
        private ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser();
        private System.Timers.Timer JSTimer = new System.Timers.Timer();
        private string CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\";
        private string CacheFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\.graal_id";
        private GraalID GraalID = new GraalID("");

        public App()
        {
            InitializeComponent();
            InitializeChromium();
        }

        private void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            WebBrowser.Load(URL);
            Controls.Add(WebBrowser);

            WebBrowser.Dock = DockStyle.Fill;
            WebBrowser.Show();

            // This timer determines when any custom JavaScript should begin executing in the web browser.
            JSTimer = new System.Timers.Timer();
            JSTimer.Interval = 500;
            JSTimer.Elapsed += JSTimer_Elapsed;
            JSTimer.Start();
        }

        private void JSTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                if (WebBrowser.IsBrowserInitialized)
                {
                    WebBrowser.EvaluateScriptAsync($"document.getElementById(\"email\").value = \"{GraalID.ID}\";");
                    JSTimer.Stop();

                    if (!RefreshButton.Visible)
                    {
                        RefreshButton.Show();
                    }
                }
            }));
        }

        private void App_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(Width, Height);
            MaximumSize = MinimumSize;

            if (File.Exists(CacheFile))
            {
                string[] contents = File.ReadAllLines(CacheFile);
                GraalID? graalID = contents.Length > 0 ? new GraalID(contents.First()) : null;
                GraalID.ID = graalID != null ? (graalID.IsValid() ? graalID.ID : "") : "";
            }
        }

        private async void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            JavascriptResponse response = await WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            GraalID.ID = (string)response.Result;

            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }

            if (GraalID.IsValid())
            {
                File.WriteAllText(CacheFile, GraalID.ID);
            }
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            await WebBrowser.LoadUrlAsync(URL);
            JSTimer.Start();
        }
    }
}