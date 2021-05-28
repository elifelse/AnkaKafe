using AnkaKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnkaKafe.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db = new KafeVeri();
        public AnaForm()
        {       
            OrnekUrunleriEkle();
            InitializeComponent();
            Icon = Resource.AnkaKafeIcon;
            masalarImageList.Images.Add("bos", Resource.bos);
            masalarImageList.Images.Add("dolu", Resource.dolu);
            MasalariOlustur();
        }

        private void OrnekUrunleriEkle()
        {
            db.Urunler.Add(new Urun() { UrunAd = "Çay", BirimFiyat = 4.00m });
            db.Urunler.Add(new Urun() { UrunAd = "Simit", BirimFiyat = 5.00m });
        }

        private void MasalariOlustur()
        {
            ListViewItem lvi;
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                lvi = new ListViewItem();
                lvi.Tag = i; // Masa noyu her bir ögenin tag property'sinde saklayalım
                lvi.Text = "Masa " + i;
                lvi.ImageKey = "bos";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsmiUrunler)
            {
                new UrunlerForm(db).ShowDialog();
            }
            else if (e.ClickedItem == tsmiGecmisSiparis)
            {
                new GecmisSiparislerForm(db).ShowDialog();
            }
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            int masaNo = (int)lvi.Tag; // unboxing
            lvi.ImageKey = "dolu";

            // eğer bu masada önceden sipariş yoksa oluştur
            Siparis siparis = SiparisBul(masaNo);

            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.AktifSiparisler.Add(siparis);
            }

            // bu siparişi başka bir formda aç
            SiparisForm siparisForm = new SiparisForm(db, siparis);

            // EVENT OLUŞTURMADA 5. ADIM : Event'e oluşturulan metotu atamak
            siparisForm.MasaTasindi += SiparisForm_MasaTasindi;

            siparisForm.ShowDialog();

            // Sipariş formu kapandıktan sonra sipariş durumunu kontrol et
            if (siparis.Durum != SiparisDurum.Aktif)
            {
                lvi.ImageKey = "bos";
            }
        }

        // EVENT OLUŞTURMADA 4. ADIM : Event'e atanacak metotu
        // event delegesinin dönüş tipi ve argüman çeşitlerine uygun olarak oluşturmak
        private void SiparisForm_MasaTasindi(object sender, MasaTasindiEventArgs e)
        {
            foreach (ListViewItem lvi in lvwMasalar.Items)
            {
                int masaNo = (int)lvi.Tag;
                if((int)lvi.Tag == e.EskiMasaNo)
                {
                    lvi.ImageKey = "bos";
                }
                else if (masaNo == e.YeniMasaNo)
                {
                    lvi.ImageKey = "dolu";
                }
            }
        }

        private Siparis SiparisBul(int masaNo)
        {
            // return db.AktifSiparisler.FirstOrDefault(s => s.MasaNo == masaNo); // (FirstOrDefault) şarta uyan ilk elemanı verir

            foreach (Siparis siparis in db.AktifSiparisler)
            {
                if (siparis.MasaNo == masaNo)
                {
                    return siparis;
                }
            }
            return null;
        }
    }
}
