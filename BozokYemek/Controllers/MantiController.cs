using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BozokYemek.Controllers
{
    [Filtre.OturumKontrol(kullaniciKontrolEt: 3)]
    public class MantiController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var mantiListesi = databaseContext.Manti.Where(x => x.Silme == false).ToList();
            return View(mantiListesi);
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            var mantiListesi = databaseContext.Manti.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            var kategoriListesi = databaseContext.Kategori.Where(x => x.Silme == false).Select(x => new SelectListItem()
            {
                Text = x.Adi,
                Value = x.Id.ToString()
            }).ToList();

            ViewBag.KategoriGonder = kategoriListesi;
            return View(mantiListesi);

        }

        [HttpPost]
        public ActionResult Edit(Deytabeyz.Manti manti)
        {
            if (ModelState.IsValid)
            {
                var mantiImageNamePath = string.Empty;
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file.ContentLength > 0)
                    {
                        var folder = Server.MapPath("~/Resim/Yemek");
                        var fileName = Guid.NewGuid() + ".jpg";
                        file.SaveAs(System.IO.Path.Combine(folder, fileName));

                        var filePath = "Resim/Yemek/" + fileName;
                        mantiImageNamePath = filePath;
                    }
                }

                if (manti.Id > 0)
                {
                    var mantiListesi = databaseContext.Manti.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == manti.Id);
                    mantiListesi.Baslik = manti.Baslik;
                    mantiListesi.Aciklama = manti.Aciklama;
                    mantiListesi.KacKisilik = manti.KacKisilik;
                    mantiListesi.HazirlamaSuresi = manti.HazirlamaSuresi;
                    mantiListesi.PisirmeSuresi = manti.PisirmeSuresi;
                    mantiListesi.Malzeme = manti.Malzeme;
                    mantiListesi.DegistirmeTarihi = DateTime.Now;
                    mantiListesi.KategoriId = manti.KategoriId;
                    mantiListesi.KullaniciId = 1;
                    mantiListesi.Silme = false;

                    if (string.IsNullOrEmpty(mantiImageNamePath) == false)
                    {
                        mantiListesi.Resim = mantiImageNamePath;
                    }
                }
                else
                {
                    manti.EklemeTarihi = DateTime.Now;
                    manti.Silme = false;
                    manti.KullaniciId = 1;
                    manti.Resim = mantiImageNamePath;
                    databaseContext.Entry(manti).State = System.Data.Entity.EntityState.Added;
                }
            }
            databaseContext.SaveChanges();
            return RedirectToAction("Index", "Manti");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var mantiSil = databaseContext.Manti.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            mantiSil.Silme = true;
            databaseContext.SaveChanges();
            return RedirectToAction("Index", "Manti");
        }
    }
}