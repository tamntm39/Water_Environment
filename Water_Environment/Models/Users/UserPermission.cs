using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Water_Environment.Models.Users
{
    public class UserPermission
    {
        public User User { get; set; }
        public string PermissionName { get; set; }
    }
}