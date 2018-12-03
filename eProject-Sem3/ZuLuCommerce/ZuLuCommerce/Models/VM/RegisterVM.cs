using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZuLuCommerce.Models.VM
{
    public class RegisterVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password), MaxLength(30), MinLength(6)]
        public string Password { get; set; }
        [DataType(DataType.Password), MaxLength(30), MinLength(6)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string RePassword { get; set; }
    }
}