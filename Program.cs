using scanapp;
using scanapp.Models;



 List<ConsoleMenuItem<DevEnvironment>> environmentList = new List<ConsoleMenuItem<DevEnvironment>>
{
    new ConsoleMenuItem<DevEnvironment>("localhost", DevEnvironment.LOCALHOST),
    new ConsoleMenuItem<DevEnvironment>("Raspberrypi 5", DevEnvironment.RASPBERRY_PI_5),
    new ConsoleMenuItem<DevEnvironment>("Production NAS", DevEnvironment.PRODUCTION_NAS)
};

Dictionary<DevEnvironment, String> mongoHosts = new Dictionary<DevEnvironment, String>();

mongoHosts.Add(DevEnvironment.LOCALHOST, "mongodb://localhost:27017");
mongoHosts.Add(DevEnvironment.RASPBERRY_PI_5, "mongodb://192.168.0.13:27017");
mongoHosts.Add(DevEnvironment.PRODUCTION_NAS, "mongodb://192.168.0.5:27017");

Dictionary<DevEnvironment, String> backendApiHosts = new Dictionary<DevEnvironment, String>();
backendApiHosts.Add(DevEnvironment.LOCALHOST, "http://localhost:5269/");
backendApiHosts.Add(DevEnvironment.RASPBERRY_PI_5, "http://192.168.0.13:5269/");
backendApiHosts.Add(DevEnvironment.PRODUCTION_NAS, "http://192.168.0.5:5269/");



DevEnvironment env = new SelectorMenu<DevEnvironment>(environmentList, 1, "Select Development Ennvironment").runConsoleMenu();
string host = mongoHosts[env];
string backendUri = backendApiHosts[env];


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
    new ConsoleMenuItem<MainMenuAction>("Assign articles to containment", MainMenuAction.ASSIGN_CONTAINMENT_TO_ARTICLE),
    new ConsoleMenuItem<MainMenuAction>("Quit", MainMenuAction.QUIT)
};

var mainMenu = new SelectorMenu<MainMenuAction>(
    menuActions,
    7,
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
            Procedures.InsertNewArticlesWithBarcode(articles, backendUri);
            break;
        case MainMenuAction.ASSIGN_CONTAINMENT_TO_ARTICLE:
            Procedures.AssignArticlesToContainment(articles, containments);
            break;
        case MainMenuAction.QUIT:
            return 0;
    }
}

enum DevEnvironment {
    LOCALHOST,
    RASPBERRY_PI_5,
    PRODUCTION_NAS
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