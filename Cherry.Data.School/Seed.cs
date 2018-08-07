using Cherry.Data.Configuration;
using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Locales;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cherry.Data.School
{
    public static class Seed
    {
        private static readonly Tenant[] Tenants =
        {
            new Tenant
            {
                OfficialName = "Zespół Szkół Podstawowych nr 2",
                PseudoName = "ZSP2",
                NamedBy = "Kornel Makuszyński",
                City = new City { Name = "Legionowo" },
                Adrress = "Jagiellońska 67",
                Tag = "zsp2legionowo"
            },
            new Tenant
            {
                OfficialName = "Szkoła Podstawowa nr 314",
                PseudoName = "SP314",
                NamedBy = "Przyjaciół Ziemi",
                City = new City { Name = "Warszawa" },
                Adrress = "Porajów 3",
                Tag = "sp314warszawa"
            },
            new Tenant
            {
                OfficialName = "Zespół Szkół Samochodowych",
                PseudoName = "ZSS",
                NamedBy = "Sportowców Ziemii Szczecińskiej",
                City = new City { Name = "Szczecin" },
                Adrress = "Małopolska 22",
                Tag = "zssszczecin"
            },
            new Tenant
            {
                OfficialName = "Zespół Szkół Specjalnych nr 2",
                PseudoName = "ZSS2",
                NamedBy = null,
                City = new City { Name = "Gdańsk" },
                Adrress = "Witastwosza 23",
                Tag = "zss2gdańsk"
            },
            new Tenant
            {
                OfficialName = "Sopocka Szkoła Montessori",
                PseudoName = "SSP",
                NamedBy = null,
                City = new City { Name = "Sopot" },
                Adrress = "Tatrzańska 19",
                Tag = "sspsopot"
            },
            new Tenant
            {
                OfficialName = "Szkoła Podstawowa nr 84",
                PseudoName = "SP",
                NamedBy = "Ruch Oporu Pokoju",
                City = new City { Name = "Wrocław" },
                Adrress = "Górnickiego 20",
                Tag = "spwroclaw"
            },
            new Tenant
            {
                OfficialName = "XII Liceum Ogólnokształcące",
                PseudoName = "XIILO",
                NamedBy = "M. Skłodwska-Curie",
                City = new City { Name = "Poznań" },
                Adrress = "Kutrzeby 8",
                Tag = "XIIlieceumogolnoksztalcace"
            },
            new Tenant
            {
                OfficialName = "Wyższa Szkoła Techniczna",
                PseudoName = "WST",
                NamedBy = null,
                City = new City { Name = "Katowice" },
                Adrress = "Rolna 43",
                Tag = "wyzszaszkolatechniczna"
            },
            new Tenant
            {
                OfficialName = "Śląski Uniwersytet Medyczny",
                PseudoName = "ŚUM",
                NamedBy = null,
                City = new City { Name = "Katowice" },
                Adrress = "Poniatowskiego 15",
                Tag = "slaskiuniwerekmedyczny"
            },
            new Tenant
            {
                OfficialName = "Szkoła Podstawowa nr 4",
                PseudoName = "SP",
                NamedBy = "T. Kościuszki. Filla",
                City = new City { Name = "Katowice" },
                Adrress = "Józefowska 52/54",
                Tag = "szkolapodtawowakatowice"
            }
        };

        public static async Task School(ConfigurationContext configurationContext)
        {
            if(await configurationContext.Tenants.FindAsync(Tenants[0].Tag) == null)
            foreach (Tenant tenant in Tenants)
            {
                await configurationContext.AddAsync(tenant);
                await configurationContext.SaveChangesAsync();
            }    
        }
    }
}
