using MongoDB.Driver;
using System.Globalization;
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

        public static string? ReadString(string promptMessage = "")
        {
            if(promptMessage != ""){
                Console.WriteLine(promptMessage);
            }
            string data = Console.ReadLine();
            if(data == "")
                return null;
            return data;
        }

        public static DateTime? ReadDate(string promptMessage = "")
        {
            while(true){
                string data = ReadString(promptMessage);
                if(data == null)
                    return null;
                try {
                    return ParseDate(data);
                }
                catch {
                    continue;
                }
            };
        }

    
        public static DateTime ParseDate(string input)
        {
            try
            {
                DateTime date = DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return date;
            }
            catch {
                try {
                    DateTime date = DateTime.ParseExact(input, "d.M.yyyy", CultureInfo.InvariantCulture);
                    return date;
                }
                catch {
                    throw new Exception("Expected date to be in form YYYY-MM-DD or DD.MM.YYYY");
                }
            }
        }
    }
}