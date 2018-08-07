using System;
using System.Collections.Generic;
using System.Linq;
using Cherry.Data.Configuration;
using Cherry.Data.Configuration.Customers;
using Cherry.Data.Configuration.Locales;
using Cherry.Web.Globals;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.Search
{
    public class SchoolSearcher : ISearcher<Tenant>
    {
        private DbSet<Tenant> Schools { get; set; }
        private DbSet<City> Cities { get; set; }

        public List<Tenant> Result { get; private set; }
        public Range QueueRange { get; private set; }
        public int MaximumInQueue { get; private set; }

        public int RequestedCount;
        public int RequestedPage;
        public String RequestedValue;

        private readonly IConfigurationContext Database;

        public SchoolSearcher(IConfigurationContext _DB, ref int _RequestedCount, ref int _RequestedPage, ref string _RequestedValue)
        {
            RequestedCount = _RequestedCount;
            RequestedPage = _RequestedPage;
            RequestedValue = _RequestedValue;

            Schools = _DB.Tenants;
            Cities = _DB.Cities;
            Database = _DB;
        }

        public void Search()
        {
            if (string.IsNullOrEmpty(RequestedValue))
            {
                MaximumInQueue = Schools.Count();
                QueueRange = Range.CalculateFromDivider(RequestedCount, Schools.Count(), RequestedPage);
                Result =  GetAllFromRange(QueueRange);
            }
            else
            {
                List<Tenant> Matches = GetAllMatches(ref RequestedValue);
                QueueRange = Range.CalculateFromDivider(RequestedCount, Matches.Count, RequestedPage);
                MaximumInQueue = Matches.Count;
                Result = Matches.Skip(QueueRange.Min).Take(QueueRange.Max - QueueRange.Min).ToList();
            }
        }

        private string[] GetWords(string input)
        {
            return input.Split(new char[] { ' ', ',' });
        }

        private bool IsCity(string word)
        {
            var data = from x in Cities
                       where x.Name == word
                       select x.Name;
            if (data.Any())
                return true;
            return false;
        }
        private bool IsTag(string word)
        {
            var data = from x in Schools
                       where x.Tag == word
                       select x.Tag;
            if (data.Any())
                return true;
            return false;
        }

        private List<Tenant> GetByCity(string[] city)
        {
            List<Tenant> temp = new List<Tenant>();
            foreach (string tempcity in city)
            {
                var temp2 = (from x in Schools
                             where x.City.Name == tempcity
                             select x).ToList();
                temp.AddRange(temp2);
            }
            return temp;
        }
        private List<Tenant> GetByWord(string[] words)
        {
            List<Tenant> temp = new List<Tenant>();
            foreach (string tempword in words)
            {
                var temp2 = (from x in Schools
                             where x.OfficialName.Contains(tempword)
                             select x).ToList();
                temp.AddRange(temp2);
            }
            return temp;
        }
        private List<Tenant> GetAllFromRange(Range range)
        {
            return Schools.Skip(range.Min).Take(range.Max - range.Min).ToList();
        }
        private List<Tenant> GetAllMatches(ref string RequestedValue)
        {
            string[] Words = GetWords(RequestedValue).Distinct().ToArray();

            string[] prototag = Words.Where(x => IsTag(x)).ToArray();
            if (prototag.Length != 0)
            {
                return new List<Tenant>
                {
                    Database.GetTenant(prototag[0])
                };
            }

            string[] protocity = Words.Where(x => IsCity(x)).ToArray();
            if (protocity.Any())
            {
                List<Tenant> gottenByCity = GetByCity(protocity);

                Words = Words.Where(x => !IsCity(x)).ToArray();
                if (Words.Any())
                {
                    return GetByWord(Words).Intersect(gottenByCity).ToList();
                }

                return gottenByCity;
            }

            return GetByWord(Words);

        }
    }
}
