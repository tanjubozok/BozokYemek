using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BozokYemek.Controllers
{
    public class KullaniciController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var kullaniciListesi = databaseContext.Kullanici.Where(x => x.Silme == false).ToList();
            return View(kullaniciListesi);
        }

        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KayitOl(Models.Kullanici.KayitOlModel model)
        {
            try
            {
                if (model.SifreTekrar != model.Kullanici.Sifre)
                {
                    throw new Exception("Sifre ile Sifre tekrar uyusmuyor");
                }
                else if (databaseContext.Kullanici.Where(x => x.Silme == false).Any(x => x.Eposta == model.Kullanici.Eposta))
                {
                    throw new Exception("Kayitli bir eposta adresi giriyorsunuz");
                }

                model.Kullanici.KullaniciTipi = Deytabeyz.KullaniciTipi.Musteri;
                model.Kullanici.EklemeTarihi = DateTime.Now;
                model.Kullanici.Silme = false;
                model.Kullanici.Resim = "~/Resim/Profil/DarthVader.png";
                databaseContext.Kullanici.Add(model.Kullanici);
                databaseContext.SaveChanges();
                return RedirectToAction("Login", "Kullanici");
            }
            catch (Exception ex)
            {
                ViewBag.KayitOlHata = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(Models.Kullanici.GirisModel model)
        {
            try
            {
                var kullanici = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Eposta == model.Kullanici.Eposta && x.Sifre == model.Kullanici.Sifre);
                if (kullanici != null)
                {
                    Session["GirisYapanKullanici"] = kullanici;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.GirisHata = "eposta veya sifre yanlis";
                }
            }
            catch (Exception ex)
            {
                ViewBag.GirisHata = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Cikis()
        {
            if (Session["GirisYapanKullanici"] != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SifremiUnuttum(string eposta)
        {
            var kullanici = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Eposta == eposta);
            if (kullanici == null)
            {
                ViewBag.SifremiUnuttumHata = eposta + " adresi kayitli degildir.";
                return View();
            }
            else
            {
                var body = "Sifreniz : " + kullanici.Sifre;
                MyMail mail = new MyMail(kullanici.Eposta, "sifremi unuttum", body);
                mail.SendMail();

                TempData["Info"] = eposta + "\n" + " email adresine sifreniz gonderilmistir";
                return RedirectToAction("Giris", "Kullanici");
            }
        }

        [HttpGet]
        public ActionResult Profil(int id = 0)
        {
            if (id == 0)
            {
                id = KullaniciId();
            }

            var kullaniciKontrol = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            if (kullaniciKontrol == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Models.Kullanici.ProfilModel model = new Models.Kullanici.ProfilModel()
            {
                Kullanici = kullaniciKontrol
            };

            return View(model);
        }

        [HttpGet]
        [Filtre.OturumKontrol]
        public ActionResult ProfilDuzenle(int id)
        {
            if (id == 0)
                id = KullaniciId();

            var kullanici = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);

            if (kullanici == null)
                return RedirectToAction("Index", "Home");

            Models.Kullanici.ProfilModel model = new Models.Kullanici.ProfilModel()
            {
                Kullanici = kullanici
            };
            return View(model);
        }

        [HttpPost]
        [Filtre.OturumKontrol]
        public ActionResult ProfilDuzenle(Models.Kullanici.ProfilModel model)
        {
            try
            {
                int id = KullaniciId();
                var profilDuzenleme = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
                profilDuzenleme.DegistirmeTarihi = DateTime.Now;
                profilDuzenleme.Adi = model.Kullanici.Adi;
                profilDuzenleme.Soyadi = model.Kullanici.Soyadi;
                profilDuzenleme.Sifre = model.Kullanici.Sifre;
                profilDuzenleme.Biyografi = model.Kullanici.Biyografi;
                profilDuzenleme.Silme = false;

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file.ContentLength > 0)
                    {
                        var folder = Server.MapPath("~/Resim/Profil");
                        var fileName = Guid.NewGuid() + ".jpg";
                        file.SaveAs(System.IO.Path.Combine(folder, fileName));

                        var filePath = "Resim/Profil/" + fileName;
                        profilDuzenleme.Resim = filePath;
                    }
                }
                databaseContext.SaveChanges();
                return RedirectToAction("Profil", "Kullanici");
            }
            catch (Exception ex)
            {
                int id = KullaniciId();
                ViewBag.ProfilDuzenleHata = ex.Message;

                Models.Kullanici.ProfilModel modelHata = new Models.Kullanici.ProfilModel()
                {
                    Kullanici = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id)
                };
                return View(model);
            }
        }


        public ActionResult ProfilSil(int id)
        {
            var profilSil = databaseContext.Kullanici.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            profilSil.Silme = true;
            databaseContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}