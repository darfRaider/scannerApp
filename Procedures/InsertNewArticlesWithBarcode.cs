using scanapp.Models;
using scanapp;
using System.Diagnostics;


namespace scanapp {


    public enum InsertionAction {
        InsertNew,
        Duplicate
    }
    public class InsertArticle {
        private Article article;
        private int copies = 1;

        public InsertionAction insertionAction;

        public InsertArticle(Article article, InsertionAction insertionAction, int copies){
            this.article = article;
            this.copies = copies;
            this.insertionAction = insertionAction;
        }

        public async Task process(Service apiService){
            switch(this.insertionAction){
                case InsertionAction.InsertNew:
                    await this.InsertNew(apiService);
                    break;
                case InsertionAction.Duplicate:
                    await this.Duplicate(apiService);
                    break;
            }
        }

        private async Task Duplicate(Service apiService){
            for (int input = 0; input < this.copies; input++) {
                await apiService.DuplicateArticle(this.article.ArticleId, article);
            }
        }
        private async Task InsertNew(Service apiService){
            for (int input = 0; input < this.copies; input++) {
                await apiService.InsertArticle(article);
            }
        }
    }

    internal partial class Procedures {

        private static bool IsDuplicatedArticle(Article a, Article b)
        {
            if (a.ArticleName != b.ArticleName) return false;
            if (a.ImageFileKey != b.ImageFileKey) return false;
            return true;
        }


        public static async void InsertNewArticlesWithBarcode(List<Article> articles, string backendUri)
        {
            bool addExpirationDate = SelectorMenu<bool>.getYesNoMenu("Do you want to set expiration date?", true).runConsoleMenu();
            bool setNewArticlesFlagged = SelectorMenu<bool>.getYesNoMenu("Do you want new artices to be flagged?", true).runConsoleMenu();
            List<InsertArticle> articlesToBeProcessed = new List<InsertArticle>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine(String.Format("Staged Articles: {0} (new) {1} (duplicated)", articlesToBeProcessed.FindAll(x => x.insertionAction==InsertionAction.InsertNew).Count, articlesToBeProcessed.FindAll(x => x.insertionAction==InsertionAction.Duplicate).Count));
                Console.Write("Enter the barcode: ");
                string? barcode = Console.ReadLine();
                if (barcode == null || barcode == "")
                    break;
                DateTime? expirationDate = null;
                if (addExpirationDate)
                    expirationDate = Utils.ReadDate("Enter Expiration Date: ");
                int numberOfCopies = Utils.ReadInteger("Number of copies: ", 1);
                var alreadyExistingLst = articles.FindAll(article => article.Barcode == barcode).Distinct(new ArticleComparer()).ToList();
                if (alreadyExistingLst == null || alreadyExistingLst.Count == 0)
                {
                    Article newArticle = new Article();
                    newArticle.Barcode = barcode;
                    newArticle.IsFlagged = setNewArticlesFlagged;
                    newArticle.ExpirationDate = expirationDate;
                    newArticle.ArticleName = Utils.ReadString("Article Name: ");
                    articlesToBeProcessed.Add(new InsertArticle(newArticle, InsertionAction.InsertNew, numberOfCopies));
                    // Call insert routine
                    // Add a new article
                    // Enter Image URL
                }
                else if (alreadyExistingLst.Count == 1)
                {
                    Article existing = alreadyExistingLst[0];
                    existing.Comment = null;
                    existing.State = null;
		    existing.Quantity = null;
                    existing.PurchaseDate = null;
                    existing.Location = null;
                    existing.ExpirationDate = expirationDate;
                    if(setNewArticlesFlagged)
                        existing.IsFlagged = true;
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
                            articlesToBeProcessed.Add(new InsertArticle(existing, InsertionAction.Duplicate, numberOfCopies));
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
            foreach(var article in articlesToBeProcessed)
                await article.process(apiService);
            // foreach(var article in articlesToBeDuplicated)
            //     await apiService.DuplicateArticle(article.ArticleId, article);
            // articlesToBeInserted.ForEach(article => );
            // articlesToBeDuplicated.ForEach(article => apiService.DuplicateArticle(article.ArticleId, article));
        }
    };
}
