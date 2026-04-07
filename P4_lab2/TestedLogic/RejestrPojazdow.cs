using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestedLogic
{
    public class RejestrPojazdow
    {
        private List<Pojazd> pojazdy = new();

        public void DodajPojazd(Pojazd pojazd)
        {
            if (pojazd == null)
                throw new ArgumentNullException();

            if (string.IsNullOrWhiteSpace(pojazd.NumerRejestracyjny))
                throw new ArgumentException("Brak numeru");
            if (!CzyPoprawnyNumer(pojazd.NumerRejestracyjny))
                throw new ArgumentException("Niepoprawny format numeru rejestracyjnego");

            pojazdy.Add(pojazd);
        }
        private bool CzyPoprawnyNumer(string numer)
        {
            return Regex.IsMatch(numer, @"^[A-Za-z]{3}[0-9]{5}$");
        }
        public Pojazd ZnajdzPoNumerze(string numer)
        {
            return pojazdy.FirstOrDefault(p => p.NumerRejestracyjny == numer);
        }

        public List<Pojazd> ZnajdzPoWlascicielu(string wlasciciel)
        {
            return pojazdy.Where(p => p.Wlasciciel == wlasciciel).ToList();
        }

        public void Wyrejestruj(string numer)
        {
            var pojazd = ZnajdzPoNumerze(numer);
            if (pojazd == null)
                throw new Exception("Nie znaleziono");

            pojazd.CzyZarejestrowany = false;
        }

        public List<Pojazd> PojazdyZNieaktualnymOC()
        {
            return pojazdy.Where(p => !p.Ubezpieczenie.CzyWazne()).ToList();
        }
    }
}
