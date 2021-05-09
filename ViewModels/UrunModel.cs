using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoreselMarket.ViewModels
{
    public class UrunModel
    {

        public string UrunId { get; set; }
        public string UrunKategoriId { get; set; }
        public string UrunUyeId { get; set; }
        public int UrunSatisCinsiId { get; set; }
        public string UrunAdi { get; set; }
        public decimal UrunFiyati { get; set; }
        public string UrunAciklama { get; set; }
        public decimal UrunStok { get; set; }
    }
}