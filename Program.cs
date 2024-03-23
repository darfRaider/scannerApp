using ConsoleMenu;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using scanapp;
using scanapp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

const string dbName = "Warehouse";
const int UNASSIGNED = -1;

Console.WriteLine("Loading database records");

/*
var proj = Builders<Article>.Projection
    .Include(x => x.ArticleName)
    .Include(x=> x.ArticleId)
    .Include(x => x.PurchaseNr)
    .Include(x => x.ExpirationDate);
var resp = coll.Find<Article>(_ => true).Project<Article>(proj).ToList();
*/

string host = "mongodb://192.168.0.5:27017";
MongoDbHandler mongo = new MongoDbHandler(host, "Warehouse");
var resp = mongo.getArticles();
mongo.printArticles();

var menuActions = new List<ConsoleMenuItem<MainMenuAction>> {
    new ConsoleMenuItem<MainMenuAction>("Add new food article", MainMenuAction.ADD_NEW_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article", MainMenuAction.ADD_NEW_NON_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add barcode to existing article", MainMenuAction.ADD_BARCODE_TO_EXISTING),
    new ConsoleMenuItem<MainMenuAction>("Containment material check", MainMenuAction.MATERIAL_CHECK_CONTAINMENT)
};


ConsoleTable test = new ConsoleTable(new List<List<string>> { new List<string> { "LoL", "LoLLL" } });
Console.ReadLine();

var mainMenu = new SelectorMenu<MainMenuAction>(menuActions, 4, "What do you want to do today?\n");
mainMenu.runConsoleMenu();
switch (mainMenu.getData())
{
    case MainMenuAction.ADD_BARCODE_TO_EXISTING:
        addBarcodeToArticle(resp);
        break;
}

while (true) {
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
   // Console.Clear();
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

void addBarcodeToArticle(List<Article> articles)
{
    int successCount = 0;
    while (true)
    {
        Console.WriteLine("Barcode to be added to " + successCount.ToString() + " articles.");
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
        if (barcode != null)
        {
            article.PurchaseNr = barcode;
            successCount++;
        }
        Console.Clear();
    }
    if (successCount == 0)
        return;
    
}


enum MainMenuAction
{
    ADD_NEW_FOOD,
    ADD_NEW_FOOD_WITH_BARCODE,
    ADD_NEW_NON_FOOD,
    ADD_NEW_NON_FOOD_WITH_BARCODE,
    ADD_BARCODE_TO_EXISTING,
    MATERIAL_CHECK_CONTAINMENT
}