using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videotheek_DLL.Classes
{
    public class Genre
    {
        public int GenreNr { get; set; }
        public string GenreNaam { get; set; }

        public Genre (int genreNr, string genreNaam)
        {
            GenreNr = genreNr;
            GenreNaam = genreNaam;
        }
    }
}
