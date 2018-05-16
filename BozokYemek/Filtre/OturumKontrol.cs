using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BozokYemek.Filtre
{
    public class OturumKontrol : ActionFilterAttribute, IAuthorizationFilter
    {
        public int Kullanicim { get; private set; }

        public OturumKontrol()
        {

        }

        public OturumKontrol(int kullaniciKontrolEt)
        {
            Kullanicim = kullaniciKontrolEt;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var kullanici = (BozokYemek.Deytabeyz.Kullanici)HttpContext.Current.Session["GirisYapanKullanici"];
            if (kullanici == null)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
            else
            {
                var kullaniciTipi = (int)kullanici.KullaniciTipi;
                if (kullaniciTipi < Kullanicim)
                {
                    filterContext.Result = new RedirectResult("/Home/Index");
                }
            }

        }
    }
}