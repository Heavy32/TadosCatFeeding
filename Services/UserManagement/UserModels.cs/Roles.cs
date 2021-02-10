using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Services.UserManagement
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        User = 1,
        Admin = 2
    }
}

