using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Users
{
  public class User
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public bool IsAdmin { get; set; }

    }
}
