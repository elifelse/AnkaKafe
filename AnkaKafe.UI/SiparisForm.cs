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
    public partial class SiparisForm : Form
    {
        private readonly KafeVeri _db;
        private readonly Siparis _siparis;

        // yeni bir sipariş formu oluştururken bu parametreler zorunlu
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            _db = kafeVeri;
            _siparis = siparis;
            InitializeComponent();
            MasaNoGuncelle();
            FiyatNoGuncelle();
            UrunleriGoster();
            DetaylariListele();
        }

        private void DetaylariListele()
        {
            dgvSiparisDetaylar.DataSource = null;
            dgvSiparisDetaylar.DataSource = _siparis.SiparisDetaylar;
        }

        private void UrunleriGoster()
        {
            cboUrun.DataSource = _db.Urunler;
        }

        private void FiyatNoGuncelle()
        {
            lblOdemeTutar.Text = _siparis.ToplamTutarTL;
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {_siparis.MasaNo} Sipariş Bilgileri";
            lblMasaNo.Text = _siparis.MasaNo.ToString("");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;
            SiparisDetay siparisDetay = new SiparisDetay()
            {
                UrunAd = urun.UrunAd,
                BirimFiyat = urun.BirimFiyat,
                Adet = (int)nudAdet.Value
            };
            _siparis.SiparisDetaylar.Add(siparisDetay);
            DetaylariListele();
            FiyatNoGuncelle();
        }
    }
}
