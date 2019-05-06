using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorldsGreatestBankingLedger_Web.Models
{
    public class AccountModel
    {
        public AccountModel() { }
        public AccountModel(string username, string password, string name, string email)
        {
            Username = username;
            Password = password;
            Name = name;
            Email = email;
            Id = null;
            Balance = 0;
        }
        public string Id { get; set; }
        [Required]
        public string Password { get; set; }
        public float Balance { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
