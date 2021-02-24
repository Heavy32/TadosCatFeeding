using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BusinessLogic.UserManagement
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Roles
    {
        User = 1,
        Admin = 2
    }
}

