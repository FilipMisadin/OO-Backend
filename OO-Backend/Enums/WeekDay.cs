using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OO_Backend.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WeekDay
    {
        [EnumMember(Value = "Monday")] Monday,
        [EnumMember(Value = "Tuesday")] Tuesday,
        [EnumMember(Value = "Wednesday")] Wednesday,
        [EnumMember(Value = "Thursday")] Thursday,
        [EnumMember(Value = "Friday")] Friday,
        [EnumMember(Value = "Saturday")] Saturday,
        [EnumMember(Value = "Sunday")] Sunday
    }
}
