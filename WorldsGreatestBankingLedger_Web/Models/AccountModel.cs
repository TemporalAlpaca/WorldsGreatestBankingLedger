using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsGreatestBankingLedger_Web.Models
{
    public class AccountModel
    {
        public AccountModel() { }
        public AccountModel(string username, string password, string name, string email, string id)
        {
            Username = username;
            Password = password;
            Name = name;
            Email = email;
            Id = id;
            Balance = 0;
        }
        public string Id { get; set; }
        public string Password { get; set; }
        public float Balance { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
