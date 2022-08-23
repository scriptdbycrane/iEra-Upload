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
        private GraalID GraalID = new GraalID("");

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
                    // Fill the Graal ID field with the cached Graal ID.
                    // The Graal ID field will be set to nothing, if the cached Graal ID is invalid.
                    this.WebBrowser.EvaluateScriptAsync($"document.getElementById(\"email\").value = \"{this.GraalID.ID}\";");
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
                GraalID? graalID = contents.Length > 0 ? new GraalID(contents.First()) : null;
                this.GraalID.ID = graalID != null ? (graalID.IsValid() ? graalID.ID : "") : ""; 
            }
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Read the Graal ID from the Graal ID field.
            JavascriptResponse response = await this.WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            this.GraalID.ID = (String)response.Result;

            // The cache path needs to exist prior to creating the cache file.
            if (!Directory.Exists(this.CachePath))
            {
                Directory.CreateDirectory(this.CachePath);
            }

            // Write the most recently read Graal ID to the cache file.
            // This will only write "valid" Graal IDs -- Graal IDs that are formatted correctly (e.g., GraalXXXXXXX).
            if (this.GraalID.IsValid())
            {
                File.WriteAllText(this.CacheFile, this.GraalID.ID);
            }
        }
    }
}