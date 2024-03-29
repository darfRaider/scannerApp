using MongoDB.Driver;
using scanapp.Models;

namespace scanapp
{
    internal class MongoDbHandler
    {
        private MongoClient _client;
        private IMongoDatabase _db;
        private IMongoCollection<Article> articleCollection;
        private IMongoCollection<Containment> containmentCollection;

        public MongoDbHandler(string host, string dbName)
        {
            this._client = new MongoClient(host);
            this._db = this._client.GetDatabase(dbName);
            if(this.testConnection()){
                Console.WriteLine("Connection successful.");
            }
            else {
                Console.WriteLine("Connection not successful");
            }
            this.articleCollection = this._db.GetCollection<Article>("Articles");
            this.containmentCollection = this._db.GetCollection<Containment>("Containments");
        
        }

        public bool testConnection(){
            try {
                this._client.ListDatabases();
                return true;
            }
            catch {
                return false;
            }
        }

        public List<Article> getArticles()
        {
            return this.articleCollection.Find<Article>(_ => true).ToList();
        }

        public List<Containment> getContainments()
        {
            return this.containmentCollection.Find(_ => true).ToList();
        }

        private List<string> getStringListFromArticle(Article article)
        {
            return new List<string> { article.ArticleId.ToString(), article.ArticleName };
        }

        public void printArticles()
        {
            var header = new List<string> { "Id", "ArticleName" };
            var articleList = new List<List<string>> { };
            this.getArticles().ForEach(article => { articleList.Add(getStringListFromArticle(article)); });
            var ct = new ConsoleTable(articleList, header);
        }
    }
}
