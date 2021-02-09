using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding
{
    public enum ServiceResultStatus
    {
        PetIsNotShared,
        PetIsShared,
        CantShareWithUser,
        IncorrectLoginPassword,
        ItemCreated,
        ItemChanged,
        ItemDeleted,
        ItemRecieved,
        ItemNotFound,
        ItemIsNotCreated,
        NoContent
    }
}
