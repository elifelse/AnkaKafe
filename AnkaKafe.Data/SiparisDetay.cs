using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkaKafe.Data
{
    public class SiparisDetay
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }
        public int Adet { get; set; }        
        public string TutarTL { get { return string.Format("₺{0:n2}", Tutar()); } } // n2 : küsüratı 2 haneli olan numeric variable
        public decimal Tutar()
        {
            return BirimFiyat * Adet;
        }
    }
}
