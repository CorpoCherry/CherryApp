using System;
using System.Collections.Generic;
using System.Text;
using Cherry.Data.Configuration.Security;

namespace Cherry.Data.Configuration.Global
{
    public class ConnectionStringBuilder
    {
        public string ConnectionString { get; }

        public ConnectionStringBuilder(DatabaseLogin login)
        {
            ConnectionString = Build(login.IP, login.Name, login.Login, login.Password, !DatabaseParamaters.IsInDevelopment);
        }

        public ConnectionStringBuilder(string databaseName, string login, string password)
        {
            ConnectionString = Build(databaseName, login, password, !DatabaseParamaters.IsInDevelopment);
        }

        public ConnectionStringBuilder(string databaseName, string login, string password, bool isOnline)
        {
            ConnectionString = Build(databaseName, login, password, isOnline);
        }

        private string Build(string databaseName, string login, string password, bool isOnline)
        {
            string server = isOnline ? "cherryapp.pl" : "localhost";
            string database = "cherry_" + databaseName;

            return $"server={server};database={database};uid={login};pwd={password};pooling=true";
        }

        private string Build(string ip, string databaseName, string login, string password, bool isOnline)
        {
            string server = isOnline ? ip : "localhost";
            string database = "cherry_" + databaseName;

            return $"server={server};database={database};uid={login};pwd={password};pooling=true";
        }
    }
}
