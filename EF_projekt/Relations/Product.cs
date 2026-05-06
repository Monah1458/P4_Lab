using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ?Country { get; set; }

        [ForeignKey("IdSupplier")]
        public int IdSupplier { get; set; }
        public Supplier Supplier { get; set; }
        public List<Category> Categories { get; set; }
        public List<Discount> Discounts { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
