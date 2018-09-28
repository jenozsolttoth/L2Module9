using System;

namespace Module7API.Services.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
