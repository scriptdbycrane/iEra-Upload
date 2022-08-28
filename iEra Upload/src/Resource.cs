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
            Type = type;
            Path = path;
        }

        public bool Exists()
        {
            switch (Type)
            {
                case ResourceType.Directory:
                    return Directory.Exists(Path);
                case ResourceType.File:
                    return File.Exists(Path);
                default:
                    return false;
            }
        }
    }
}
