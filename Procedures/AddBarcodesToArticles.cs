using scanapp.Models;
using scanapp;

namespace scanapp {

    internal partial class Procedures {
        public static void AddBarcodeToArticle(List<Article> articles, MongoDbHandler mongoHandler)
        {
            var modifiedArticles = new List<Article>{};
            while (true)
            {
                Console.WriteLine("Barcode to be added to " + modifiedArticles.Count + " articles.");
                Console.WriteLine("Enter article id:");
                int? articleId = Utils.ReadInteger();
                if (articleId == null)
                    break;
                if (articleId == Constants.UNASSIGNED)
                    continue;
                var article = articles.Find(a => a.ArticleId == articleId);
                if (article == null)
                {
                    Console.Write("Article not found!");
                    continue;
                }
                Console.WriteLine(string.Format("Enter Barcode for article '{0}'", article.ArticleName));
                string? barcode = Console.ReadLine();
                if (barcode != null && barcode != "")
                {
                    article.PurchaseNr = barcode;
                    modifiedArticles.Add(article);
                }
                Console.Clear();
            }
            if (modifiedArticles.Count == 0)
                return;
            if (!SelectorMenu<bool>.getYesNoMenu("Do you want to update articles?").runConsoleMenu())
                return;
            var success = mongoHandler.setBarcodeOfArticles(modifiedArticles);
            if (success)
            {
                Console.WriteLine("Barcode of articles successfully updated");
            }
            else
            {
                Console.WriteLine("There was an error updating the barcodes");
            }
            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }

    };

}