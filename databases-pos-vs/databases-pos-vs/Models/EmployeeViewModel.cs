using System;
using System.ComponentModel.DataAnnotations;


namespace databseApp.Models
{
    public class EmployeeViewModel
    {
        [Key]
        public string EmployeeID { get; set; }
        public string FirstName_ { get; set; }
        public string LastName_ { get; set; }
    }
}
