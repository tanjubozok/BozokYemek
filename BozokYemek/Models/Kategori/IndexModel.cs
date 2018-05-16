using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BozokYemek.Models.Kategori
{
    public class IndexModel
    {
        public List<Deytabeyz.Kategori> Kategori { get; set; }
        public int YemekSayisi { get; set; }
    }
}