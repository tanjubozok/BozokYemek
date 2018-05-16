using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BozokYemek.Models.Index
{
    public class MantiGetirModel
    {
        public Deytabeyz.Manti Manti { get; set; }
        public List<Deytabeyz.Yorum> Yorum { get; set; }
    }
}