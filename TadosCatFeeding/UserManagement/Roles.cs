using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace TadosCatFeeding.UserManagement
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        User = 1,
        Admin = 2
    }
}

