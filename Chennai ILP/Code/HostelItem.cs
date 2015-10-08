using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chennai_ILP.Code
{
    public class HostelItem
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public double Rating { get; set; }
        public int ReleaseYear { get; set; }

        public string TableName { get; set; }
        public double Latitudes { get; set; }
        public double Longitudes { get; set; }
        public string Address { get; set; }

    }
}
