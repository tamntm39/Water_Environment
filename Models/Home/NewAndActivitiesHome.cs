using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Water_Environment.Models.Home
{
    public class NewAndActivitiesHome
    {
        public List<ActivitiesAndNew> NewsLatest { get; set; }
        public List<ActivitiesAndNew> NewsHot { get; set; }
    }
}