using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;


namespace scanapp.Models
{
    public class Article
    {
        [BsonId]
        [JsonPropertyName("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime? CreatedUtc { get; set; }

        public int ArticleId { get; set; } = 0;

        public string? ImageFileKey { get; set; } = null;

        public int? ContainmentId { get; set; }

        public int? Quantity { get; set; }

        [BsonElement("ArticleName")]
        public string ArticleName { get; set; } = "";

        public string? Description { get; set; }

        public string? ArticleType { get; set; }

        public string? PurchaseNr { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? PurchaseDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
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

        public string? Barcode { get; set; }

        public string? State { get; set; }
    }
}
