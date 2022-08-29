using CefSharp;
using CefSharp.WinForms;

namespace iEra_Upload
{
    internal class PreviewForm : Form
    {
        private static App? App = default;
        private readonly string Url = "https://graal.style/customs-viewer/";

        private ChromiumWebBrowser WebBrowser = new();

        public PreviewForm(App app)
        {
            App = app;
            this.Text = "Preview";
            this.Icon = App.Icon;
            this.Size = new Size(500, 490);
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.MaximizeBox = false;

            InitializeWebBrowser();
        }

        private async void InitializeWebBrowser()
        {
            this.Controls.Add(this.WebBrowser);
            this.WebBrowser.Dock = DockStyle.Fill;

            await this.WebBrowser.LoadUrlAsync(this.Url);
            await this.WebBrowser.EvaluateScriptAsync("window.scrollTo(0, 240);");
            await this.WebBrowser.EvaluateScriptAsync("window.onscroll = function() { window.scrollTo(0, 240); };");
            await this.WebBrowser.EvaluateScriptAsync("document.body.style.overflow = 'hidden';");
        }
    }
}
