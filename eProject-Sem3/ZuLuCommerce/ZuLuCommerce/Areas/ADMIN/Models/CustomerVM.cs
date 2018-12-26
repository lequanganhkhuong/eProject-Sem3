using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZuLuCommerce.Areas.ADMIN.Models
{
    public class CustomerVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> Birthday { get; set; }
    }
}