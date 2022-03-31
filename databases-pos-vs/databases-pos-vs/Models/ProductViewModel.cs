using System;
using System.ComponentModel.DataAnnotations;


namespace databseApp.Models
{
    public class ProductViewModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Size { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Should be greated than or equal to 1")]
        public float Price { get; set; }
        [Required]
        public string Name { get; set; }

        public int Category_id { get; set; }
        public int Vendor_id { get; set; }
    }
}