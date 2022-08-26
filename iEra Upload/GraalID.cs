namespace iEra_Upload
{
    internal class GraalID
    {
        public string ID { get; set; }

        public GraalID(string graalID)
        {
            ID = graalID;
        }

        public bool IsValid()
        {
            if (ID.Length == 12 && ID.ToUpper().StartsWith("GRAAL"))
            {
                try
                {
                    int digits = int.Parse(ID[5..]);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
