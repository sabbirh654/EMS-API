using EMS.Core.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EMS.Core.Entities;

public class OperationLog
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string OperationType { get; set; } = null!;
    public string EntityName { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string Details { get; set; } = null!;
    public string Date { get; set; }
    public string Time { get; set; }

    public OperationLog(string type, string name, string id, string details)
    {
        OperationType = type;
        EntityName = name;
        EntityId = id;
        Details = details;
        Date = DateTime.Now.ToString("yyyy/MM/dd");
        Time = DateTime.Now.ToShortTimeString();
    }
}
