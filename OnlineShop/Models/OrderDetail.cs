using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }



        [Display(Name = "Order")]
        public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		public Order Order { get; set; }



		[Display(Name = "Product")]
        public int PorductId { get; set; }
        [ForeignKey("PorductId")]
        public Product Product { get; set; }
    }
}
