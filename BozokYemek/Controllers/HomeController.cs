using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BozokYemek.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var mantiList = databaseContext.Manti.Where(x => x.Silme == false);

            Models.Index.IndexModel model = new Models.Index.IndexModel()
            {
                Manti = mantiList.ToList()
            };

            return View(model);
        }

        public ActionResult MantiGetir(int id = 0)
        {
            var mantiListesi = databaseContext.Manti.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            if (mantiListesi == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Models.Index.MantiGetirModel model = new Models.Index.MantiGetirModel()
            {
                Manti = mantiListesi,
                Yorum = mantiListesi.Yorum.ToList()
            };

            return View(model);
        }

        [HttpPost]//yorumdan gelenler
        public ActionResult MantiGetir(Deytabeyz.Yorum model)
        {
            try
            {
                model.KullaniciId = KullaniciId();
                model.EklemeTarihi = DateTime.Now;
                databaseContext.Yorum.Add(model);
                databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.MAntiYorumHata = ex.Message;
            }

            return RedirectToAction("MantiGetir", "Home");
        }

        public ActionResult MantiArama(string q)
        {
            var mantiList = databaseContext.Manti.Where(x => x.Silme == false);

            if (string.IsNullOrEmpty("q") == false)
                mantiList = mantiList.Where(x => x.Baslik.Contains(q) || x.Aciklama.Contains(q));

            return View("KategoriMantiGetir", mantiList);
        }

        public PartialViewResult Kategoriler()
        {
            var kategoriListesi = databaseContext.Kategori.Where(x => x.Silme == false).OrderByDescending(x => x.EklemeTarihi).ToList();
            return PartialView(kategoriListesi);
        }

        public ActionResult KategoriMantiGetir(int? id)
        {
            var mantiListesi = databaseContext.Manti.Where(x => x.Silme == false);

            if (id != null)
                mantiListesi = mantiListesi.Where(x => x.KategoriId == id);

            return View(mantiListesi);
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult NavbarKategori()
        {
            var kategoriListesi = databaseContext.Kategori.Where(x => x.Silme == false).ToList();

            return PartialView(kategoriListesi);
        }

        public ActionResult Slayt()
        {
            var slaytListesi = databaseContext.Manti.Where(x => x.Silme == false).OrderByDescending(x => x.EklemeTarihi).Take(6).ToList();
            return PartialView(slaytListesi);
        }


    }
}