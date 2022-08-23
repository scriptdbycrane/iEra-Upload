using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser();
        private System.Timers.Timer JSTimer = new System.Timers.Timer();
        private String CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\";
        private String CacheFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\.graal_id";
        private String GraalID = "";

        public Form1()
        {
            this.InitializeComponent();
            this.InitializeChromium();
        }

        private void InitializeChromium()
        {
            // Initialize the settings.
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);

            // Confiugure the web browser.
            const String URL = "https://eraupload.graalonline.com";
            this.WebBrowser.Load(URL);
            this.Controls.Add(this.WebBrowser);

            this.WebBrowser.Dock = DockStyle.Fill;
            this.WebBrowser.Show();

            // This timer determines when any custom JavaScript should begin executing in the web browser.
            this.JSTimer = new System.Timers.Timer();
            this.JSTimer.Interval = 500;
            this.JSTimer.Elapsed += this.WebBrowserTimer_Elapsed;
            this.JSTimer.Start();
        }

        private void WebBrowserTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                // Attempting to run any custom JavaScript before the web browser is initialized will result in a crash.
                if (this.WebBrowser.IsBrowserInitialized)
                {
                    // Fill the Graal ID field with the cached Graal ID if the latter exists.
                    this.WebBrowser.EvaluateScriptAsync($"document.getElementById(\"email\").value = \"{this.GraalID}\";");
                    this.JSTimer.Stop();
                    this.JSTimer.Dispose();
                }
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the permanent dimensions of the application.
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = this.MinimumSize;

            // Read the cached Graal ID, if it exists.
            if (File.Exists(this.CacheFile))
            {
                String[] contents = File.ReadAllLines(this.CacheFile);
                this.GraalID = contents.Length > 0 ? contents.First() : "";
            }
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If the cache file is not found, create the cache directory and cache file.
            if (!File.Exists(this.CacheFile))
            {
                Directory.CreateDirectory(this.CachePath);
            }

            JavascriptResponse response = await this.WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            this.GraalID = (String)response.Result;

            // Write the most recently typed Graal ID to the cache file.
            // This will only write "valid" Graal IDs -- Graal IDs that are formatted correctly (e.g. Graal1234567).
            if (this.GraalID.Length == 12 && this.GraalID.ToUpper().StartsWith("GRAAL"))
            {
                File.WriteAllText(this.CacheFile, this.GraalID);
            }
        }
    }
}