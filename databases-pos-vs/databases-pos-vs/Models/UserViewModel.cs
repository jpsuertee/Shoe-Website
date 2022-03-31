using System;
using System.ComponentModel.DataAnnotations;


namespace databseApp.Models
{
    public class UserViewModel
    {
        [Key]
        public int UserID { get; set; }
        public string password { get; set; }
    }
}
