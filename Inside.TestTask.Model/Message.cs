
using System.Text.Json.Serialization;

namespace Inside.TestTask.Model;
public class Message
{
    public int Id {get;set;}
    public int Session_id { get; set; }
    [JsonPropertyName("MC1_timestamp")]
    public DateTime MC1_timestamp { get; set; }
    [JsonPropertyName("MC2_timestamp")]
    public DateTime MC2_timestamp { get; set; }
    [JsonPropertyName("MC3_timestamp")]
    public DateTime MC3_timestamp { get; set; }
    public DateTime End_timestamp { get; set; }

}
