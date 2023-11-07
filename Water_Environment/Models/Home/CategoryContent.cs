using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Water_Environment.Models.Home
{
    public class CategoryContent
    {
        public string CateName { get; set; }
        public List<ActivitiesAndNew> CateContent { get; set; }
    }
}