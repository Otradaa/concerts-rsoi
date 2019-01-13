using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Models
{
    public class ConcertsPage
    {
        public int totalCount { get; private set; }
        public int page { get; private set; }
        public int pageSize { get; private set; }
        public IEnumerable<ConcertRequest> concertsData { get; private set; }

        public ConcertsPage(int _count, int _page, int _size, IEnumerable<ConcertRequest> data)
        {
            totalCount = _count;
            page = _page;
            pageSize = _size;
            concertsData = data;
        }
    }
}
