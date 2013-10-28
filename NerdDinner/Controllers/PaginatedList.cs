using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerdDinner.Controllers
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int Totalpages { get; private set; }


        public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize )
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            Totalpages = (int) Math.Ceiling(TotalCount/(double) PageSize);

            this.AddRange(source.Skip(PageIndex * PageSize).Take(pageSize));
        }

        public bool HasPreviousPage
        {
            get { return PageIndex > 0; }
        }

        public bool HasNextPage
        {
            get { return PageIndex + 1 < Totalpages; }
        }


    }
}