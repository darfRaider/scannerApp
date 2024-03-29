using System.Text.Json;
using ConsoleMenu;
using scanapp;
using scanapp.Models;

const string dbName = "Warehouse";
const int UNASSIGNED = -1;

var mongoHostList = new List<ConsoleMenuItem<String>>
{
    new ConsoleMenuItem<String>("localhost", "mongodb://localhost:27017"),
    new ConsoleMenuItem<String>("Raspberrypi 5", "mongodb://192.168.0.13:27017"),
    new ConsoleMenuItem<String>("Production NAS", "mongodb://192.168.0.5:27017")
};

string? host = new SelectorMenu<String>(mongoHostList, 1, "Select Development Environment").runConsoleMenu();

if(host == null)
{
    return 0;
}

Console.WriteLine("Loading database records");
MongoDbHandler mongo = new MongoDbHandler(host, dbName);
var resp = mongo.getArticles();

var menuActions = new List<ConsoleMenuItem<MainMenuAction>> {
    new ConsoleMenuItem<MainMenuAction>("Add new article with barcode", MainMenuAction.ADD_NEW_ARTICLE_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new food article", MainMenuAction.ADD_NEW_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article", MainMenuAction.ADD_NEW_NON_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add barcode to existing article", MainMenuAction.ADD_BARCODE_TO_EXISTING),
    new ConsoleMenuItem<MainMenuAction>("Containment material check", MainMenuAction.MATERIAL_CHECK_CONTAINMENT)
};

var mainMenu = new SelectorMenu<MainMenuAction>(
    menuActions,
    0,
    "What do you want to do today?\n"
    );


while (true) {
    mainMenu.runConsoleMenu();
    switch (mainMenu.getData())
    {
        case MainMenuAction.ADD_BARCODE_TO_EXISTING:
            addBarcodeToArticle(resp, mongo);
            break;
        case MainMenuAction.ADD_NEW_ARTICLE_WITH_BARCODE:
            insertNewArticlesWithBarcode(resp);
            break;
    }
    /*
    string? data = Console.ReadLine();
    if(data == null){
        Console.WriteLine("String should not be null!");
        continue;
    }
    int query = UNASSIGNED;
    try {
        query = Int32.Parse(data);
    }
    catch {
        Console.WriteLine("Unable to parse data.");
        continue;
    }
    var searchResult = resp.Find(ret => ret.ArticleId == query);
    if(searchResult == null){
        Console.WriteLine("Article id not found");
        continue;
    }
    Console.WriteLine("Article Name: " + searchResult.ArticleName);
    Console.WriteLine(DateTime.Now.ToString());
    Console.WriteLine("Expiration date: " + searchResult.ExpirationDate.ToString());
    */
}

int? readInteger()
{
    string? articleId = Console.ReadLine();
    if (articleId == "" || articleId == null)
    {
        return null;
    }
    int query = UNASSIGNED;
    try
    {
        query = Int32.Parse(articleId);
    }
    catch
    {
        Console.WriteLine("Unable to parse integer.");
    }
    return query;
}


void insertNewArticlesWithBarcode(List<Article> articles)
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
            var decision = new SelectorMenu<AddNewArticleAction>(new List<ConsoleMenuItem<AddNewArticleAction>>
            {
                new ConsoleMenuItem<AddNewArticleAction>("Duplicate the article", AddNewArticleAction.DUPLICATE_ARTICLE),
                new ConsoleMenuItem<AddNewArticleAction>("Define new article", AddNewArticleAction.INSERT_NEW_ARTICLE),
            }, 0, menuString).runConsoleMenu();
            switch (decision)
            {
                case AddNewArticleAction.INSERT_NEW_ARTICLE:
                    // Call insert routine
                    break;
                case AddNewArticleAction.DUPLICATE_ARTICLE:
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

void addBarcodeToArticle(List<Article> articles, MongoDbHandler mongoHandler)
{
    var modifiedArticles = new List<Article>{};
    while (true)
    {
        Console.WriteLine("Barcode to be added to " + modifiedArticles.Count + " articles.");
        Console.WriteLine("Enter article id:");
        int? articleId = readInteger();
        if (articleId == null)
            break;
        if (articleId == UNASSIGNED)
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

enum DeveolpmentEnvironment
{
    PRODUCTION_NAS,
    RASPBERRY_PI_5,
    LOCALHOST,
}

enum AddNewArticleAction
{
    DUPLICATE_ARTICLE,
    INSERT_NEW_ARTICLE
};

enum MainMenuAction
{
    ADD_NEW_ARTICLE_WITH_BARCODE,
    ADD_NEW_FOOD,
    ADD_NEW_FOOD_WITH_BARCODE,
    ADD_NEW_NON_FOOD,
    ADD_NEW_NON_FOOD_WITH_BARCODE,
    ADD_BARCODE_TO_EXISTING,
    MATERIAL_CHECK_CONTAINMENT
}