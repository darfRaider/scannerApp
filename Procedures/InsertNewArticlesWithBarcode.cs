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


        public static void InsertNewArticlesWithBarcode(List<Article> articles, string backendUri)
        {
            bool addExpirationDate = SelectorMenu<bool>.getYesNoMenu("Do you want to set expiration date?", true).runConsoleMenu();
            bool setNewArticlesFlagged = SelectorMenu<bool>.getYesNoMenu("Do you want new artices to be flagged?", true).runConsoleMenu();
            List<Article> articlesToBeDuplicated = new List<Article>();
            List<Article> articlesToBeInserted = new List<Article>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine(String.Format("Staged Articles: {0} (new) {1} (duplicated)", articlesToBeInserted.Count, articlesToBeDuplicated.Count));
                Console.Write("Enter the barcode: ");
                string? barcode = Console.ReadLine();
                if (barcode == null || barcode == "")
                    break;
                DateTime? expirationDate = Utils.ReadDate("Enter Expiration Date: ");
                var alreadyExistingLst = articles.FindAll(article => article.Barcode == barcode);
                if (alreadyExistingLst == null || alreadyExistingLst.Count == 0)
                {
                    Article newArticle = new Article();
                    newArticle.Barcode = barcode;
                    newArticle.IsFlagged = setNewArticlesFlagged;
                    newArticle.ExpirationDate = expirationDate;
                    newArticle.ArticleName = Utils.ReadString("Article Name: ");
                    articlesToBeInserted.Add(newArticle);
                    // Call insert routine
                    // Add a new article
                    // Enter Image URL
                }
                else if (alreadyExistingLst.Count == 1)
                {
                    Article existing = alreadyExistingLst[0];
                    existing.ExpirationDate = expirationDate;
                    string menuString =
                        string.Format(
                            "Purchase number '{0}' already registered with article #{1} '{2}'",
                            barcode,
                            existing.ArticleId,
                            existing.ArticleName);
                    var decision = new SelectorMenu<Constants.AddNewArticleAction>(new List<ConsoleMenuItem<Constants.AddNewArticleAction>>
                    {
                        new ConsoleMenuItem<Constants.AddNewArticleAction>(
                            "Duplicate the article",
                            Constants.AddNewArticleAction.DUPLICATE_ARTICLE),
                        new ConsoleMenuItem<Constants.AddNewArticleAction>(
                            "Define new article",
                            Constants.AddNewArticleAction.INSERT_NEW_ARTICLE),
                    }, 0, menuString).runConsoleMenu();
                    switch (decision)
                    {
                        case Constants.AddNewArticleAction.INSERT_NEW_ARTICLE:
                            Console.WriteLine("INSERT NEW ARTICLE ROUTINE");
                            // Call insert routine
                            break;
                        case Constants.AddNewArticleAction.DUPLICATE_ARTICLE:
                            // Call duplication routine
                            articlesToBeDuplicated.Add(existing);
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
            bool performImport = SelectorMenu<bool>.getYesNoMenu("Do you want to import the articles?", false).runConsoleMenu();
            if(!performImport)
                return;
            
            Service apiService  = new Service(backendUri);
            articlesToBeInserted.ForEach(article => apiService.InsertArticle(article));
            articlesToBeDuplicated.ForEach(article => apiService.DuplicateArticle(article.ArticleId, article));
        }
    };
}