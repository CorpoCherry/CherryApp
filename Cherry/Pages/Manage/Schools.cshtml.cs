using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Data.Schools;
using Cherry.Web.DataContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cherry.Web.Pages.Manage
{
    public class SchoolsModel : PageModel
    {
        private readonly IConfigurationContextDb configuration;

        public DbSet<School> Schools { get; set; }

        private List<ReturnedSchool> SchoolList;

        public SchoolsModel(IConfigurationContextDb _configuration)
        {
            SchoolList = new List<ReturnedSchool>();
            configuration = _configuration;
            Schools = configuration.Schools;
        }

        public Task<JsonResult> OnPostGetByName(string name)
        {
            IQueryable<School> ReturnedData = null;
            int no = 0;

            List<string> WordTags = name.Split(' ').ToList();

            //TODO: Modify to use preset values instead of fetching them all from database
            var Cities = (from cits in Schools
                          select cits.City).Distinct();

            var Tags = (from cits in Schools
                        select cits.Tag).Distinct();

            //TODO: Modify to display pages instead of returning all matches

            switch (FindInList(WordTags, ref Cities, ref Tags, out string Tag, out string City, out string Other))
            {
                case TypesInList.Other:
                    ReturnedData = from sch in Schools
                                   where sch.OfficialName.Contains(Other)
                                   select sch;
                    break;
                case TypesInList.City:
                    ReturnedData = from sch in Schools
                                   where sch.City.Contains(City)
                                   select sch;
                    break;
                case TypesInList.Tag:
                    ReturnedData = from sch in Schools
                                   where sch.Tag.Contains(Tag)
                                   select sch;
                    break;
                case TypesInList.CityAndOther:
                    ReturnedData = from sch in Schools
                                   where sch.OfficialName.Contains(Other)
                                   where sch.City.Contains(City)
                                   select sch;
                    break;
                case TypesInList.TagAndOther:
                    ReturnedData = from sch in Schools
                                   where sch.OfficialName.Contains(Other)
                                   where sch.Tag.Contains(Tag)
                                   select sch;
                    break;
                case TypesInList.CityAndTag:
                    ReturnedData = from sch in Schools
                                   where sch.Tag.Contains(Tag)
                                   where sch.City.Contains(City)
                                   select sch;
                    break;
                case TypesInList.CityAndTagAndOther:
                    ReturnedData = from sch in Schools
                                   where sch.Tag.Contains(Tag)
                                   where sch.City.Contains(City)
                                   where sch.OfficialName.Contains(Other)
                                   select sch;
                    break;
            }

            foreach (var school in ReturnedData)
            {
                SchoolList.Add(new ReturnedSchool
                {
                    OfficialName = school.OfficialName,
                    Tag = school.Tag
                });
                no++;
            }

            return Task.FromResult(new JsonResult(SchoolList));
        }
        public Task<JsonResult> OnPostGetPage(string id)
        {
            DbSet<School> Schools = configuration.Schools;
            int no = 0;

            int selectedpage = Convert.ToInt16(id) * 15;
            var query = (from sch in Schools select sch).Skip(selectedpage).Take(15);
            foreach (var school in query)
            {
                SchoolList.Add(new ReturnedSchool
                {
                    OfficialName = school.OfficialName,
                    Tag = school.Tag
                });
                no++;
            }
            return Task.FromResult(new JsonResult(SchoolList));
        }
        public Task<JsonResult> OnPostGetSchool(string tag)
        {
            School ReturnedData = null;
            try
            {
                ReturnedData = (from sch in Schools
                              where sch.Tag == tag
                              select sch).Single();
            }
            catch
            {
                return Task.FromResult(new JsonResult("School not found"));
            }

            return Task.FromResult(new JsonResult(new ReturnedSchool
            {
                OfficialName = ReturnedData.OfficialName,
                NamedBy = ReturnedData.NamedBy,
                PseudoName = ReturnedData.PseudoName,
                City = ReturnedData.City,
                Address = ReturnedData.Adrress,
                Tag = ReturnedData.Tag,
                Country = ReturnedData.Country
            }));
        }

        private class ReturnedSchool
        {
            public string OfficialName;
            public string NamedBy;
            public string PseudoName;
            public string Tag;
            public string City;
            public string Address;
            public string Country;
        }
        public bool CityInList(ref List<string> WordTags, ref IQueryable<string> DbCities, out string Result)
        {
            foreach (string WordTag in WordTags)
            {
                if (DbCities.Contains(WordTag))
                {
                    Result = WordTag;
                    return true;
                }
            }
            Result = null;
            return false;
        }
        public bool TagInList(ref List<string> WordTags, ref IQueryable<string> DbTags, out string Result)
        {
            foreach (string WordTag in WordTags)
            {
                if (DbTags.Contains(WordTag))
                {
                    Result = WordTag;
                    return true;
                }
            }
            Result = null;
            return false;
        }

        public TypesInList FindInList(List<string> WordTags, ref IQueryable<string> DbCities, ref IQueryable<string> DbTags, out string TagResult, out string CityResult, out string OtherResult)
        {
            //TODO: Optimize
            bool FoundCity = CityInList(ref WordTags, ref DbCities, out string _CityResult);
            bool FoundTag = CityInList(ref WordTags, ref DbTags, out string _TagResult);

            if (FoundCity)
            {
                WordTags.RemoveAll(x => x.Contains(_CityResult));
            }
            if(FoundTag)
            {
                WordTags.RemoveAll(x => x.Contains(_TagResult));
            }
            string _OtherResult = String.Join(" ", WordTags.ToArray());

            if (FoundCity || FoundTag)
            {
                if (WordTags.Count == 0)
                {
                    if (FoundCity && FoundTag)
                    {
                        TagResult = _TagResult;
                        CityResult = _CityResult;
                        OtherResult = null;
                        return TypesInList.CityAndTag;
                    }
                    else if (FoundTag)
                    {
                        TagResult = _TagResult;
                        CityResult = null;
                        OtherResult = null;
                        return TypesInList.Tag;
                    }
                    else
                    {
                        TagResult = null;
                        CityResult = _CityResult;
                        OtherResult = null;
                        return TypesInList.City;
                    }
                }
                else
                {
                    if (FoundCity && FoundTag)
                    {
                        TagResult = _TagResult;
                        CityResult = _CityResult;
                        OtherResult = _OtherResult;
                        return TypesInList.CityAndTagAndOther;
                    }
                    else if (FoundTag)
                    {
                        TagResult = _TagResult;
                        CityResult = null;
                        OtherResult = _OtherResult;
                        return TypesInList.TagAndOther;
                    }
                    else
                    {
                        TagResult = null;
                        CityResult = _CityResult;
                        OtherResult = _OtherResult;
                        return TypesInList.CityAndOther;
                    }
                }
                
            }
            else
            {
                TagResult = null;
                CityResult = null;
                OtherResult = _OtherResult; 
                return TypesInList.Other;
            }

        }

        public enum TypesInList
        {
            Other,
            Tag,
            TagAndOther,
            City,
            CityAndTag,
            CityAndOther,
            CityAndTagAndOther
        }
    }
}
