using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BozokYemek.Models.Index
{
    public class MantiYorumModel
    {
        public List<Deytabeyz.Yorum> Yorum { get; set; }
        public Deytabeyz.Manti Manti { get; set; }

    }
}