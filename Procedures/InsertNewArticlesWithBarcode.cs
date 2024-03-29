using scanapp.Models;

namespace scanapp {

    internal partial class Procedures {

        public static void InsertNewArticlesWithBarcode(List<Article> articles)
        {
            while (true)
            {
                Console.Write("Enter the barcode: ");
                string? barcode = Console.ReadLine();
                if (barcode == null || barcode == "")
                    return;
                var alreadyExisting = articles.FindAll(article => article.PurchaseNr == barcode);
                if (alreadyExisting == null)
                {
                    // Call insert routine

                    // Add a new article
                    // Enter Article Name
                    // Enter Image URL
                    // Enter expiration date
                }
                else if (alreadyExisting.Count == 1)
                {
                    string menuString =
                        string.Format("Purchase number '{0}' already registered with article #{1} '{2}'",
                        barcode, alreadyExisting[0].ArticleId, alreadyExisting[0].ArticleName);
                    var decision = new SelectorMenu<Constants.AddNewArticleAction>(new List<ConsoleMenuItem<Constants.AddNewArticleAction>>
                    {
                        new ConsoleMenuItem<Constants.AddNewArticleAction>("Duplicate the article", Constants.AddNewArticleAction.DUPLICATE_ARTICLE),
                        new ConsoleMenuItem<Constants.AddNewArticleAction>("Define new article", Constants.AddNewArticleAction.INSERT_NEW_ARTICLE),
                    }, 0, menuString).runConsoleMenu();
                    switch (decision)
                    {
                        case Constants.AddNewArticleAction.INSERT_NEW_ARTICLE:
                            // Call insert routine
                            break;
                        case Constants.AddNewArticleAction.DUPLICATE_ARTICLE:
                            // Call duplication routine
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("There exists multiple articles with same barcode. Handling has to be implemented...");
                    Console.Read();
                    // There exists multiple
                }
            }
        }
    };
}