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
    public class GenreService
    {
        public ObservableCollection<Genre> GetGenres()
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
            var db = new VideoDbManager();

            using (var conVideo = db.GetConnection())
            {
                using (var comGenre = conVideo.CreateCommand())
                {
                    comGenre.CommandType = CommandType.Text;
                    comGenre.CommandText = "SELECT * FROM vdo_genres ORDER BY Genre";
                    conVideo.Open();
                    using (var rdrGenre = comGenre.ExecuteReader())
                    {
                        Int32 genreNrPos = rdrGenre.GetOrdinal("GenreNr");
                        Int32 genrePos = rdrGenre.GetOrdinal("Genre");
                        while (rdrGenre.Read())
                        {
                            genres.Add(new Genre(rdrGenre.GetInt32(genreNrPos), rdrGenre.GetString(genrePos)));
                        }
                    }
                }
            }
            return genres;
        }        
    }
}
