using System;
using System.Collections.Generic;
using System.Linq;
using Cherry.Data.Administration;
using Cherry.Data.Globals;
using Cherry.Web.DataContexts;
using Cherry.Web.Globals;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.Search
{
    public class SchoolSearcher : ISearcher<School>
    {
        private DbSet<School> Schools { get; set; }
        private DbSet<City> Cities { get; set; }

        public List<School> Result => _result;
        private List<School> _result;

        public Range QueueRange => _range;
        private Range _range;

        public int MaximumInQueue => _maximuminqueue;
        private int _maximuminqueue;

        public int RequestedCount;
        public int RequestedPage;
        public String RequestedValue;

        private readonly IConfigurationContextDb Database;

        public SchoolSearcher(IConfigurationContextDb _DB, ref int _RequestedCount, ref int _RequestedPage, ref string _RequestedValue)
        {
            RequestedCount = _RequestedCount;
            RequestedPage = _RequestedPage;
            RequestedValue = _RequestedValue;

            Schools = _DB.Schools;
            Cities = _DB.Cities;
            Database = _DB;
        }

        public void Search()
        {
            if (string.IsNullOrEmpty(RequestedValue))
            {
                _maximuminqueue = Schools.Count();
                _range = Range.CalculateFromDivider(RequestedCount, Schools.Count(), RequestedPage);
                _result =  GetAllFromRange(_range);
            }
            else
            {
                List<School> Matches = GetAllMatches(ref RequestedValue);
                _range = Range.CalculateFromDivider(RequestedCount, Matches.Count, RequestedPage);
                _maximuminqueue = Matches.Count;
                _result = Matches.Skip(_range.Min).Take(_range.Max - _range.Min).ToList();
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

        private List<School> GetByCity(string[] city)
        {
            List<School> temp = new List<School>();
            foreach (string tempcity in city)
            {
                var temp2 = (from x in Schools
                             where x.City.Name == tempcity
                             select x).ToList();
                temp.AddRange(temp2);
            }
            return temp;
        }
        private List<School> GetByWord(string[] words)
        {
            List<School> temp = new List<School>();
            foreach (string tempword in words)
            {
                var temp2 = (from x in Schools
                             where x.OfficialName.Contains(tempword)
                             select x).ToList();
                temp.AddRange(temp2);
            }
            return temp;
        }
        private List<School> GetAllFromRange(Range range)
        {
            return Schools.Skip(range.Min).Take(range.Max - range.Min).ToList();
        }
        private List<School> GetAllMatches(ref string RequestedValue)
        {
            string[] Words = GetWords(RequestedValue).Distinct().ToArray();

            string[] prototag = Words.Where(x => IsTag(x)).ToArray();
            if (prototag.Length != 0)
            {
                return new List<School>
                {
                    Database.GetSchool(prototag[0])
                };
            }

            string[] protocity = Words.Where(x => IsCity(x)).ToArray();
            if (protocity.Any())
            {
                List<School> gottenByCity = GetByCity(protocity);

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
