using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestedLogic
{
    public class Pojazd
    {
        public string NumerRejestracyjny { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int RokProdukcji { get; set; }
        public string Wlasciciel { get; set; }
        public bool CzyZarejestrowany { get; set; }

        public PrzegladTechniczny Przeglad { get; set; }
        public Ubezpieczenie Ubezpieczenie { get; set; }
    }
}
