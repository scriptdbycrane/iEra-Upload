using CefSharp;
using CefSharp.WinForms;
using System.CodeDom;

namespace iEra_Upload
{
    public partial class Form1 : Form
    {
        private CefSettings? Settings = null;
        private ChromiumWebBrowser? WebBrowser = null;

        public Form1()
        {
            this.InitializeComponent();
            this.InitializeChromium();
        }

        private void InitializeChromium()
        {
            this.Settings = new CefSettings();
            Cef.Initialize(this.Settings);

            const String URL = "https://eraupload.graalonline.com";
            this.WebBrowser = new ChromiumWebBrowser(URL);
            this.Controls.Add(this.WebBrowser);

            this.WebBrowser.Dock = DockStyle.Fill;
            this.WebBrowser.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(this.Width, this.Height);
            this.MaximumSize = this.MinimumSize;
        }
    }
}