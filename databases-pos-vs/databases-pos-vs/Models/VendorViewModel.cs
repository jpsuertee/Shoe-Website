using System;
using System.ComponentModel.DataAnnotations;

namespace databaseApp.Models
{
    public class VendorViewModel
    {
        [Key]
        public int InventoryID { get; set; }
        public int VendorID { get; set; }
        public string Location { get; set; }
        public int SupervisorID { get; set; }


    }
}

