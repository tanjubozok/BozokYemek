using System.Collections.Generic;

namespace BozokYemek.Models.Index
{
    public class MantiYorumModel
    {
        public List<Deytabeyz.Yorum> Yorum { get; set; }
        public Deytabeyz.Manti Manti { get; set; }
    }
}