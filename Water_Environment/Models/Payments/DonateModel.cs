using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Water_Environment.Models.Payments
{
    public class DonateModel
    {
        public string Content { get; set; }
        public int MoneyDonate { get; set; }
        public int TypePaymentVN { get; set; }
    }
}