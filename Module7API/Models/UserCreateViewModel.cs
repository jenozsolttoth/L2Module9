using System;
using System.ComponentModel.DataAnnotations;

namespace Module7API.Models
{
    public class UserCreateViewModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
