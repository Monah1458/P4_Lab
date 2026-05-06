using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Relations
{
    public class Discount
    {
        [Key]
        public int IdDiscount    { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ?Description { get; set; }
        public float Percentage { get; set; }

        public List<Product> Products { get; set; }
    }
}
