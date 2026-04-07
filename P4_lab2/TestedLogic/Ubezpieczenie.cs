using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestedLogic
{
    public class Ubezpieczenie
    {
        public string NumerPolisy { get; set; }
        public string Firma { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }

        public bool CzyWazne()
        {
            return DateTime.Now >= DataOd && DateTime.Now <= DataDo;
        }
    }
}
