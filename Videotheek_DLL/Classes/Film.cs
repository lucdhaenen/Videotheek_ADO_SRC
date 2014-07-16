using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videotheek_DLL.Classes
{
    public class Film
    {
        public int BandNr { get; set; }
        public string Titel { get; set; }
        public int GenreNr { get; set; }
        public int InVoorraad { get; set; }
        public int UitVoorraad { get; set; }
        public decimal Prijs { get; set; }
        public int TotaalVerhuurd { get; set; }

        public Film()
        {
        }
        
        public Film (int bandNr, string titel, int genreNr, int inVoorraad, int uitVoorraad, decimal prijs, int totaalVerhuurd)
        {
            BandNr = bandNr;
            Titel = titel;
            GenreNr = genreNr;
            InVoorraad = inVoorraad;
            UitVoorraad = uitVoorraad;
            Prijs = prijs;
            TotaalVerhuurd = totaalVerhuurd;
        }
    }
}
