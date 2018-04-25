using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cherry.Data.Administration;
using Cherry.Web.Globals;
using Microsoft.EntityFrameworkCore;

namespace Cherry.Web.Search
{
    public interface ISearcher
    {
        void Search();
        Range QueueRange { get; }
        int MaximumInQueue { get; }
    }

    public interface ISearcher<T> : ISearcher where T : class
    {
        List<T> Result { get; }
    }

}
