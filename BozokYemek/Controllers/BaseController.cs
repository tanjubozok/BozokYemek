using System.Web.Mvc;

namespace BozokYemek.Controllers
{
    public class BaseController : Controller
    {
        protected Deytabeyz.BozokMantiDBEntities databaseContext;

        public BaseController()
        {
            databaseContext = new Deytabeyz.BozokMantiDBEntities();
        }

        protected Deytabeyz.Kullanici Kullanici()
        {
            if (Session["GirisYapanKullanici"] == null)
                return null;

            return (Deytabeyz.Kullanici)Session["GirisYapanKullanici"];
        }

        protected int KullaniciId()
        {
            if (Session["GirisYapanKullanici"] == null)
                return 0;

            return ((Deytabeyz.Kullanici)Session["GirisYapanKullanici"]).Id;
        }

        protected bool GirisYapildimi()
        {
            if (Session["GirisYapanKullanici"] == null)
                return false;
            else
                return true;
        }
    }
}