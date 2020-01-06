using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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
