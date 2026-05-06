using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Basket
    {
        [Key]
        public int IdBasket { get; set; }

        [ForeignKey("IdClient")]
        public int IdClient { get; set; }
        public Client ?Client { get; set; }

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
    }
}
