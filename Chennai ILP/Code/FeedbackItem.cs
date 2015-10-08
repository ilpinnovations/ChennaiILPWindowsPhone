using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chennai_ILP.Code
{
    public class FeedbackItem
    {
        public string Comments { get; set; }
        public int Rating { get; set; }

        public FeedbackItem(int rating, string comments)
        {
            this.Comments = comments;
            this.Rating = rating;
        }
    }
}
