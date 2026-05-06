using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Supplier
    {
        [Key]
        public int IdSupplier { get; set; }
        public string SupplierName { get; set; }

        public List<Product> Products { get; set; }
        public string Country { get; internal set; }
    }
}
