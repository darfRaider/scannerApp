using MongoDB.Driver;
using scanapp;

namespace scanapp {
    class Utils {
        public static int ReadPositiveInteger()
        {
            string? lineString = Console.ReadLine();
            if (lineString == "" || lineString == null)
            {
                return Constants.UNASSIGNED;
            }
            int query = Constants.UNASSIGNED;
            try
            {
                query = Int32.Parse(lineString);
            }
            catch
            {
                Console.WriteLine("Unable to parse integer.");
            }
            return query;
        }

        public static int? ReadInteger()
        {
            string? lineString = Console.ReadLine();
            if (lineString == "" || lineString == null)
            {
                return null;
            }
            int query = Constants.UNASSIGNED;
            try
            {
                query = Int32.Parse(lineString);
            }
            catch
            {
                Console.WriteLine("Unable to parse integer.");
            }
            return query;
        }

        public static DateTime ParseDate(string input)
        {
            try
            {
                return DateTime.Parse(input);
            }
            catch
            {
                throw new Exception(String.Format("Datetimeparsing failed with input {0}", input));
            }
        }
    }
}