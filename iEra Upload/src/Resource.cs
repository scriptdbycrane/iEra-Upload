namespace iEra_Upload
{
    internal class Resource
    {
        public static readonly string Root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\iEra Upload\";
        public enum ResourceType
        {
            Directory,
            File
        };
        public ResourceType Type { get; set; }
        public string Path { get; set; }

        public Resource(ResourceType type, string path)
        {
            this.Type = type;
            this.Path = path;
        }

        public bool Exists()
        {
            switch (this.Type)
            {
                case ResourceType.Directory:
                    return Directory.Exists(this.Path);
                case ResourceType.File:
                    return File.Exists(this.Path);
                default:
                    return false;
            }
        }
    }
}
