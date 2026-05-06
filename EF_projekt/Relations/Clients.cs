using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
        public string Email { get; set; }

        public List<Order> ?Orders { get; set; }
        public Basket Basket { get; set; } = new();
    }
}
