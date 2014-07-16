using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Videotheek_DLL
{
    public class VideoDbManager
    {
        private static ConnectionStringSettings conVideoSetting = ConfigurationManager.ConnectionStrings["Video"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conVideoSetting.ProviderName);

        public DbConnection GetConnection()
        {
            var conVideo = factory.CreateConnection();
            conVideo.ConnectionString = conVideoSetting.ConnectionString;
            return conVideo;
        }
    }
}
