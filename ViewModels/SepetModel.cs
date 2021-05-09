using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoreselMarket.ViewModels
{
    public class SepetModel
    {
        public string SepetId { get; set; }
        public string UrunId { get; set; }
        public string UyeId { get; set; }
        public decimal UrunMiktar { get; set; }
    }
}