using System.Collections;

namespace WebAPI.ViewModels
{
    public class SearchResultViewModel
    {
        public IList? DataList { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }

    }
}
