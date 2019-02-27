using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistralTrialAPI.Data
{
    public class User
    {
        public int id { get; set; }
        public String Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string email { get; set; }
    }
}
