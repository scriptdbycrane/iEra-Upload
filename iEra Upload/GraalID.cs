namespace iEra_Upload
{
    internal class GraalID
    {
        public string ID { get; set; }

        public GraalID(string graalID)
        {
            this.ID = graalID;
        }

        public bool IsValid()
        {
            if (this.ID.Length == 12 && this.ID.ToUpper().StartsWith("GRAAL"))
            {
                try
                {
                    int digits = int.Parse(this.ID[5..]);
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
