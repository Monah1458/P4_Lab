using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestedLogic;

namespace Testing
{
    public class Test
    {
        [Test]
        public async Task DodajPojazd_Poprawny_DoRejestru()
        {
            var rejestr = new RejestrPojazdow();
            var pojazd = new Pojazd { NumerRejestracyjny = "ABC12345" };

            rejestr.DodajPojazd(pojazd);

            var wynik = rejestr.ZnajdzPoNumerze("ABC12345");
            await Assert.That(wynik).IsNotNull();
        }
        [Test]
        public async Task DodajPojazd_Null_Wyjatek()
        {
            var rejestr = new RejestrPojazdow();

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.Run(() => rejestr.DodajPojazd(null))
            );
        }
        [Test]
        [Arguments("ABCD")]
        [Arguments("")]
        [Arguments(null)]
        public async Task DodajPojazd_BlednyNumer_Wyjatek(string numer)
        {
            var rejestr = new RejestrPojazdow();
            var pojazd = new Pojazd { NumerRejestracyjny = numer };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                Task.Run(() => rejestr.DodajPojazd(pojazd))
            );
        }
        [Test]
        public async Task Ubezpieczenie_Wygasle_Niewazne()
        {
            var ubezpieczenie = new Ubezpieczenie
            {
                DataOd = DateTime.Now.AddYears(-1),
                DataDo = DateTime.Now.AddDays(-1)
            };

            var wynik = ubezpieczenie.CzyWazne();

            await Assert.That(wynik).IsFalse();
        }
        [Test]
        public async Task ZnajdzPoNumerze_BrakPojazdu_Null()
        {   
            var rejestr = new RejestrPojazdow();

            var wynik = rejestr.ZnajdzPoNumerze("XYZ");

            await Assert.That(wynik).IsNull();
        }
        [Test]
        public async Task Wyrejestruj_Pojazd_StatusNaFalse()
        {
            var rejestr = new RejestrPojazdow();
            var pojazd = new Pojazd { NumerRejestracyjny = "ABC12345", CzyZarejestrowany = true };

            rejestr.DodajPojazd(pojazd);

            rejestr.Wyrejestruj("ABC12345");

            var wynik = rejestr.ZnajdzPoNumerze("ABC12345");

            await Assert.That(wynik.CzyZarejestrowany).IsFalse();
        }
        [Test]
        public async Task PojazdyZNieaktualnymOC_ZwracaListe()
        {
            var rejestr = new RejestrPojazdow();

            var pojazd = new Pojazd
            {
                NumerRejestracyjny = "ABC12345",
                Ubezpieczenie = new Ubezpieczenie
                {
                    DataOd = DateTime.Now.AddYears(-1),
                    DataDo = DateTime.Now.AddDays(-1)
                }
            };

            rejestr.DodajPojazd(pojazd);
            rejestr.DodajPojazd(pojazd);
            var wynik = rejestr.PojazdyZNieaktualnymOC();

            await Assert.That(wynik.Count).IsEqualTo(2);
        }
        [Test]
        [Arguments(-1, false)]
        [Arguments(1, true)]
        public async Task Ubezpieczenie_RozneDaty_SprawdzaWaznosc(int dni, bool oczekiwany)
        {
            var ubezpieczenie = new Ubezpieczenie
            {
                DataOd = DateTime.Now.AddDays(-10),
                DataDo = DateTime.Now.AddDays(dni)
            };

            var wynik = ubezpieczenie.CzyWazne();

            await Assert.That(wynik).IsEqualTo(oczekiwany);
        }

    }
}
