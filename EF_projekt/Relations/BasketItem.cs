using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class BasketItem
    {
        [Key]
        public int IdBasketItem { get; set; }

        [ForeignKey("IdBasket")]
        public int IdBasket { get; set; }
        
        public Basket Basket { get; set; }

        [ForeignKey("IdProduct")]
        public int IdProduct { get; set; }
        

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
