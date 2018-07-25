using System.ComponentModel.DataAnnotations;

namespace Cherry.Data.Configuration.Security
{
    public class DatabaseLogin
    {
        [Key]
        public string Name { get; set; }

        public string IP { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
