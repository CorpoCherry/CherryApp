using Cherry.Data.Configuration.Customers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cherry.Data.Configuration.Locales
{
    public class City
    {
        [Key]
        public string Name { get; set; }

        public ICollection<Tenant> Tenants { get; set; }
    }
}
