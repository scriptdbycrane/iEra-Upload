namespace iEra_Upload
{
    internal class GraalID
    {
        public String ID = "";

        public GraalID(String graalID)
        {
            this.ID = graalID;
        }

        public Boolean IsValid()
        {
            if (this.ID.Length == 12 && this.ID.ToUpper().StartsWith("GRAAL"))
            {
                try
                {
                    Int32 digits = Int32.Parse(this.ID[5..]);
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
