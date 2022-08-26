using System.Security.Policy;

namespace iEra_Upload
{
    internal class Resource
    {
        public string Path { get; set; }

        public Resource(string path)
        {
            Path = path;
        }

        public bool Exists()
        {
            return Directory.Exists(Path) || File.Exists(Path);
        }
    }
}
