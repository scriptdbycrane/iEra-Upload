using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    public partial class Form1 : Form
    {
        private ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser();
        private System.Timers.Timer JSTimer = new System.Timers.Timer();

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
        }
    }
}