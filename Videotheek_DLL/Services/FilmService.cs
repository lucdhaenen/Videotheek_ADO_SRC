using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Videotheek_DLL.Classes;

namespace Videotheek_DLL.Services
{
    public class FilmService
    {
        public ObservableCollection<Film> GetFilms()
        {
            ObservableCollection<Film> films = new ObservableCollection<Film>();
            var db = new VideoDbManager();
            using (var conVideo = db.GetConnection())
            {
                using (var comFilms = conVideo.CreateCommand())
                {
                    comFilms.CommandType = CommandType.Text;
                    comFilms.CommandText = "SELECT * FROM vdo_films ORDER BY Titel";
                    try
                    {
                        conVideo.Open();
                        using (var rdrVideo = comFilms.ExecuteReader())
                        {
                            Int32 bandNrPos = rdrVideo.GetOrdinal("BandNr");
                            Int32 titelPos = rdrVideo.GetOrdinal("Titel");
                            Int32 genreNrPos = rdrVideo.GetOrdinal("GenreNr");
                            Int32 inVoorraadPos = rdrVideo.GetOrdinal("InVoorraad");
                            Int32 uitVoorraadPos = rdrVideo.GetOrdinal("UitVoorraad");
                            Int32 prijsPos = rdrVideo.GetOrdinal("Prijs");
                            Int32 totaalVerhuurdPos = rdrVideo.GetOrdinal("TotaalVerhuurd");
                            while (rdrVideo.Read())
                            {
                                films.Add(new Film(
                                    rdrVideo.GetInt32(bandNrPos),
                                    rdrVideo.GetString(titelPos),
                                    rdrVideo.GetInt32(genreNrPos),
                                    rdrVideo.GetInt32(inVoorraadPos),
                                    rdrVideo.GetInt32(uitVoorraadPos),
                                    rdrVideo.GetDecimal(prijsPos),
                                    rdrVideo.GetInt32(totaalVerhuurdPos)));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    
                }
            }   
            return films;            
        }

        public Int32 Toevoegen(Film film)
        {
            var db = new VideoDbManager();
            using (var conVideo = db.GetConnection())
            {
                using (var comToevoegen = conVideo.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.Text;
                    comToevoegen.CommandText = "INSERT INTO vdo_films " + 
                        "(Titel, GenreNr, InVoorraad, UitVoorraad, Prijs, TotaalVerhuurd) " + 
                        "VALUES (@titel, @genreNr, @inVoorraad, @uitVoorraad, @prijs, @totaalVerhuurd); " + 
                        "SELECT LAST_INSERT_ID();";
                    var parTitel = comToevoegen.CreateParameter();
                    parTitel.ParameterName = "@titel";
                    parTitel.Value = film.Titel;
                    comToevoegen.Parameters.Add(parTitel);
                    var parGenreNr = comToevoegen.CreateParameter();
                    parGenreNr.ParameterName = "@genreNr";
                    parGenreNr.Value = film.GenreNr;
                    comToevoegen.Parameters.Add(parGenreNr);
                    var parInVoorraad = comToevoegen.CreateParameter();
                    parInVoorraad.ParameterName = "@inVoorraad";
                    parInVoorraad.Value = film.InVoorraad;
                    comToevoegen.Parameters.Add(parInVoorraad);
                    var parUitVoorraad = comToevoegen.CreateParameter();
                    parUitVoorraad.ParameterName = "@uitVoorraad";
                    parUitVoorraad.Value = film.UitVoorraad;
                    comToevoegen.Parameters.Add(parUitVoorraad);
                    var parPrijs = comToevoegen.CreateParameter();
                    parPrijs.ParameterName = "@prijs";
                    parPrijs.Value = film.Prijs;
                    comToevoegen.Parameters.Add(parPrijs);
                    var parTotaalVerhuurd = comToevoegen.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@totaalVerhuurd";
                    parTotaalVerhuurd.Value = film.TotaalVerhuurd;
                    comToevoegen.Parameters.Add(parTotaalVerhuurd);
                    conVideo.Open();
                    Int32 BandNr = Convert.ToInt32(comToevoegen.ExecuteScalar());
                    if (BandNr == 0)
                    {
                        throw new Exception("Kan film niet toevoegen"); 
                    }
                    else
                    {
                        return BandNr;
                    }
                }
            }
        }

        public void Wijzigen(Film film)
        {            
            var db = new VideoDbManager();
            using (var conVideo = db.GetConnection())
            {
                using (var comWijzigen = conVideo.CreateCommand())
                {
                    comWijzigen.CommandType = CommandType.Text;
                    comWijzigen.CommandText = "UPDATE vdo_films " + 
                        "SET Titel=@titel, GenreNr=@genreNr, InVoorraad=@inVoorraad, UitVoorraad=@uitVoorraad, " + 
                        "Prijs=@prijs, TotaalVerhuurd=@totaalVerhuurd WHERE BandNr=@bandNr";
                    var parBandNr = comWijzigen.CreateParameter();
                    parBandNr.ParameterName = "@bandNr";
                    parBandNr.Value = film.BandNr;
                    comWijzigen.Parameters.Add(parBandNr);
                    var parTitel = comWijzigen.CreateParameter();
                    parTitel.ParameterName = "@titel";
                    parTitel.Value = film.Titel;
                    comWijzigen.Parameters.Add(parTitel);
                    var parGenreNr = comWijzigen.CreateParameter();
                    parGenreNr.ParameterName = "@genreNr";
                    parGenreNr.Value = film.GenreNr;
                    comWijzigen.Parameters.Add(parGenreNr);
                    var parInVoorraad = comWijzigen.CreateParameter();
                    parInVoorraad.ParameterName = "@inVoorraad";
                    parInVoorraad.Value = film.InVoorraad;
                    comWijzigen.Parameters.Add(parInVoorraad);
                    var parUitVoorraad = comWijzigen.CreateParameter();
                    parUitVoorraad.ParameterName = "@uitVoorraad";
                    parUitVoorraad.Value = film.UitVoorraad;
                    comWijzigen.Parameters.Add(parUitVoorraad);
                    var parPrijs = comWijzigen.CreateParameter();
                    parPrijs.ParameterName = "@prijs";
                    parPrijs.Value = film.Prijs;
                    comWijzigen.Parameters.Add(parPrijs);
                    var parTotaalVerhuurd = comWijzigen.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@totaalVerhuurd";
                    parTotaalVerhuurd.Value = film.TotaalVerhuurd;
                    comWijzigen.Parameters.Add(parTotaalVerhuurd);
                    try
                    {
                        conVideo.Open();
                        comWijzigen.ExecuteNonQuery(); 
                    }
                    catch (Exception)
                    {
                        throw new Exception("Kan film niet wijzigen");
                    }                                       
                }
            }
        }

        public void Verwijderen(Film film)
        {
            var db = new VideoDbManager();
            using (var conVideo = db.GetConnection())
            {
                using (var comVerwijder = conVideo.CreateCommand())
                {
                    comVerwijder.CommandType = CommandType.Text;
                    comVerwijder.CommandText = "DELETE from vdo_films WHERE BandNr = @bandNr";
                    var parBandNr = comVerwijder.CreateParameter();
                    parBandNr.ParameterName = "@bandNr";
                    parBandNr.Value = film.BandNr;
                    comVerwijder.Parameters.Add(parBandNr);
                    try
                    {
                        conVideo.Open();
                        comVerwijder.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {                        
                        throw new Exception("Verwijderen mislukt");
                    }                    
                }
            }
        }
    }
}
