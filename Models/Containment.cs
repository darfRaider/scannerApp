using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace scanapp.Models;

public class Containment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int ContainmentId { get; set; }

    public int? ParentContainmentId {get;set;}

    public string Title {get;set;} = null!;

    public string? TitleLong {get;set;} = null!;

    public string? Comment {get;set;} = null!;

    public string ContainmentType {get;set;} = null!;

    public string? Description { get; set; } = null!;

    public string? Location { get; set; }

    public double? Weight { get; set; }

    public int? Capacity { get; set; }

    public bool IsDeleted { get; set; }

}
