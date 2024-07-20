using scanapp.Models;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Threading;
using scanapp;

namespace scanapp {

    internal partial class Procedures {
        
        private static int ParseContainmentIdFromString(string data)
        {
            try
            {
                return Int32.Parse(data.Substring(1, data.Length - 1));
            }
            catch
            {
                return Constants.UNASSIGNED;
            }
        }

        private static List<List<String>> GetStringlistFromArticleSet(HashSet<Article> articles, int? containmentId)
        {
            List<List<String>> lst = new List<List<String>>();
            foreach (var item in articles)
            {
                lst.Add(new List<string>{item.ArticleName, (item.ContainmentId == containmentId).ToString()});
            }
            return lst;
        }

        public static async void AssignArticlesToContainment(List<Article> articles, List<Containment> containments, string backendUri){
            string? containment = "";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Specify the containment-id you want to use.");
                containment = Console.ReadLine();
                if (containment == null || containment == "")
                    break;
                if (containment[0] != 'C')
                {
                    Console.WriteLine("The containment ID must start with the character 'C'");
                    Console.ReadLine();
                    continue;
                }
                int parsedContainmentId = ParseContainmentIdFromString(containment);
                if(parsedContainmentId == Constants.UNASSIGNED)
                {
                    Console.WriteLine("Unable to obtain integer from containment string '{0}'", containment);
                    Console.ReadLine();
                    continue;
                }
                Containment? c = containments.Find(c => c.ContainmentId == parsedContainmentId);
                if(c == null)
                {
                    Console.WriteLine("Unable to find containment with id {0}", parsedContainmentId);
                    Console.ReadLine();
                    continue;
                }
                HashSet<Article> articlesToUpdate = new HashSet<Article>();
                List<String> tableHeader = new List<String> { "Article Name", "Already Assigned" };
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Mark the articles in the containment {0} {1}", c.ContainmentId, c.Title);
                    if(articlesToUpdate.Count > 0)
                        new ConsoleTable(GetStringlistFromArticleSet(articlesToUpdate, parsedContainmentId), tableHeader);
                    int articleId = Utils.ReadPositiveInteger();
                    if (articleId == Constants.UNASSIGNED)
                        break;
                    Article? a = articles.Find(a => a.ArticleId == articleId);
                    if( a == null)
                    {
                        Console.WriteLine("Article with id {0} not found.", articleId);
                        Console.ReadLine();
                        continue;
                    }
                    bool isNewAssignment = a.ContainmentId == parsedContainmentId;
                    Console.WriteLine("Article '{0}' | Already in containment? {1}", a.ArticleName, isNewAssignment);
                    articlesToUpdate.Add(a);
                }
                Console.WriteLine("Updating {0} articles.", articlesToUpdate.Count);
                Service apiService  = new Service(backendUri);
                await apiService.SetArticleContainments(parsedContainmentId, articlesToUpdate);
                Console.WriteLine("Update complete..");
                Console.ReadLine();
                break;
            }
            
        }

    };

}