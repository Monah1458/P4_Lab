using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestedLogic
{
    public class PrzegladTechniczny
    {
        public DateTime DataOstatniego { get; set; }
        public DateTime DataNastepnego { get; set; }

        public bool CzyWazny()
        {
            return DataNastepnego >= DateTime.Now;
        }
    }
}
