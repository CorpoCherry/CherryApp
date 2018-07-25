using System;
using System.IO;
using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Security;
using MySql.Data.MySqlClient;

namespace Cherry.Data.Configuration.Global
{
    public class LoginManager
    {
        public DatabaseLogin Login
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    return _configurationContext.DatabaseLogins.Find(_tenant.Tag);
                else
                    return _configurationContext.DatabaseLogins.Find(_name);
            }
        }

        public bool Exists
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    return _configurationContext.DatabaseLogins.Find(_tenant.Tag) != null;
                else
                    return _configurationContext.DatabaseLogins.Find(_name) != null;
            }

        }

        private readonly string _name;
        private readonly string _login;
        private readonly Tenant _tenant;
        private readonly ConfigurationContext _configurationContext;

        public LoginManager(string login, string name, ConfigurationContext configurationContext)
        {
            if (String.IsNullOrEmpty(name))
                throw new Exception("Name can't be empty");
            _name = name;
            _login = login;
            _configurationContext = configurationContext;
        }

        public LoginManager(Tenant tenant, ConfigurationContext configurationContext)
        {
            _tenant = tenant;
            _configurationContext = configurationContext;
        }
        
        public void Create(string IP = null)
        {
            DatabaseLogin databaseLogin = new DatabaseLogin();
            if (string.IsNullOrEmpty(IP))
                databaseLogin.IP = "localhost";
            else
                databaseLogin.IP = IP;

            if (!string.IsNullOrEmpty(_login))
                databaseLogin.Login = _login;
            else
                databaseLogin.Login = _tenant.Tag;

            if (!string.IsNullOrEmpty(_name))
                databaseLogin.Name = _name;
            else
                databaseLogin.Name = _tenant.Tag;

            databaseLogin.Password = BCrypt.Net.BCrypt.HashPassword(Path.GetRandomFileName());

            using (MySqlConnection Connection = new MySqlConnection(_configurationContext.ConnectionString))
            using (MySqlCommand Command = new MySqlCommand($"CREATE USER '{databaseLogin.Login}'@'{databaseLogin.IP}' IDENTIFIED BY '{databaseLogin.Password}'; GRANT ALL ON cherry_{databaseLogin.Name}.* TO '{databaseLogin.Login}'@'{databaseLogin.IP}'; GRANT CREATE ON *.* TO '{databaseLogin.Login}'@'{databaseLogin.IP}';", Connection))
            {
                Connection.Open();
                Command.ExecuteNonQuery();
            }
            

            _configurationContext.DatabaseLogins.Add(databaseLogin);
            _configurationContext.SaveChanges();
        }
    }
}
