using System.Collections.Generic;

namespace BozokYemek.Models.Index
{
    public class KategorileriGetirModel
    {
        public List<Deytabeyz.Manti> Manti { get; set; }
        public Deytabeyz.Kullanici Kullanici { get; set; }
    }
}