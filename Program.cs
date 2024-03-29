using scanapp;
using scanapp.Models;


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
MongoDbHandler mongo = new MongoDbHandler(host, Constants.DatabaseName);

List<Article> articles = mongo.getArticles();
List<Containment> containments = mongo.getContainments();

var menuActions = new List<ConsoleMenuItem<MainMenuAction>> {
    new ConsoleMenuItem<MainMenuAction>("Add new article with barcode", MainMenuAction.ADD_NEW_ARTICLE_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new food article", MainMenuAction.ADD_NEW_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article", MainMenuAction.ADD_NEW_NON_FOOD),
    new ConsoleMenuItem<MainMenuAction>("Add new non-food article with barcode", MainMenuAction.ADD_NEW_NON_FOOD_WITH_BARCODE),
    new ConsoleMenuItem<MainMenuAction>("Add barcode to existing article", MainMenuAction.ADD_BARCODE_TO_EXISTING),
    new ConsoleMenuItem<MainMenuAction>("Containment material check", MainMenuAction.MATERIAL_CHECK_CONTAINMENT),
    new ConsoleMenuItem<MainMenuAction>("Quit", MainMenuAction.QUIT)
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
            Procedures.AddBarcodeToArticle(articles, mongo);
            break;
        case MainMenuAction.ADD_NEW_ARTICLE_WITH_BARCODE:
            Procedures.InsertNewArticlesWithBarcode(articles);
            break;
        case MainMenuAction.ASSIGN_CONTAINMENT_TO_ARTICLE:
            Procedures.AssignArticlesToContainment(articles, containments);
            break;
        case MainMenuAction.QUIT:
            return 0;
    }
    /*
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
    */
}

enum MainMenuAction
{
    ADD_NEW_ARTICLE_WITH_BARCODE,
    ADD_NEW_FOOD,
    ADD_NEW_FOOD_WITH_BARCODE,
    ADD_NEW_NON_FOOD,
    ADD_NEW_NON_FOOD_WITH_BARCODE,
    ADD_BARCODE_TO_EXISTING,
    MATERIAL_CHECK_CONTAINMENT,
    ASSIGN_CONTAINMENT_TO_ARTICLE,
    QUIT
}