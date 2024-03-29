using System.Text.Json;
using ConsoleMenu;
using scanapp;
using scanapp.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

const string dbName = "Warehouse";
const string host = "mongodb://192.168.0.13:27017";
// const int UNASSIGNED = -1;

Console.WriteLine("Loading database records");

/*
var proj = Builders<Article>.Projection
    .Include(x => x.ArticleName)
    .Include(x=> x.ArticleId)
    .Include(x => x.PurchaseNr)
    .Include(x => x.ExpirationDate);
var resp = coll.Find<Article>(_ => true).Project<Article>(proj).ToList();
*/

MongoDbHandler mongo = new MongoDbHandler(host, dbName);
List<Article> articles = mongo.getArticles();
List<Containment> containments = mongo.getContainments();
// mongo.printArticles();

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

var mainMenu = new SelectorMenu<MainMenuAction>(
    menuActions,
    4,
    "What do you want to do today?\n"
    );
mainMenu.runConsoleMenu();

switch (mainMenu.getData())
{
    case MainMenuAction.ADD_BARCODE_TO_EXISTING:
        Procedures.AddBarcodeToArticle(articles);
        break;
    
    case MainMenuAction.ASSIGN_CONTAINMENT_TO_ARTICLE:
        Procedures.AssignArticlesToContainment(articles, containments);
        break;
}

while (true) {
    string? data = Console.ReadLine();
    if(data == null){
        Console.WriteLine("String should not be null!");
        continue;
    }
    int query = Constants.UNASSIGNED;
    try {
        query = Int32.Parse(data);
    }
    catch {
        Console.WriteLine("Unable to parse data.");
        continue;
    }
    var searchResult = articles.Find(ret => ret.ArticleId == query);
    if(searchResult == null){
        Console.WriteLine("Article id not found");
        continue;
    }
    Console.WriteLine("Article Name: " + searchResult.ArticleName);
    Console.WriteLine(DateTime.Now.ToString());
    Console.WriteLine("Expiration date: " + searchResult.ExpirationDate.ToString());
   // Console.Clear();
}

enum MainMenuAction
{
    ADD_NEW_FOOD,
    ADD_NEW_FOOD_WITH_BARCODE,
    ADD_NEW_NON_FOOD,
    ADD_NEW_NON_FOOD_WITH_BARCODE,
    ADD_BARCODE_TO_EXISTING,
    MATERIAL_CHECK_CONTAINMENT,
    ASSIGN_CONTAINMENT_TO_ARTICLE
}