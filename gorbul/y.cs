using System;

namespace gorbul
{
    public class y
    {
        public static string
            //version ve dil
            version = "Ver. {0}",
            globalization_CultureInfo = "tr-TR",//global dil kodu olmalı!

            //klavye
            klavyeTumHarfler = "ABCDEFGHIJKLMNOPQRSTUVWXYZİĞÜŞÖÇ",
            klavyeSatir1 = "QWERTYUIOPĞÜ",
            klavyeSatir2 = "ASDFGHJKLŞİ",
            klavyeSatir3 = "ZXCVBNMÖÇ",

            //yukleniyor
            yukleniyor = "YÜKLENİYOR",

            //genel
            intBag = " Lütfen internet bağlantınızı kontrol edin ve tekrar deneyin.",
            hataOlustu = "Bir hata oluştu. || {0}",
            verilerSunucuyaKaydedilemiyor = "Veriler sunucuya kaydedilemiyor. Lütfen tekrar deneyin.",
            baziVerilerYuklenemedi = "Bazı veriler yüklenemedi." + intBag,
            jetonYok = "Yeterli jetonunuz yok. Jeton almak için mağaza açılsın mı?",
            gorselAlinamadi = "Görsel sunuculardan alınırken bir hata oluştu. Lütfen tekrar deneyin.",
            sunucuBakimda = "Sunucular bakıma alındı. Bir süre sonra tekrar giriş yapabileceksiniz.",
            guncellemeVar = "Oyunun yeni bir güncellemesi var. Lütfen mağaza üzerinden oyunu güncelleyiniz.",
            bolumlerYuklenemedi = "Bölümler yüklenemedi." + intBag,
            sorularYuklenemedi = "Sorular yüklenemedi." + intBag,
            basarimlarYuklenemedi = "Başarımlar yüklenemedi." + intBag,
            skorTablolariYuklenemedi = "Skor tabloları yüklenemedi." + intBag,
            cikmayaEminMisiniz = "Oyundan çıkmak istediğinize emin misiniz?",
            baglantiBasarisiz = "Sunucuya bağlanma denemesi başarısız oldu." + intBag,
            oncekiBolumlerGecilmemis = "Geçilmeyen bölüm(ler) tespit edildi. O bölüm(ler) bitirilmeden sonraki bölümlere devam edilemez.",

            //genel butonlar
            cik_btn = "ÇIK",
            cikma_btn = "ÇIKMA",
            magazaAc_btn = "MAĞAZAYI AÇ",
            kapat_btn = "KAPAT",
            iptal_btn = "İPTAL",
            evet_btn = "EVET",
            hayir_btn = "HAYIR",
            sifirla_btn = "SIFIRLA",
            vazgec_btn = "VAZGEÇ",
            sil_btn = "SİL",
            acma_btn = "AÇMA",
            bitti_btn = "BİTTİ",
            tamam_btn = "TAMAM",
            guncelle_btn = "GÜNCELLE",

            //bulmacaEkrani
            harfAc_btn = "Harf Aç (" + f.harfAlJetonUcreti + "j)",
            ipucu_btn = "İpucu (" + f.ipucuJetonUcreti + "j)",
            kelimeyiAc_btn = "Kelimeyi Aç (" + f.kelimeAcJetonUcreti + "j)",

            geriSayimKapali = "Otomatik geri sayım kapalı",
            gorselSayisi = "[Görsel {0}]",
            sonGorsel = "[Son Görsel]",
            snSonraIpucu = "{0} sn sonra ipucu verilecek",
            ipucuHakkiBitti = "Görsel ipucu hakkı bitti",
            snKala = "{0} sn kala",
            gorselGecmedenAcildi = "{0}Görsel geçmeden {1} açıldı{0}",
            sonGorselGoruldu = "{0}Son görsel görüldü{0}",
            adetGorseldeAcildi = "{0}{1}. görselde {2} açıldı{0}",
            gorselAltiYaziAyraci = " | ",
            ipucuYok = "{0}İpucu yok{0}" + gorselAltiYaziAyraci,
            sonIpucu = "{0}Son ipucu{0}" + gorselAltiYaziAyraci,
            adetIpucu = "{0}{1} İpucu{0}" + gorselAltiYaziAyraci,
            kelimeAcildi = "{0}\"Kelime Aç\"ıldı{0}" + gorselAltiYaziAyraci,

            bolumTamamlandi = "Bölüm Tamamlandı!",
            skor = "Skor\n{0} Puan!",

            //bolumler
            oncekiBolumler_btn = "▲",
            sonrakiBolumler_btn = "▼",

            yeniBolumlerYakinda = "Yeni bölümler yakında...",

            //anaEkran
            basla_btn = "BAŞLA",
            baslatiliyor_btn = "Başlatılıyor...",

            ayarlarAlinamadi = "Ayarlar alınamadı." + intBag,
            hesapYasakli = "Bu hesap yasaklandığı için giriş yapılamaz.",
            girisYapilamadi = "Giriş yapılamadı." + intBag,
            googleaGirisYapilamadi = "Google hesabınıza giriş yapılamadı. Başlayabilmek için lütfen Google hesabınıza giriş yapınız. :: {0}",
            googleaGirisHatasi = "Google hesabınıza giriş yapılırken bir hata oluştu. :: {0}",
            guncellemeIptal = "Güncelleme iptal edildi.",
            guncellenemedi = "Uygulama güncellenemedi.",

            //_update
            simdiGuncelle_btn = "ŞİMDİ GÜNCELLE",
            sonra_btn = "SONRA",
            tekrarDene_btn = "TEKRAR DENE",

            guncellemeTmm = "Güncelleme başarıyla tamamlandı!",
            yeniSurumIcinOturumAc = "Yeni sürümü kullanabilmeniz için uygulamayı kapatıp tekrar açmalısınız.",
            yeniSurumVar = "Uygulamanın yeni bir sürümü var. Başlayabilmek için güncelleme yapmanız gerekiyor.",
            guncellemeHatasi = "Güncelleme yapılırken bir hata oluştu. :: {0}",

            //menuPenceresiniAc
            menuMagaza_btn = "Mağaza",
            menuBasarimlar_btn = "Başarımlar",
            menuAyarlar_btn = "Ayarlar",
            menuMuzik_btn = "Müzik",
            menuSesler_btn = "Sesler",
            menuYenilikler_btn = "Yenilikler",
            menuCikis_btn = "Çıkış",
            menuBolumlereDon_btn = "Bölümlere Dön",

            //ayarlarPenceresiniAc
            ayarlarSesler_btn = "Sesler",
            ayarlarMuzik_btn = "Müzik",
            ayarlarTitresim_btn = "Titreşim",
            ayarlarYanlisKelimeSil_btn = "Yanlış kelimeyi sil",
            ayarlarGoogleOturumunuKapat_btn = "Google Oturumunu Kapat",
            ayarlarKullan_btn = "KULLAN",
            ayarlarIlerleyisiSifirla_btn = "İLERLEYİŞİ SIFIRLA",
            ayarlarYardim_btn = "YARDIM",
            ayarlarGizlilik_btn = "Gizlilik Sözleşmesi",
            ayarlarHizmet_btn = "Hizmet Kullanım Şartları",
            ayarlarVerileriYonet_btn = "Verileri Yönet",

            ayarlarGoogledanCikilsinMi = "Google hesabınızdan çıkış yapmak istediğinize emin misiniz",
            ayarlarKodGirBasarili = "Kodu başarı ile kullandınız. Jetonlarınız güncellendi.",
            ayarlarKodGirKullanilmis = "Bu kodu zaten daha önce kullandınız.",
            ayarlarKodGirArtikGecersiz = "Bu kod artık geçerli değil.",
            ayarlarKodGirGecersiz = "Kod geçerli değil.",
            ayarlarKodGirHata = "Kod yüklenirken bir hata oluştu, lütfen tekrar deneyin.",
            ayarlarKodGiriniz = "Lütfen bir kod giriniz.",
            ayarlarKodGirTxt = "Kod gir:",
            ayarlarIlerleyisSifirlanacak = "Tüm ilerleyişiniz sıfırlanacak, bu işlemin geri dönüşü yoktur. Emin misiniz?",
            ayarlarIlerleyisSifirlamaHatasi = "İlerleyişiniz sıfırlanamadı." + intBag,
            ayarlarIlerleyisSifirlamaBasarili = "İlerleyişinizi başarı ile sıfırladınız.",
            ayarlarYukleniyor = "Yükleniyor...",

            //profilPenceresiniAc
            profilBasarimlar = "BAŞARIMLAR",
            profilBasarimAlindi = "TAMAMLANDI\nÖDÜL ALINDI",
            profilBasarimTamamlandi = "TAMAMLANDI\nDOKUN ve ÖDÜLÜNÜ AL",
            profilBasarimAlinamadi = "Başarım ödülü alınamadı." + intBag,

            //ustMenuOlustur
            ustBaslikBolumYazisi = "Bölüm {0}: {1}",
            ustBaslikBulmacaYazisi = "{0} {1}",

            //magazaPenceresiniAc
            magazaBasligi = "MAĞAZA",
            jeton2 = "2 JETON",
            jeton20 = "20 JETON",
            jeton50 = "50 JETON",
            jeton120 = "120 JETON",
            jeton320 = "320 JETON",
            jeton750 = "750 JETON",
            jeton20fiyat = "₺9,99",
            jeton50fiyat = "₺19,99",
            jeton120fiyat = "₺39,99",
            jeton320fiyat = "₺99,99",
            jeton750fiyat = "₺199,99",

            //magazaReklamGoster
            ucretsizJetonAlindi = "Ücretsiz jetonunuz başarı ile alındı.",
            ucretsizJetonHata = "Jeton yüklenirken bir hata oluştu." + intBag,
            ucretsizJetonReklamKapatildi = "Reklam kapatıldığı için jeton yüklenmedi.",
            ucretsizJetonReklamGoruntulenemedi = "Reklam görüntülenemediği için jeton yüklenmedi.",
            ucretsizJetonReklamYuklenemedi = "Reklam yüklenemedi. Lütfen daha sonra tekrar deneyin.",
            ucretsizJetonHakkiBitti = "Ücretsiz jeton hakkınız bitti. {0} sonra ücretsiz jeton alabilirsiniz.",
            ucretsizJetonReklamKontroluYapilamadi = "Reklam kontrolü yapılamadı." + intBag,

            //magazaJetonVer
            jetonSatinAlindi = "Jetonlarınız başarı ile hesabınıza yüklendi. Satın alımınız için teşekkür ederiz.",
            jetonYuklenirkenHata = "Jetonlar hesabınıza yüklenirken bir hata oluştu." + intBag,

            //magazaJetonKontrolu
            jetonSatinAlmaGecersiz = "Hata: Satın Alma Geçersiz.",
            jetonSatinAlmaBeklemede = "Satın alma beklemede. Kartınızdan onay bekleniyor. Yanıta göre işlem yapılacak.",
            jetonSatinAlmaAnlasilamadi = "Uyarı: Satın alma durumu anlaşılamadı.",
            jetonSatinAlmaBilinemiyor = "Hata: Satın alma durumu bilinemiyor.",

            //magazaAyarlariYukle
            jetonSatinAlmaBaglantiKesildi = "İşlem yapılırken bağlantı kesildi.",
            jetonSatinAlmaAyarlanamadi = "Mağaza satın alma ayarları yapılamadı. :: {0}",
            jetonSatinAlmaIptalEdildi = "Satın alma iptal edildi.",
            jetonSatinAlmaBekliyor = "Jeton yüklemeniz kartınızdan cevap bekliyor olabilir. Ödeme yapıldığına eminseniz ve uzun süre geçmesine rağmen bu uyarıyı alıyorsanız, lütfen bunu bize bildirin.",
            jetonSatinAlmaOnayBekliyor = "Kartınızdan onay bekleniyor. Jetonlarınızın yüklenebilmesi için işlemin onaylanması gerekiyor.",
            jetonSatinAlmaHata = "Hata: Ödeme durumu öğrenilemiyor",
            jetonSatinAlmaUrunHatasi = "Ürün bilgileri alınırken hata oluştu. :: {0}",

            //sandikReklamGoster
            sandikAc_btn = "SANDIK AÇ",
            izleVeSandikAc_btn = "İZLE VE SANDIK AÇ",

            videoIzleVeSandikAc = "VIDEO İZLE VE 1 SANDIK DAHA AÇ!",
            gunlukUcretsizSandik = "GÜNLÜK ÜCRETSİZ SANDIK",
            sandikJetonKazandiniz = "{0} JETON KAZANDINIZ!",
            reklamKapatildi = "Reklam kapatıldığı için jeton yüklenmedi.",
            reklamYuklenemedi = "Reklam yüklenemedi. Lütfen daha sonra tekrar deneyin.",
            gunlukJetonHata = "Günlük jeton kontrolü yapılamadı." + intBag,

            //verileriYonetPenceresiniAc
            kisiselVerileriSil_btn = "Kişisel Verileri Sil",

            herSeySilinecek = "Her şey silinecek, bu işlemin geri dönüşü yoktur. Emin misiniz?",
            herSeySilinecekHata = "Kişisel bilgileriniz silinirken bir hata oluştu." + intBag,
            herSeySilinecekBasarili = "Kişisel bilgileriniz başarı ile silindi.",
            verileriYonetYazisi = @"
MuMMy'nin herhangi bir mobil uygulamasını kullandığınızda, kullanıcı davranışına göre değişen reklamlara maruz kalabilirsiniz. Platformlarımızda ilgi alanlarına dayalı olarak gösterilen reklamları devre dışı bırakmak için aşağıdaki adımları uygulayabilirsiniz:

1. Reklam kimliğinizi ('Reklam Tanımlayıcı') sıfırlamak

Reklam kimliğinizi sıfırlayınca, cihazınızda mobil uygulamalara erişim ve bunların kullanımından doğan ilgi alanına dayalı reklamlara ait veriler, reklam verenler tarafından izlenemez hale gelecektir; bu veriler yalnızca MuMMy'nin mobil uygulamalarına erişim ve bunların kullanımından doğan verilerle sınırlı değildir. Reklam kimliğinizi sıfırlamak için aşağıdaki adımları uygulayınız:

Android 2.2 ve daha yeni sürümler ile Google Play Hizmetleri 4.0 ve daha yeni sürümler için: Ayarlar > Google Ayarları > Reklamlar > 'İlgi alanına dayalı reklamları devre dışı bırak' seçeneğini işaretleyin veya Ayarlar > Google > Kişisel Bilgiler ve Gizlilik > Reklam Ayarları > 'Reklam Kişiselleştirmeyi devre dışı bırak' seçeneğini işaretleyin. Reklam Kimliğini Sıfırla seçeneğini tıklamayı unutmayın.

MuMMy'nin sizin reklam kimliğinizin özelliklerinden sorumlu olmadığının altını çizmek isteriz. Bununla ilgili sorularınız varsa, cihazınızdaki mobil uygulamaların işetim sistemi sağlayıcısı ile iletişime geçmenizi tavsiye ederiz.

2.Verilerinizi Silmek:

MuMMy'nin mobil uygulamalarına ilişkin tüm verilerinizin silinmesini istiyorsanız, aşağıdaki butona tıklayarak bu talebinizi iletebilirsiniz. İlgili uygulamaya erişiminizin, uygulamadaki ilerlemeniz ve hesabınızla birlikle durdurulacağını hatırlatmak isteriz. MuMMy, tüm ortaklarından sizin verilerinizi silmelerini de isteyecektir - söz konusu ortakların verilerinizi başka amaçlarla tutmaları gerekeceğini hatırlatmak isteriz. Her surette, ortaklarımızın Gizlilik Politikasını incelemenizi ve onlara özel bir talep göndermenizi tavsiye ederiz.
",
            verileriYonetSiliniyor = @"
DİKKAT! BÜTÜN BİLGİLERİNİZİ UYGULAMADAN SİLMEK ÜZERESİNİZ!

Şu anda MuMMy'nin veri kullanımı politikasına izin vermiş durumdasınız.

İzninizi geri çekmeniz halinde, bu uygulamayı kullanmanız için gereken kullanıcı lisansınızın otomatik olarak sona ereceğini ve oyuncu hesabınızın silineceğini hatırlatırız.

Kişisel verilerinizi silmek istediğinizden emin misiniz?
";

        //liste1: https://apps.timwhitlock.info/emoji/tables/unicode
        //diğer listeler: boşluk altındaki satırlarda olanlar; desteklenmeme olasılığı yüksek olanlar, yeniler
        //tüm emojiler: https://www.unicode.org/emoji/charts/full-emoji-list.html
        public static int[] iyiEmojiler =
        {
            //😍     👌       😏       😉       👍       😎       💯       👏
            0x1F60D, 0x1F44C, 0x1F60F, 0x1F609, 0x1F44D, 0x1F60E, 0x1F4AF, 0x1F44F,
            //😜     🎉       🔥       👑       🏆       ✅       ✨
            0x1F61C, 0x1F389, 0x1F525, 0x1F451, 0x1F3C6, 0x2705, 0x2728,

            //🧿     🤩       🎆       🏅       🌟       🎊       🎖       ✔
            0x1F9FF, 0x1F929, 0x1F386, 0x1F3C5, 0x1F31F, 0x1F38A, 0x1F396, 0x2714
        };
        public static int[] ortaEmojiler =
        {
            //😕     👀       🔍       🔎       😐       😶
            0x1F615, 0x1F440, 0x1F50D, 0x1F50E, 0x1F610, 0x1F636,
                        
            //🤔     🙄       🤨       🧐
            0x1F914, 0x1F644, 0x1F928, 0x1F9D0
        };
        public static int[] kotuEmojiler =
        {
            //😭         😒       😑       😤       😵       🍼       😨
            0x1F62D , 0x1F612 , 0x1F611, 0x1F624 , 0x1F635, 0x1F37C, 0x1F628,
                        
            //🤦     🥴       🥺       🔫
            0x1F926, 0x1F974, 0x1F97A, 0x1F52B
        };
        public static int[] kelimeAcEmojiler =
        {
            //👊     💀       👽       💣       💥       ⚡       🃏       🔓
            0x1F44A, 0x1F480, 0x1F47D, 0x1F4A3, 0x1F4A5, 0x26A1, 0x1F0CF, 0x1F513,
            //🔮
            0x1F52E,
                        
            //🤑     🤖       🧙      👻
            0x1F911, 0x1F916, 0x1F9D9, 0x1F47B
        };

        public static string p(string y, params object[] p)
        {
            try
            {
                string[] s = new string[p.Length];
                for (int i = 0; i < p.Length; i++)
                {
                    s[i] = p[i].ToString();
                }
                return string.Format(y, s);
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return "";
            }
        }
    }
}