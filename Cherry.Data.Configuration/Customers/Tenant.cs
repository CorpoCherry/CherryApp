using System.ComponentModel.DataAnnotations;
using Cherry.Data.Configuration.Locales;

namespace Cherry.Data.Configuration.Customers
{
    public class Tenant
    {
        [Key]
        [MaxLength(255)]
        public string Tag { get; set; }

        public string OfficialName { get; set; }
        public string PseudoName { get; set; }
        public string NamedBy { get; set; }

        public City City { get; set; }
        public string Adrress { get; set; }
    }
}
