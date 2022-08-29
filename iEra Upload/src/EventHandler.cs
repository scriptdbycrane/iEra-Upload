using CefSharp;

namespace iEra_Upload
{
    internal static class EventHandler
    {
        static private App? App = default;

        static public void Initialize(App app)
        {
            App = App ?? app;
        }

        static public void Load(object? sender, EventArgs e)
        {
            App!.MinimumSize = App.Size;
            App.MaximumSize = App.Size;

            if (App.CacheFile.Exists())
            {
                string[] content = File.ReadAllLines(App.CacheFile.Path);
                GraalID? graalID = content.Length > 0 ? new GraalID(content[0]) : null;
                App.GraalID.ID = graalID != null ? (graalID.IsValid() ? graalID.ID : string.Empty) : string.Empty;
            }
        }

        static public async void FormClosing(object? sender, FormClosingEventArgs e)
        {
            JavascriptResponse response = await App!.WebBrowser.EvaluateScriptAsync("document.getElementById(\"email\").value;");
            App.GraalID.ID = (string)response.Result;

            if (!Directory.Exists(Resource.Root))
            {
                Directory.CreateDirectory(Resource.Root);
            }
            if (App.GraalID.IsValid())
            {
                File.WriteAllText(App.CacheFile.Path, App.GraalID.ID);
            }
        }

        static public async void RefreshButton_Click(object? sender, EventArgs e)
        {
            await App!.WebBrowser.LoadUrlAsync(App.Url);
            App.JavascriptPreexecutionTimer.Start();
        }

        static public void PreviewButton_Click(object? sender, EventArgs e)
        {
            PreviewForm preview = new(App!);
            preview.Show();
        }

        static public void Elapsed(object? sender, EventArgs e)
        {
            if (App!.WebBrowser.IsBrowserInitialized)
            {
                App.WebBrowser.EvaluateScriptAsync($"document.getElementById(\"email\").value = \"{App.GraalID.ID}\";");
                App.JavascriptPreexecutionTimer.Stop();
            }
        }
    }
}
