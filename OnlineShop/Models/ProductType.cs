using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public string? ProductTypes { get; set; }
    }
}
