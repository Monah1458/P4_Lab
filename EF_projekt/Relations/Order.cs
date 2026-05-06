using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public DateTime OrderDate { get; set; }
        
        [ForeignKey("IdClient")]
        public int IdClient { get; set; }
        
        public Client Client { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
