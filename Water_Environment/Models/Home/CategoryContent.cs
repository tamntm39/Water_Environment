using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Water_Environment.Models.Home
{
    public class CategoryContent
    {
        public string CateName { get; set; }
        public List<ContentOfCate> CateContent { get; set; }

        public class ContentOfCate
        {
            public string Title { get; set; }
            public string url { get; set; }
        }
    }
}