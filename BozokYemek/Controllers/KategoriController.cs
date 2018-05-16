using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BozokYemek.Controllers
{
    [Filtre.OturumKontrol(kullaniciKontrolEt: 3)]
    public class KategoriController : BaseController
    {
        public ActionResult Index()
        {
            var kategoriListesi = databaseContext.Kategori.Where(x => x.Silme == false).ToList();
            return View(kategoriListesi);
        }

        public ActionResult Edit(int id = 0)
        {
            var kategoriListesi = databaseContext.Kategori.Where(x => x.Silme == false).FirstOrDefault(x => x.Id == id);
            return View(kategoriListesi);
        }

        [HttpPost]
        public ActionResult Edit(Deytabeyz.Kategori kategori)
        {
            if (kategori.Id > 0)
            {
                var kategoriListesi = databaseContext.Kategori.FirstOrDefault(x => x.Id == kategori.Id);
                kategoriListesi.Adi = kategori.Adi;
                kategoriListesi.Aciklama = kategori.Aciklama;
                kategoriListesi.DegistirmeTarihi = DateTime.Now;
                kategori.Silme = false;
            }
            else
            {
                kategori.EklemeTarihi = DateTime.Now;
                kategori.Silme = false;
                databaseContext.Entry(kategori).State = System.Data.Entity.EntityState.Added;
            }
            databaseContext.SaveChanges();
            return RedirectToAction("Index", "Kategori");
        }

        public ActionResult Delete(int id)
        {
            var kategoriSilme = databaseContext.Kategori.FirstOrDefault(x => x.Id == id);
            kategoriSilme.Silme = true;
            databaseContext.SaveChanges();
            return RedirectToAction("Index", "Kategori");
        }
    }
}