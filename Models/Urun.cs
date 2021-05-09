//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YoreselMarket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Urun
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Urun()
        {
            this.Sepet = new HashSet<Sepet>();
        }
    
        public string UrunId { get; set; }
        public string UrunKategoriId { get; set; }
        public string UrunUyeId { get; set; }
        public int UrunSatisCinsiId { get; set; }
        public string UrunAdi { get; set; }
        public decimal UrunFiyati { get; set; }
        public string UrunAciklama { get; set; }
        public decimal UrunStok { get; set; }
    
        public virtual Kategori Kategori { get; set; }
        public virtual SatisCİnsi SatisCİnsi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sepet> Sepet { get; set; }
        public virtual Uye Uye { get; set; }
    }
}
