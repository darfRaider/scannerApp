using ConsoleMenu;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using scanapp.Models;

const string dbName = "Warehouse";
const int UNASSIGNED = -1;

Console.WriteLine("Loading database records");

var client = new MongoClient("mongodb://192.168.0.5:27017");
var db = client.GetDatabase(dbName);
var coll = db.GetCollection<Article>("Articles");
/*
var proj = Builders<Article>.Projection
    .Include(x => x.ArticleName)
    .Include(x=> x.ArticleId)
    .Include(x => x.PurchaseNr)
    .Include(x => x.ExpirationDate);
var resp = coll.Find<Article>(_ => true).Project<Article>(proj).ToList();
*/
var resp = coll.Find<Article>(_ => true).ToList();
Console.WriteLine("Loaded " + resp.Count);
var menulist = new List<string> { "Test", "Test2", "Test3", "Test4" };

var menuActions = new List<ConsoleMenuItem<MainMenuAction>> {
    new ConsoleMenuItem<MainMenuAction>("Add new food article", MainMenuAction.ADD_NEW_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article", MainMenuAction.ADD_NEW_NON_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add barcode to existing article", MainMenuAction.ADD_BARCODE_TO_EXISTING),
    new ConsoleMenuItem<MainMenuAction>("Containment material check", MainMenuAction.MATERIAL_CHECK_CONTAINMENT)
};
var mainMenu = new SelectorMenu<MainMenuAction>(menuActions, 2, "What do you want to do today?\n");
int selectedIdx = mainMenu.runConsoleMenu();
if(selectedIdx != UNASSIGNED)
{
    Console.WriteLine("You have selected menu entry " + menulist[mainMenu.getSelectedIndex()]);
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



enum MainMenuAction
{
    ADD_NEW_FOOD,
    ADD_NEW_FOOD_WITH_BARCODE,
    ADD_NEW_NON_FOOD,
    ADD_NEW_NON_FOOD_WITH_BARCODE,
    ADD_BARCODE_TO_EXISTING,
    MATERIAL_CHECK_CONTAINMENT
}