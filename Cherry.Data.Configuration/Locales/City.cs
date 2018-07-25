using System.ComponentModel.DataAnnotations;

namespace Cherry.Data.Configuration.Locales
{
    public class City
    {
        [Key]
        public string Name { get; set; }
    }
}
