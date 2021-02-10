﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserManagement
{
    public class UserGetModel : IUniqueModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string NickName { get; set; }
    }
}