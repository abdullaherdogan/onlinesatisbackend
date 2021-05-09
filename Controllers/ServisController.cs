using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoreselMarket.Models;
using YoreselMarket.ViewModels;

namespace YoreselMarket.Controllers
{
    public class ServisController : ApiController
    {
        MarketDbEntities db = new MarketDbEntities();
        SonucModel sonuc = new SonucModel();

        #region Uye

        [HttpGet]
        [Route("api/uyeliste")]

        public List<UyeModel> UyeListele()
        {
            List<UyeModel> liste = db.Uye.Select(s => new UyeModel()
            {
                UyeId= s.UyeId,
                UyeYetkiId = s.UyeYetkiId,
                UyeAdSoyad=s.UyeAdSoyad,
                UyeKullaniciAdi = s.UyeKullaniciAdi,
                UyeAdres = s.UyeAdres,
                UyeMail = s.UyeMail,
                UyeParola = s.UyeParola
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uyeId}")]

        public UyeModel UyeById(string uyeId)
        {
            UyeModel uye = db.Uye.Where(x => x.UyeId == uyeId).Select(s => new UyeModel()
            {
                UyeId = s.UyeId,
                UyeYetkiId = s.UyeYetkiId,
                UyeAdSoyad = s.UyeAdSoyad,
                UyeKullaniciAdi=  s.UyeKullaniciAdi,
                UyeMail= s.UyeMail,
                UyeParola = s.UyeParola,
                UyeAdres= s.UyeAdres
            }).SingleOrDefault();

            return uye;
        }
        [HttpGet]
        [Route("api/uyegiris")]
        public DataModel UyeGirisi(string kullanici, string parola)
        {
            DataModel data = new DataModel();
            if ((db.Uye.Count(x=>x.UyeMail==kullanici)>0||db.Uye.Count(x=>x.UyeKullaniciAdi==kullanici)>0)&&db.Uye.Count(x=>x.UyeParola==parola)>0)
            {
                UyeModel uye = db.Uye.Where(x => x.UyeKullaniciAdi == kullanici || x.UyeMail == kullanici).Select(s => new UyeModel()
                {
                    UyeId = s.UyeId,
                    UyeYetkiId = s.UyeYetkiId,
                    UyeAdSoyad = s.UyeAdSoyad,
                    UyeKullaniciAdi = s.UyeKullaniciAdi,
                    UyeMail = s.UyeMail,
                    UyeParola = s.UyeParola,
                    UyeAdres = s.UyeAdres
                }).SingleOrDefault();
                data.Islem = true;
                data.Mesaj = "Üye girişi başarılı";
                data.Data = uye;
                return data;
            }else if((db.Uye.Count(x=>x.UyeKullaniciAdi==kullanici)>0||db.Uye.Count(x=> x.UyeMail == kullanici) > 0) && db.Uye.Count(x => x.UyeParola != parola) > 0)
            {
                data.Islem = false;
                data.Mesaj = "Şifreniz yanlış";
                return data;
            }
            else
            {
                data.Islem = false;
                data.Mesaj = "Böyle bir kullanıcı bulunamdı lütfen önce üye kaydı oluşturun";
                return data;
            }
        }

        [HttpPost]
        [Route("api/uyeekle")]

        public SonucModel UyeEkle(UyeModel model)
        {
            if (db.Uye.Count(x=>x.UyeMail==model.UyeMail)>0||db.Uye.Count(x=>x.UyeKullaniciAdi==model.UyeKullaniciAdi)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Bu e-mail ya da kullanıcı adı zaten kullanılıyor";
                return sonuc;
            }
            Uye yeni = new Uye();
            yeni.UyeId = Guid.NewGuid().ToString();
            yeni.UyeYetkiId = 1;
            yeni.UyeAdSoyad = model.UyeAdSoyad;
            yeni.UyeKullaniciAdi = model.UyeKullaniciAdi;
            yeni.UyeMail = model.UyeMail;
            yeni.UyeParola = model.UyeParola;
            yeni.UyeAdres = "";
            db.Uye.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üyelik kaydınız başarıyla yapıldı";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyeModel model)
        {
            Uye uye = db.Uye.Where(x => x.UyeId == model.UyeId).SingleOrDefault();
            if (uye==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Üye bulunamadı";
                return sonuc;
            }

            uye.UyeAdSoyad = model.UyeAdSoyad;
            uye.UyeKullaniciAdi = model.UyeKullaniciAdi;
            uye.UyeYetkiId = model.UyeYetkiId;
            uye.UyeMail = model.UyeMail;
            uye.UyeParola = model.UyeParola;
            uye.UyeAdres = model.UyeAdres;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üyelik Bilgileriniz Güncellendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/yetkidegistir")]
        public SonucModel YetkiDegistir(string uyeId)
        {
            Uye uye = db.Uye.Where(x => x.UyeId == uyeId).SingleOrDefault();
            if (uye.UyeYetkiId==1)
            {
                uye.UyeYetkiId = 2;
                db.SaveChanges();
                sonuc.Islem = true;
                sonuc.Mesaj = "Sitemiz Üzerinde satış yapmaya başlayabilirsiniz";
                return sonuc;
            }
            else if (uye.UyeYetkiId==2)
            {
                uye.UyeYetkiId = 1;
                db.SaveChanges();
                sonuc.Islem = true;
                sonuc.Mesaj = "Artık bir satıcı değilsiniz";
                return sonuc;
            }

            else
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Üye Kaydınız bulunamdı";
                return sonuc;
            }         
        }

        [HttpDelete]
        [Route("api/uyesil/{uyeId}")]
        public SonucModel UyeSil(string uyeId)
        {
            Uye uye = db.Uye.Where(x => x.UyeId == uyeId).SingleOrDefault();
            if (db.Urun.Count(x=>x.UrunUyeId==uyeId)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Üye kaydınızı silmek için satışa sunduğunuz ürün bulunmadığından emin olun";
                return sonuc;
            }
            db.Uye.Remove(uye);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Üyelik silme işleminiz gerçekleştirildi";
            return sonuc;
        }

        #endregion

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]

        public List<KategoriModel> KategoriListele()
        {
            List<KategoriModel> liste = db.Kategori.Select(s => new KategoriModel()
            {
                KategoriAdi = s.KategoriAdi,
                KategoriId = s.KategoriId
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{kategoriId}")]
        
        public KategoriModel KategoriByıd(string kategoriId)
        {
            KategoriModel kayit = db.Kategori.Where(x => x.KategoriId == kategoriId).Select(s => new KategoriModel()
            {
                KategoriId= s.KategoriId,
                KategoriAdi=s.KategoriAdi 
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kategoriekle")]

        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s=>s.KategoriAdi==model.KategoriAdi)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Bu isimde bir kategori zaten var";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.KategoriId = Guid.NewGuid().ToString();
            yeni.KategoriAdi = model.KategoriAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]

        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(x => x.KategoriId == model.KategoriId).SingleOrDefault();
            if (kayit==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bulunamdı";
                return sonuc;
            }
            kayit.KategoriAdi = model.KategoriAdi;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{kategoriId}")]

        public SonucModel KategoriSil(string kategoriId)
        {
            Kategori kayit = db.Kategori.Where(x => x.KategoriId == kategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori Bulunamdı";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion

        #region Satış cinsi
        [HttpGet]
        [Route("api/satiscinsiliste")]
        public List<SatisCinsiModel> SatışCinsiListele()
        {
            List<SatisCinsiModel> liste = db.SatisCİnsi.Select(s => new SatisCinsiModel()
            {
                SatisCinsi = s.SatisCinsi,
                SatisCinsiId= s.SatisCinsiId
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/satiscinsibyid/{satisCinsiId}")]
        public SatisCinsiModel SatisCinsiById(int satisCinsiId)
        {
            SatisCinsiModel cins = db.SatisCİnsi.Where(x=>x.SatisCinsiId==satisCinsiId).Select(c=>new SatisCinsiModel()
            {
                SatisCinsiId = c.SatisCinsiId,
                SatisCinsi= c.SatisCinsi
            }).SingleOrDefault();
            return cins;
        }
        #endregion

        #region Urun

        [HttpGet]
        [Route("api/urunliste")]
        public List<UrunModel> UrunListele()
        {
            List<UrunModel> liste = db.Urun.Select(s => new UrunModel()
            {
                UrunId = s.UrunId,
                UrunUyeId =s.UrunUyeId,
                UrunKategoriId = s.UrunKategoriId,
                UrunSatisCinsiId  =s.UrunSatisCinsiId,
                UrunAdi = s.UrunAdi,
                UrunAciklama = s.UrunAciklama,
                UrunFiyati = s.UrunFiyati,
                UrunStok = s.UrunStok
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/urunbykategori/{kategoriId}")]
        public List<UrunModel> UrunByKategori(string kategoriId)
        {
            List<UrunModel> liste = db.Urun.Where(x=>x.UrunKategoriId==kategoriId).Select(s => new UrunModel()
            {
                UrunId =s.UrunId,
                UrunUyeId = s.UrunUyeId,
                UrunKategoriId = s.UrunKategoriId,
                UrunSatisCinsiId = s.UrunSatisCinsiId,
                UrunAdi = s.UrunAdi,
                UrunAciklama = s.UrunAciklama,
                UrunFiyati = s.UrunFiyati,
                UrunStok = s.UrunStok
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/urunbyid/{urunid}")]
        public UrunModel UrunByıd(string urunId)
        {
            UrunModel urun = db.Urun.Where(x => x.UrunId == urunId).Select(s => new UrunModel()
            {
                UrunId = s.UrunId,
                UrunUyeId = s.UrunUyeId,
                UrunKategoriId = s.UrunKategoriId,
                UrunSatisCinsiId = s.UrunSatisCinsiId,
                UrunAdi = s.UrunAdi,
                UrunAciklama = s.UrunAciklama,
                UrunFiyati = s.UrunFiyati,
                UrunStok = s.UrunStok
            }).SingleOrDefault();
            return urun;
        }
        [HttpGet]
        [Route("api/urunbyuye/{uyeId}")]
        public List<UrunModel> UrunByUye(string uyeId)
        {
            List<UrunModel> liste = db.Urun.Where(x => x.UrunUyeId == uyeId).Select(s => new UrunModel()
            {
                UrunId = s.UrunId,
                UrunUyeId = s.UrunUyeId,
                UrunKategoriId = s.UrunKategoriId,
                UrunSatisCinsiId = s.UrunSatisCinsiId,
                UrunAdi = s.UrunAdi,
                UrunAciklama = s.UrunAciklama,
                UrunFiyati = s.UrunFiyati,
                UrunStok = s.UrunStok
            }).ToList();
            return liste;
        }

        [HttpPost]
        [Route("api/urunekle")]
        public SonucModel UrunEkle(UrunModel model)
        {
            List<Urun> uyeninUrunleri = db.Urun.Where(x => x.UrunUyeId == model.UrunUyeId).ToList();
            if (uyeninUrunleri.Count(x=>x.UrunAdi == model.UrunAdi)>0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Bu ürün zaten ürünlerinizde bulunuyor";
                return sonuc;
            }

            Urun yeni = new Urun();
            yeni.UrunId = Guid.NewGuid().ToString();
            yeni.UrunUyeId = model.UrunUyeId;
            yeni.UrunKategoriId = model.UrunKategoriId;
            yeni.UrunSatisCinsiId = model.UrunSatisCinsiId;
            yeni.UrunAdi = model.UrunAdi;
            yeni.UrunAciklama = model.UrunAciklama;
            yeni.UrunFiyati = model.UrunFiyati;
            yeni.UrunStok = model.UrunStok;
            db.Urun.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün satış listenize eklendi";
            return sonuc;

        }
        [HttpPut]
        [Route("api/urunduzenle")]
        public SonucModel UrunDuzenle(UrunModel model)
        {
            Urun urun = db.Urun.Where(x => x.UrunId == model.UrunId).SingleOrDefault();
            if (urun==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Ürün Bulunamadı";
                return sonuc;
            }
            urun.UrunKategoriId = model.UrunKategoriId;
            urun.UrunSatisCinsiId = model.UrunSatisCinsiId;
            urun.UrunStok = model.UrunStok;
            urun.UrunAdi = model.UrunAdi;
            urun.UrunAciklama = model.UrunAciklama;
            urun.UrunFiyati = model.UrunFiyati;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün Güncellendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/urunsil/{urunId}")]
        public SonucModel UrunSil(string urunId)
        {
            Urun urun = db.Urun.Where(x => x.UrunId == urunId).SingleOrDefault();
            if (urun==null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Ürün Bulunamdı";
                return sonuc;
            }
            db.Urun.Remove(urun);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün Sİlindi";
            return sonuc;
        }
        #endregion

        #region Sepet

        [HttpGet]
        [Route("api/sepetliste")]
        public List<SepetModel> SepetListele()
        {
            List<SepetModel> liste = db.Sepet.Select(s => new SepetModel()
            {
                SepetId = s.SepetId,
                UrunId = s.UrunId,
                UrunMiktar =s.UrunMiktar,
                UyeId = s.UyeId
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/sepetbyuye/{uyeId}")]
        public List<SepetModel> SepetByUye(string uyeId)
        {
            List<SepetModel> liste = db.Sepet.Where(x => x.UyeId == uyeId).Select(s => new SepetModel()
            {
                SepetId  = s.SepetId,
                UyeId = s.UyeId,
                UrunId = s.UrunId,
                UrunMiktar= s.UrunMiktar
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/sepetbyid/{sepetId}")]
        public SepetModel SepetById(string sepetId)
        {
            SepetModel kayit = db.Sepet.Where(x => x.SepetId == sepetId).Select(s => new SepetModel()
            {
                SepetId = s.SepetId,
                UyeId = s.UyeId,
                UrunId = s.UrunId,
                UrunMiktar = s.UrunMiktar
            }).SingleOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/sepeteekle")]
        public SonucModel SepeteEkle(SepetModel model)
        {
            List<Sepet> uyeninSepeti = db.Sepet.Where(x => x.UyeId == model.UyeId).ToList();
            foreach (var item in uyeninSepeti)
            {
                if (item.UrunId==model.UrunId)
                {
                    item.UrunMiktar += 1;
                    db.SaveChanges();
                    sonuc.Islem = true;
                    sonuc.Mesaj = "Ürün Sepete Eklendi";
                    return sonuc;
                }
            }
            if (model.UrunMiktar==0)
            {
                model.UrunMiktar = 1;
            }
            Sepet yeni = new Sepet();
            yeni.SepetId = Guid.NewGuid().ToString();
            yeni.UrunId = model.UrunId;
            yeni.UyeId = model.UyeId;
            yeni.UrunMiktar = model.UrunMiktar;
            db.Sepet.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün Sepete Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/sepetiduzenle")]
        public SonucModel SepetiDuzenle(SepetModel model)
        {
            Sepet kayit = db.Sepet.Where(x => x.SepetId == model.SepetId).SingleOrDefault();
            kayit.UrunMiktar = model.UrunMiktar;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün miktarı değiştirildi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/sepetisil/{sepetId}")]
        public SonucModel SepetiSil(string sepetId)
        {
            Sepet kayit = db.Sepet.Where(x => x.SepetId == sepetId).SingleOrDefault();
            db.Sepet.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Ürün sepetten Çıkarıldı";
            return sonuc;
        }
        #endregion
    }
}
