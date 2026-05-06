using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class OrderItem
    {
        [Key]
        public int IdOrderItem { get; set; }
       

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("IdProduct")]
        public int IdProduct { get; set; }
        public Product Product { get; set; }
        [ForeignKey("IdOrder")]
        public int IdOrder { get; set; }
        public Order Order { get; set; }
    }
}
