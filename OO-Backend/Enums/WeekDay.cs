using System.Runtime.Serialization;
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
