using System.Collections.Generic;

namespace BozokYemek.Models.Index
{
    public class MantiGetirModel
    {
        public Deytabeyz.Manti Manti { get; set; }
        public List<Deytabeyz.Yorum> Yorum { get; set; }
    }
}