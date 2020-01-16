using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace OO_Backend.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationStatus
    {
        [EnumMember(Value = "Pending")] Pending, 
        [EnumMember(Value = "Accepted")] Accepted,
        [EnumMember(Value = "Declined")] Declined
    }
}
