using System.Collections.Generic;
using Cherry.Web.Globals;

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
