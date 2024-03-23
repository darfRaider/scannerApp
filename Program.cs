using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ZstdSharp.Unsafe;

const string dbName = "Warehouse";
const int UNASSIGNED = -1;

Console.WriteLine("Loading database records");

var client = new MongoClient("mongodb://192.168.0.5:27017");
var db = client.GetDatabase(dbName);
var coll = db.GetCollection<Article>("Articles");
var proj = Builders<Article>.Projection
    .Include(x => x.ArticleName)
    .Include(x=> x.ArticleId)
    .Include(x => x.PurchaseNr)
    .Include(x => x.ExpirationDate);
var resp = coll.Find<Article>(_ => true).Project<Article>(proj).ToList();
// var resp2 = coll.Find(_ => true).Project(proj.ToList();

Console.WriteLine("Loaded " + resp.Count);

while(true) {
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
}

public class Article
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime? CreatedUtc { get; set; }

    public int ArticleId { get; set; } = 0;

    public string? ImageFileKey { get; set; } = null;

    public int? ContainmentId { get; set; }

    public int? Quantity { get; set; }

    [BsonElement("ArticleName")]
    public string? ArticleName { get; set; }

    public string? Description { get; set; }

    public string? ArticleType { get; set; }

    public string? PurchaseNr { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? PurchaseDate { get; set; }
    
    [BsonDateTimeOptions()]
    public DateTime? ExpirationDate { get; set; }

    public string? Vendor { get; set; } = null!;

    public float? Price { get; set; }

    public string? Comment { get; set; }
    
    public string[]? Keywords { get; set; }

    public string? Location { get; set; }

    public bool? InStock { get; set; }

    public bool IsFlagged { get; set; } = false;

    public bool IsConsumed { get; set; }

    public double? Weight { get; set; }

    public double? WeightNet { get; set; }
}