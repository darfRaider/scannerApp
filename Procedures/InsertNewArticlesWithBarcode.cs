using scanapp.Models;
using scanapp;


namespace scanapp {

    internal partial class Procedures {

        private static bool IsDuplicatedArticle(Article a, Article b)
        {
            if (a.ArticleName != b.ArticleName) return false;
            if (a.ImageFileKey != b.ImageFileKey) return false;
            return true;
        }

        public static void InsertNewArticlesWithBarcode(List<Article> articles)
        {
            bool addExpirationDate = SelectorMenu<bool>.getYesNoMenu("Do you want to set expiration date?", true).runConsoleMenu();
            bool setNewArticlesFlagged = SelectorMenu<bool>.getYesNoMenu("Do you want new artices to be flagged?", true).runConsoleMenu();
            List<Article> articlesToBeDuplicated = new List<Article>();
            List<Article> articlesToBeInserted = new List<Article>();
            while (true)
            {
                Console.Write("Enter the barcode: ");
                string? barcode = Console.ReadLine();
                if (barcode == null || barcode == "")
                    break;
                var alreadyExisting = articles.FindAll(article => article.Barcode == barcode);
                if (alreadyExisting == null || alreadyExisting.Count == 0)
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
                            DateTime? dt = Utils.ReadDate("Enter Expiration Date: ");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("There exists multiple articles with same barcode. Handling has to be implemented...");
                    Console.Read();
                    // There exists multiple
                    // TODO: use IsDuplicatedArticle in order to find out if existing article actually are the same drop_duplicates subset imageFIleKey and articleName
                }
            }
            Console.WriteLine("HERE DO THE INSERION OR DUPLICATION");
            Console.ReadLine();
        }
    };
}