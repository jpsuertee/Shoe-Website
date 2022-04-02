using System;
using System.ComponentModel.DataAnnotations;


namespace databseApp.Models
{
    public class CustomerModel
    {
        [Key]
        public int CustomerID { get; set; }
        public string Password { get; set; }
        public string FirstName_ { get; set; }
        public string LastName_ { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
