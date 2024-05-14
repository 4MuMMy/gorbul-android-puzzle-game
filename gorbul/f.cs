using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.OS;
using Android.Gms.Ads;
using Android.BillingClient.Api;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Android.Support.Annotation;
using Android.Content.Res;
using Java.IO;
using System.Runtime.Remoting.Contexts;
using Xamarin.Forms.Xaml;

namespace gorbul
{
    public class ayarlarTBL
    {
        public int aktif { get; set; }
        public string duyuru { get; set; }
        public string version { get; set; }
    }
    public class bolumlerTBL
    {
        public int id { get; set; }
        public string bolumAdi { get; set; }
    }
    public class jetonlarTBL
    {
        public int id { get; set; }
        public int jetonSayisi { get; set; }
        public DateTime sonJetonTarihi { get; set; }
        public DateTime gunlukJeton { get; set; }
    }
    public class sorularTBL
    {
        public int id { get; set; }
        public string soru { get; set; }
        public string cevap { get; set; }
        public int gorselSayisi { get; set; }
    }
    public class soruIstatistikTBL
    {
        public int soruID { get; set; }
        public bool bolumGecildi { get; set; }
        public string hangiHarflerAcildi { get; set; }
        public int toplamSure { get; set; }
        public int yanlisDenemeSayisi { get; set; }
        public int gorselSahneSayisi { get; set; }
        public int suAnkiGorselSahneSuresi { get; set; }
        public int uzaklastirmaSayisi { get; set; }
        public int harfAlmaSayisi { get; set; }
        public bool kelimeAcildi { get; set; }
        public int toplamPuan { get; set; }
        public string acilanHarfler { get; set; }
    }
    public class soruIstatistikIDleriTBL
    {
        public int soruID { get; set; }
        public bool bolumGecildi { get; set; }
        public int toplamPuan { get; set; }
    }
    public class GoogleProfile
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public Android.Net.Uri PhotoUrl { get; set; }

    }
    public class veriGonder
    {
        public string gonderilecekVeriAdi { get; set; }
        public object gonderilecekVeri { get; set; }
    }
    public class oAyarlariTBL
    {
        public bool sesler { get; set; }
        public bool muzik { get; set; }
        public bool titresim { get; set; }
        public bool yanlisKelimeSil { get; set; }
    }
    public class basarimlarTBL
    {
        public int id { get; set; }
        public string baslik { get; set; }
        public string aciklama { get; set; }
        public int odul { get; set; }
        public int hedefDeger { get; set; }
        public string achi_id { get; set; }
    }
    public class basarimKontrolTBL
    {
        public int id { get; set; }
        public int basarimID { get; set; }
        public bool odulAlindi { get; set; }
        public string googleID { get; set; }
    }
    public class skorTablolari
    {
        public int id { get; set; }
        public int ilk { get; set; }
        public int son { get; set; }
        public string lead_id { get; set; }
    }

    static class f
    {
        public static ayarlarTBL ayarlarTBL_;
        public static oAyarlariTBL oAyarlariTBL_;
        public static GoogleProfile googleProfile_;
        public static soruIstatistikTBL soruIstatistikTBL_;
        public static jetonlarTBL jetonlarTBL_;
        public static bolumlerTBL[] bolumlerTBL_;
        public static sorularTBL[] sorularTBL_;
        public static soruIstatistikIDleriTBL[] soruIstatistikIDleriTBL_;
        public static basarimlarTBL[] basarimlarTBL_;
        public static basarimKontrolTBL[] basarimKontrolTBL_;
        public static skorTablolari[] skorTablolari_;

        public static Bitmap profilGorseli_;
        public static int toplamGecilenBolumSayisi = 0;

        public static bool sandikKontrolEdilmedi = true;

        public static bool bakimAktif = false;

        public static bool temelVerileriCek_ = false;

        public static bool acilisTamamlandi = false;

        public const int
            baglantiTimeOutSuresiSN = 8,
            gorselTimeOutSuresiSN = 32,

            harfAlJetonUcreti = 6,
            ipucuJetonUcreti = 10,
            kelimeAcJetonUcreti = 20;

        public static _PlayGames PlayGames__ = new _PlayGames();
        public static List<PopupWindow> acikPencereler = new List<PopupWindow>();
        public static AppCompatActivity c = null;

        public static double
            olceklendirmeOrani,
            ekranGenisligi = 1080,
            ekranYuksekligi = 1920;

        public static bool hasServerTime = false;
        public static DateTime
            server_utc_time,
            local_time;

        public const string
            server_main = "https://gorbul-server-main.com",// or server ip
            server_bolum_gorseli_link = server_main + "gorbul/b/",
            server_bulmaca_gorseli_link = server_main + "gorbul/i/",
            server_main_link = server_main + "gorbul/index.php",

            google_firebase_account_link = "firebase-adminsdk-x.gserviceaccount.com",
            admob_magaza_reklam_id = "ca-app-pub-x",
            admob_gecis_reklam_id = "ca-app-pub-x",
            admob_sandik_reklam_id = "ca-app-pub-x";

        public static string _version = "";

        //ÖNEMLİ !!! >>> logRecording = false yapıldığında tüm projede alttaki satır yorum satırı yapılacak
        //logKaydet()
        public static bool logRecording = true;
        public static string main_log = "";

        public static void log(Exception ex)
        {
            if (f.logRecording)
            {
                string e = y.p(y.hataOlustu, ex);
                f.main_log += e + "\n\n";
            }
        }

        public static void hata(object hata, Action act = null)
        {
            string e = "";
            if (hata is string) e = y.p(y.hataOlustu, hata as string);
            if (hata is Exception) e = y.p(y.hataOlustu, (hata as Exception).ToString());
            if (hata is string[])
            {
                string[] h = hata as string[];
                if (h.Length==2)
                    e = y.p(h[0], h[1]);
                else if (h.Length == 1)
                    e = h[0];
            }
            if (f.c is anaEkran)
            {
                if (f.main_log != "")
                {
                    f.logKaydet("\n\n_____________________\n\n" + f.main_log + "\n\n_____________________\n\n");
                    f.main_log = "";
                }
                if (act != null) f.mesajGoster(e, act);
                else f.mesajGoster(e);
            }
            else
                f.anaEkranaDonVeMesajGoster(e);
            return;
        }

        public static void boyutlariOlustur(string sayfa)
        {
            if (sayfa == "bolumler")
            {
                RelativeLayout ustMenu = c.FindViewById<RelativeLayout>(Resource.Id.ustMenu_bolumler);
                var p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, px(150));
                ustMenu.LayoutParameters = p;
                ustMenu.RequestLayout();

                ImageView bolumGorseli = c.FindViewById<ImageView>(Resource.Id.bolumGorseli);
                bolumGorseli.SetBackgroundColor(r.bolumGorseliArkaplanR);
                bolumGorseli.SetScaleType(ImageView.ScaleType.FitXy);
                var p3 = new RelativeLayout.LayoutParams(px(1080), px(7490));//height bolumler.cs:bolumlerYukle():141de değiştiriliyor (sayfa 1den sonrası için)
                p3.AddRule(LayoutRules.AlignParentTop);
                p3.AddRule(LayoutRules.CenterHorizontal);
                bolumGorseli.LayoutParameters = p3;
                bolumGorseli.RequestLayout();


                Button oncekiBolumlerBtn = c.FindViewById<Button>(Resource.Id.oncekiBolumlerBtn);
                oncekiBolumlerBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(60));
                var p2 = new LinearLayout.LayoutParams(px(150), px(150));
                p2.Gravity = GravityFlags.CenterHorizontal;
                p2.SetMargins(0, px(40), 0, px(80));
                oncekiBolumlerBtn.LayoutParameters = p2;
                oncekiBolumlerBtn.RequestLayout();

                Button sonrakiBolumlerBtn = c.FindViewById<Button>(Resource.Id.sonrakiBolumlerBtn);
                sonrakiBolumlerBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(60));
                p2 = new LinearLayout.LayoutParams(px(150), px(150));
                p2.Gravity = GravityFlags.CenterHorizontal;
                p2.SetMargins(0, px(100), 0, px(40));
                sonrakiBolumlerBtn.LayoutParameters = p2;
                sonrakiBolumlerBtn.RequestLayout();
            }
            else if (sayfa == "bulmacaEkrani")
            {
                RelativeLayout ustMenu2 = c.FindViewById<RelativeLayout>(Resource.Id.ustMenu_bulmacaEkrani);
                var p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, px(175));
                ustMenu2.LayoutParameters = p;
                ustMenu2.RequestLayout();

                ImageView soruGorseli = c.FindViewById<ImageView>(Resource.Id.soruGorseli);
                soruGorseli.SetScaleType(ImageView.ScaleType.FitCenter);

                //!!!!!!! burası değiştirilirse OnConfigurationChanged() fonksiyonu içinden de değiştirilmeli
                int soruGorseli_height = px(500);

                //ekran yüksekiği belli yüksekliklerden sonra belli sayıya göre belirlenir (3te biri gibi)
                if (ekranYuksekligi > 1920) soruGorseli_height = Convert.ToInt32(Math.Round(ekranYuksekligi / 3));
                if (ekranYuksekligi > 2400 && ekranGenisligi > 1440) soruGorseli_height = Convert.ToInt32(Math.Round(ekranYuksekligi / 2.5));

                var pq = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, soruGorseli_height);
                soruGorseli.LayoutParameters = pq;
                soruGorseli.RequestLayout();


                ImageView gorselCevir = c.FindViewById<ImageView>(Resource.Id.gorselCevir);
                gorselCevir.SetImageResource(Resource.Drawable.p_gorselCevir);
                gorselCevir.SetScaleType(ImageView.ScaleType.FitCenter);
                pq = new FrameLayout.LayoutParams(px(60), px(60));
                pq.Gravity = GravityFlags.Right;
                int _ = px(50);
                pq.SetMargins(_, _, _, _);
                gorselCevir.LayoutParameters = pq;
                gorselCevir.RequestLayout();


                LinearLayout gorselAltiSatiri = c.FindViewById<LinearLayout>(Resource.Id.gorselAltiSatiri);
                gorselAltiSatiri.SetPadding(px(30), 0, px(30), 0);

                TextView gorselSuresiTxt = c.FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                gorselSuresiTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(36));

                TextView gorselSayisiTxt = c.FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                gorselSayisiTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(36));

                ScrollView soruScroll = c.FindViewById<ScrollView>(Resource.Id.soruScroll);
                soruScroll.SetPadding(px(30), 0, px(30), 0);
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, px(235));
                p.SetMargins(0, 0, 0, px(40));
                p.Gravity = GravityFlags.Center;
                soruScroll.LayoutParameters = p;
                soruScroll.RequestLayout();

                TextView soru = c.FindViewById<TextView>(Resource.Id.soru);
                soru.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(52));

                Button harfAlBtn = c.FindViewById<Button>(Resource.Id.harfAlBtn);
                harfAlBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(32));
                p = new LinearLayout.LayoutParams(px(350), px(100));
                harfAlBtn.LayoutParameters = p;
                harfAlBtn.RequestLayout();

                Button uzaklastirBtn = c.FindViewById<Button>(Resource.Id.uzaklastirBtn);
                uzaklastirBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(32));
                p = new LinearLayout.LayoutParams(px(350), px(100));
                uzaklastirBtn.LayoutParameters = p;
                uzaklastirBtn.RequestLayout();

                Button kelimeyiAcBtn = c.FindViewById<Button>(Resource.Id.kelimeyiAcBtn);
                kelimeyiAcBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(32));
                p = new LinearLayout.LayoutParams(px(350), px(100));
                kelimeyiAcBtn.LayoutParameters = p;
                kelimeyiAcBtn.RequestLayout();
            }
            else if (sayfa == "anaEkran")
            {
                ImageView anaEkranGorseli = c.FindViewById<ImageView>(Resource.Id.anaEkranGorseli);
                anaEkranGorseli.SetBackgroundColor(r.anaEkranGorseliArkaplanR);
                anaEkranGorseli.SetScaleType(ImageView.ScaleType.Center);
                anaEkranGorseli.SetBackgroundResource(Resource.Drawable.girisEkrani);
                var p2 = new RelativeLayout.LayoutParams(px(1080), px(1920));
                p2.AddRule(LayoutRules.CenterInParent);
                anaEkranGorseli.LayoutParameters = p2;
                anaEkranGorseli.RequestLayout();

                ImageView anaEkranCikis = c.FindViewById<ImageView>(Resource.Id.anaEkranCikis);
                anaEkranCikis.SetScaleType(ImageView.ScaleType.FitXy);
                var p = new LinearLayout.LayoutParams(px(80), px(80));
                p.Gravity = GravityFlags.Top | GravityFlags.Right;
                int _ = px(50);
                p.SetMargins(_, _, _, _);
                anaEkranCikis.LayoutParameters = p;
                anaEkranCikis.RequestLayout();

                Button basla = c.FindViewById<Button>(Resource.Id.basla);
                basla.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(60));
                _ = px(40);
                basla.SetPadding(_, _, _, _);
                p = new LinearLayout.LayoutParams(px(500), px(170));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(350));
                basla.LayoutParameters = p;
                basla.RequestLayout();
            }
            else if (sayfa == "acilisEkrani")
            {
                ImageView TFpng = c.FindViewById<ImageView>(Resource.Id.TFpng);
                var p = new RelativeLayout.LayoutParams(px(650), px(96));
                TFpng.LayoutParameters = p;
                TFpng.RequestLayout();

                TextView versionTxtAE = c.FindViewById<TextView>(Resource.Id.versionTxtAE);
                versionTxtAE.SetTypeface(versionTxtAE.Typeface, TypefaceStyle.Bold);
                versionTxtAE.TextAlignment = TextAlignment.Center;
                versionTxtAE.SetTextColor(r.yukleniyorTxtYaziR);
                versionTxtAE.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
                versionTxtAE.SetPadding(0, 0, 0, px(200));
                var p2 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                versionTxtAE.LayoutParameters = p2;
                versionTxtAE.RequestLayout();
            }
            //else if (sayfa == "yukleniyor") { } //yukleniyorOlustur()
            //else if (sayfa == "magaza") { } //magazaPenceresiniAc()
            //else if (sayfa == "menu") { } //menuPenceresiniAc()
            //else if (sayfa == "mesajGoster") { } //mesajGoster()
            //else if (sayfa == "sandikAc") { } //sandikGoster()
            //else if (sayfa == "verileriYonet") { } //verileriYonetPenceresiniAc() uyumlu gözüktüğü için değişiklik yapılmadı [!]
            //else if (sayfa == "profil") { } //profilPenceresiniAc()
            //else if (sayfa == "ayarlar") { } //ayarlarPenceresiniAc()
            //else if (sayfa == "bulmacaEkrani_gecildi") { } //bulmacaEkrani.cs:bolumSonuPenceresiniAc()
        }

        //ölçekli çözünürlük ayarlama
        public static int px(int px)
        {
            int r = Convert.ToInt32(Math.Round(px * olceklendirmeOrani));

            return r;
        }
        public static void cihazCozunurlukAyarlari()
        {
            //varsayılan çözünürlük
            double dW = 1080, dH = 1920;


            //cihazın şimdiki çözünürlüğü
            var m = c.Resources.DisplayMetrics;
            ekranGenisligi = m.WidthPixels;
            ekranYuksekligi = m.HeightPixels;


            //varsayılan ayarlar full hd
            if (ekranGenisligi == dW) olceklendirmeOrani = 1;
            else
            {//orana göre küçültme veya büyültme uygulanır

                //genişlik boyutu 1440dan fazla ise genişletme yapmıyoruz
                if (ekranGenisligi > 1440) ekranGenisligi = dW;
                
                //küçültme veya büyültme oranı
                olceklendirmeOrani = ekranGenisligi / dW;
            }

            if (ekranYuksekligi <= 1280)
            {
                olceklendirmeOrani = ekranYuksekligi / dH;
            }
        }

        public static Drawable LD(Drawable d, string color = "")
        {
            if (color == "") color = r.loadingBarR;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                d.SetColorFilter(AndroidX.Core.Graphics.BlendModeColorFilterCompat.CreateBlendModeColorFilterCompat(Color.ParseColor(color), AndroidX.Core.Graphics.BlendModeCompat.SrcAtop));
            }
            else
            {
                #pragma warning disable 0618
                d.SetColorFilter(Color.ParseColor(color), PorterDuff.Mode.SrcAtop);
                #pragma warning restore 0618
            }

            return d;
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string tokenOlustur()
        {
            TimeSpan kacSaniyeGecti = DateTime.Now - local_time;
            DateTime server_now = server_utc_time.AddSeconds(kacSaniyeGecti.TotalSeconds);

            long yil = server_now.Year;
            long ay = server_now.Month;
            long gun = server_now.Day;
            long saat = server_now.Hour;
            long dk = server_now.Minute;
            long sn = server_now.Second;

            //kolay bulunmasını zorlaştırmak için şu anki verilere belirli sayılar ekliyoruz
            yil += 561701840414;
            ay += 17862422145;
            gun += 586121244;
            saat += 8944542165;
            dk += 48412154256;
            sn += 87684254125;

            long hepsininToplami = yil + ay + gun + saat + dk + sn;

            string karistir = hepsininToplami + "." + gun + "-" + dk + "/" + yil + ":" + saat + ";" + sn + "\\" + ay;

            string hash = ComputeSha256Hash(karistir + "tknbymmyv1");


            Random rnd = new Random();
            string rastgeleSahteSayi = rnd.Next(1561, 999999999) + "-" + rnd.Next(1561, 999999999);

            string fakeHash = ComputeSha256Hash(rastgeleSahteSayi + "tknbymmyv1");

            string token = hash + fakeHash.Substring(0, 16);

            //tokena version bilgisini ekliyoruz
            token += "_v" + _version;

            return token;
        }

        public static async Task<string> GetResponseFromURI(string url)
        {
            if (hasServerTime)
                url = url + "&t=" + tokenOlustur();

            Uri u = new Uri(url);
            var response = "";


            int kacOldu = 0;
            bool resultStatusErr = true;
            bool timeOut = false;
            do
            {
                try
                {
                    kacOldu++;
                    using (var client = new HttpClient() { Timeout = new TimeSpan(0,0, f.baglantiTimeOutSuresiSN) })
                    {
                        HttpResponseMessage result = await client.GetAsync(u);
                        if (result.IsSuccessStatusCode)
                        {
                            response = await result.Content.ReadAsStringAsync();
                        }
                        resultStatusErr = !result.IsSuccessStatusCode;
                    }
                }
                catch(TaskCanceledException)
                {
                    timeOut = true;
                }
                catch (Exception ex)
                {
                    string e = y.p(y.hataOlustu, ex);
                    if (logRecording)
                    {
                        main_log += e + "\n\n";
                    }
                }

                //timeout olduysa direk çıkıyoruz
                if (timeOut) break;
                //* kez * ms aralıklarla bağlanmayı tekrar dene, * sn geçtikten sonra hâlâ bağlantı yoksa sonlandır
                //başarısız bağlantı için || başarısız token eşleşmesi için || bilinmeyen başarısız denemeler için
                else if (resultStatusErr || response == "yok" || response == "" || response == null)
                {
                    if (kacOldu >= 20) break;
                    else
                    {
                        await Task.Delay(100);
                    }
                }
                else break;
            }
            while (true);



            if (response == "bakim")
            {
                bakimAktif = true;
                if (hasServerTime)
                {
                    if (acilisTamamlandi)
                    {
                        anaEkranaDonVeMesajGoster(y.sunucuBakimda);
                    }
                    return "";
                }
            }

            if (response == "verup")
            {
                bakimAktif = true;
                if (hasServerTime)
                {
                    if (acilisTamamlandi)
                    {
                        anaEkranaDonVeMesajGoster(y.guncellemeVar);
                    }
                    return "";
                }
            }

            if (response == "yok") response = "";

            return response;
        }

        //post ile işlem yapıldığında cloudflare domain üzerinden erişimi engelliyor
        public static async Task<string> SendPostToURI(string url, Dictionary<string, string> veriler)
        {
            url = url + "?t=" + tokenOlustur();

            Uri u = new Uri(url);
            var response = "";
            StringContent stringContent = null;

            try
            {
                var stringFormParams = new Func<IDictionary<string, string>, string>((dic) =>
                {
                    string result = "";
                    foreach (var param in dic)
                    {
                        if (result.Length > 0) { result += "&"; }
                        result += param.Key + "=" + WebUtility.UrlEncode(param.Value);
                    }
                    return result;
                }).Invoke(veriler);

                stringContent = new StringContent(stringFormParams, Encoding.UTF8, "application/x-www-form-urlencoded");
            }
            catch(Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                    anaEkranaDonVeMesajGoster(e);
                    return "";
                }
            }


            int kacOldu = 0;
            bool resultStatusErr = true;
            bool timeOut = false;
            do
            {
                try
                {
                    using (var client = new HttpClient() { Timeout = new TimeSpan(0, 0, f.baglantiTimeOutSuresiSN) })
                    {

                        kacOldu++;

                        HttpResponseMessage result = await client.PostAsync(u, stringContent);
                        //önceki versiyon YEDEK
                        //HttpResponseMessage result = await client.PostAsync(u, new FormUrlEncodedContent(veriler));
                        if (result.IsSuccessStatusCode)
                        {
                            response = await result.Content.ReadAsStringAsync();
                        }

                        resultStatusErr = !result.IsSuccessStatusCode;
                    }
                }
                catch (TaskCanceledException)
                {
                    timeOut = true;
                }
                catch (Exception ex)
                {
                    string e = y.p(y.hataOlustu, ex);
                    if (logRecording)
                    {
                        main_log += e + "\n\n";
                    }
                }

                //timeout olduysa direk çıkıyoruz
                if (timeOut) break;
                //* kez * ms aralıklarla bağlanmayı tekrar dene, * sn geçtikten sonra hâlâ bağlantı yoksa sonlandır
                //başarısız bağlantı için || başarısız token eşleşmesi için || bilinmeyen başarısız denemeler için
                if (resultStatusErr || response == "yok" || response == "" || response == null)
                {
                    if (kacOldu >= 20) break;
                    else
                    {
                        await Task.Delay(100);
                    }
                }
                else break;
            }
            while (true);



            if (response == "bakim")
            {
                bakimAktif = true;
                if (hasServerTime)
                {
                    if (acilisTamamlandi)
                    {
                        anaEkranaDonVeMesajGoster(y.sunucuBakimda);
                    }
                    return "";
                }
            }

            if (response == "verup")
            {
                bakimAktif = true;
                if (hasServerTime)
                {
                    if (acilisTamamlandi)
                    {
                        anaEkranaDonVeMesajGoster(y.guncellemeVar);
                    }
                    return "";
                }
            }

            if (response == "yok") response = "";

            return response;
        }

        public static void getServerTime()
        {
            //server'dan ana saati çekiyoruz (tokenda kullanmak için)
            string server_time = "";
            var t2 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?st=e"));
            try
            {
                t2.Wait((new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(f.baglantiTimeOutSuresiSN)).Token));
            }
            catch { }//kontrollü catch - 5 saniyeyi geçerse exception

            if (t2.Result != null) server_time = t2.Result;
            t2.Dispose();

            f.local_time = DateTime.Now;

            if (server_time != "")
            {
                //şifrelemeyi çözüyoruz
                string ts = server_time;
                ts = ts.Replace("*", "0");
                ts = ts.Replace("/", "1");
                ts = ts.Replace("{", "2");
                ts = ts.Replace("+", "3");
                ts = ts.Replace("_", "4");
                ts = ts.Replace("!", "5");
                ts = ts.Replace("%", "6");
                ts = ts.Replace("(", "7");
                ts = ts.Replace(")", "8");
                ts = ts.Replace("#", "9");
                ts = ts.Replace("$", "-");
                ts = ts.Replace("]", " ");
                ts = ts.Replace("½", ":");

                f.server_utc_time = DateTime.ParseExact(ts, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                f.hasServerTime = true;
            }
        }

        public static async Task<Bitmap> GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            Uri u = new Uri(url);

            using (var webClient = new WebClient())
            {
                try
                {
                    var request = webClient.DownloadDataTaskAsync(u);
                    var timeout = Task.Delay(TimeSpan.FromSeconds(f.gorselTimeOutSuresiSN));
                    var completed = await Task.WhenAny(request, timeout);

                    if (completed == timeout)
                    {
                        webClient.CancelAsync();
                    }

                    var imageBytes = request.Result;

                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    }
                    imageBytes = null;
                }
                catch (Exception ex)
                {
                    string e = y.p(y.hataOlustu, ex);
                    if (logRecording)
                    {
                        main_log += e + "\n\n";
                    }
                    anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                    return null;
                }
            }

            if (imageBitmap == null)
            {
                anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                return null;
            }

            return imageBitmap;
        }

        public static void yukleniyorOlustur()
        {
            acikPencereleriKapat();

            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

            RelativeLayout rel_ust = new RelativeLayout(c);
            rel_ust.Tag = "rel_ust";
            rel_ust.SetBackgroundColor(r.yukleniyorArkaplanR);
            var p = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            anaView.AddView(rel_ust, p);

            FrameLayout topFragment = new FrameLayout(c);
            topFragment.Tag = "topFragment";
            var p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            p2.AddRule(LayoutRules.AlignParentTop);
            rel_ust.AddView(topFragment, p2);

            RelativeLayout relat = new RelativeLayout(c) { Id = View.GenerateViewId() };
            relat.Tag = "relat";
            relat.SetBackgroundColor(r.yukleniyorArkaplanR);
            p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
            p2.AddRule(LayoutRules.CenterInParent);
            rel_ust.AddView(relat, p2);

            ProgressBar loading = new ProgressBar(c);
            loading.Tag = "loadd";
            loading.ScrollBarFadeDuration = 0;
            loading.IndeterminateDrawable = LD(loading.IndeterminateDrawable, r.yukleniyorLoadingBarR);
            loading.Indeterminate = true;
            p2 = new RelativeLayout.LayoutParams(px(200), px(200));
            p2.AddRule(LayoutRules.CenterInParent);
            relat.AddView(loading, p2);

            TextView yukleniyorTxt = new TextView(c);
            yukleniyorTxt.Tag = "yukleniyorTxt";
            yukleniyorTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
            yukleniyorTxt.SetTypeface(yukleniyorTxt.Typeface, TypefaceStyle.Bold);
            yukleniyorTxt.SetTextColor(r.yukleniyorTxtYaziR);
            yukleniyorTxt.TextAlignment = TextAlignment.Center;
            yukleniyorTxt.Gravity = GravityFlags.Center;
            p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            p2.AddRule(LayoutRules.Below, relat.Id);
            p2.SetMargins(0, px(300), 0, 0);
            rel_ust.AddView(yukleniyorTxt, p2);

            FrameLayout bottomFragment = new FrameLayout(c);
            bottomFragment.Tag = "bottomFragment";
            p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
            p2.AddRule(LayoutRules.AlignParentBottom);
            rel_ust.AddView(bottomFragment, p2);

            TextView versionTxt = new TextView(c);
            versionTxt.Tag = "versionTxt";
            versionTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
            versionTxt.SetTextColor(r.yukleniyorVersionTxtYaziR);
            versionTxt.SetTypeface(versionTxt.Typeface, TypefaceStyle.Bold);
            versionTxt.TextAlignment = TextAlignment.Center;
            versionTxt.SetPadding(0, 0, 0, px(200));
            var p3 = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.WrapContent, FrameLayout.LayoutParams.WrapContent);
            p3.Gravity = GravityFlags.Center;
            bottomFragment.AddView(versionTxt, p3);


            yukleniyorTxt.Text = y.yukleniyor;
            versionTxt.Text = y.p(y.version, _version);
        }

        public static void yukleniyorKaldir()
        {
            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

            RelativeLayout rel_ust = anaView.FindViewWithTag("rel_ust") as RelativeLayout;

            int[] gecisler = new int[8]
            {
                    Resource.Animation.abc_fade_out,
                    Resource.Animation.abc_popup_exit,
                    Resource.Animation.abc_shrink_fade_out_from_bottom,
                    Resource.Animation.abc_slide_out_bottom,
                    Resource.Animation.abc_slide_out_top,
                    Resource.Animation.abc_tooltip_exit,
                    Resource.Animation.ExitToLeft,
                    Resource.Animation.ExitToRight
            };

            var anim = Android.Views.Animations.AnimationUtils.LoadAnimation(c, gecisler[new Random().Next(0, 8)]);

            rel_ust.ClearAnimation();
            rel_ust.StartAnimation(anim);
            rel_ust.Visibility = ViewStates.Gone;
            rel_ust.Dispose();
        }

        public static void acikPencereleriKapat()
        {
            if (acikPencereler != null)
            {
                for (int i = 0; i < acikPencereler.Count; i++)
                {
                    try
                    {
                        if (acikPencereler[i] != null)
                            acikPencereler[i].Dismiss();
                    }
                    catch { }//kontrollü catch
                    try
                    {
                        if (acikPencereler.Count > 0)
                            BuPencereKapandi(acikPencereler[i]);
                    }
                    catch { }//kontrollü catch
                }
            }
        }

        public static void BuPencereAcildi(PopupWindow pw)
        {
            acikPencereler.Add(pw);
        }

        public static void BuPencereKapandi(PopupWindow pw)
        {
            try
            {
                if (acikPencereler.Count > 0)
                    acikPencereler.RemoveAt(acikPencereler.IndexOf(pw));
            }
            catch { }//kontrollü catch
        }

        public static soruIstatistikIDleriTBL soruIstIDleriBul(int suAnkiSoruID)
        {
            soruIstatistikIDleriTBL soru = null;
            if (soruIstatistikIDleriTBL_ != null)
            {
                foreach (soruIstatistikIDleriTBL s in soruIstatistikIDleriTBL_)
                {
                    if (s.soruID == suAnkiSoruID)
                    {
                        soru = s;
                        break;
                    }
                }
            }
            return soru;
        }

        public static bool bolumGecilmemisse(int soruID)
        {
            bool bolumGecilmemis = true;

            if (f.soruIstatistikTBL_ != null)
            {
                if (f.soruIstatistikTBL_.bolumGecildi)
                {
                    bolumGecilmemis = false;
                    //bölüm daha önce geçilmiş
                }
            }

            return bolumGecilmemis;
        }

        public static void tumNesneleriDeaktifEt(ViewGroup vg = null)
        {
            acikPencereleriKapat();

            if (vg == null)
            {
                ViewGroup anaView = f.c.Window.DecorView.RootView as ViewGroup;
                vg = anaView;
            }

            if (vg is ViewGroup)
            {
                for (int i = 0; i < vg.ChildCount; i++)
                {
                    View child = vg.GetChildAt(i);
                    child.Enabled = false;

                    if (child is ViewGroup)
                    {
                        tumNesneleriDeaktifEt((ViewGroup)child);
                    }
                }
            }
        }

        public static void bolumuBaslat(int soruID, bool neOlursaOlsunBaslat = false)
        {
            //Sonraki bölüme geçme kodları
            if (neOlursaOlsunBaslat || (soruID != -1 && (soruID - 1) != f.sorularTBL_[f.sorularTBL_.Length - 1].id))
            {
                veriGonder vg = new veriGonder();
                vg.gonderilecekVeriAdi = "soruID";
                vg.gonderilecekVeri = soruID;

                ekranGecisi(typeof(bulmacaEkrani), vg);
                c.Finish();
            }
            else
            {
                //Eğer son bölümse veya soruid -1 ise bölümlere dön
                f.ekranGecisi(typeof(bolumler));
                c.Finish();
            }
        }

        public static async Task temelVerileriCek()
        {
            //sadece bakım aktif değilse çek; bakım aktif ise verileri çekmemizin anlamı yok
            if (!bakimAktif)
            {
                try
                {
                    var t = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=tml"));
                    await t.ConfigureAwait(false);
                    if (t.Result != null)
                    {
                        string r = t.Result;
                        string ayrac = "___";
                        int beklenenJsonMiktari = 4;

                        if (r.IndexOf(ayrac) >= 0)
                        {
                            string[] bol = r.Split(ayrac);
                            if (bol.Length == beklenenJsonMiktari)
                            {
                                bolumlerTBL_ = JsonConvert.DeserializeObject<bolumlerTBL[]>(bol[0]);
                                sorularTBL_ = JsonConvert.DeserializeObject<sorularTBL[]>(bol[1]);
                                basarimlarTBL_ = JsonConvert.DeserializeObject<basarimlarTBL[]>(bol[2]);
                                skorTablolari_ = JsonConvert.DeserializeObject<skorTablolari[]>(bol[3]);
                            }
                        }

                    }
                    else
                    {
                        if (acilisTamamlandi)
                        {
                            mesajGoster(y.baziVerilerYuklenemedi);
                        }
                        return;
                    }
                }
                catch { }//kontrollü catch


                if (bolumlerTBL_ == null)
                {
                    if (acilisTamamlandi)
                    {
                        mesajGoster(y.bolumlerYuklenemedi);
                    }
                    return;
                }

                if (sorularTBL_ == null)
                {
                    if (acilisTamamlandi)
                    {
                        mesajGoster(y.sorularYuklenemedi);
                    }
                    return;
                }

                if (basarimlarTBL_ == null)
                {
                    if (acilisTamamlandi)
                    {
                        mesajGoster(y.basarimlarYuklenemedi);
                    }
                    return;
                }

                if (skorTablolari_ == null)
                {
                    if (acilisTamamlandi)
                    {
                        mesajGoster(y.skorTablolariYuklenemedi);
                    }
                    return;
                }


            }
        }

        public async static Task<bool> bolumAyarlariCek(int soruID = 0)
        {
            //önceki kaydı temizliyoruz
            soruIstatistikTBL_ = null;

            var t2 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=sict&_sid=" + soruID + "&_g=" + googleProfile_.Id));
            await t2.ConfigureAwait(false);
            if (t2.Result != null)
            {
                //veritabanında bu soruya ait kayıt varsa
                if (t2.Result != "")
                {
                    soruIstatistikTBL_ = JsonConvert.DeserializeObject<soruIstatistikTBL>(t2.Result);
                }
            }
            else
            {
                anaEkranaDonVeMesajGoster(y.sorularYuklenemedi);
                return false;
            }

            //soruIstatistikIDleriTBL_'de idler varsa ve soruIstatistik boş ise bu verinin düzgün çekilmediği anlamına gelir ve dur
            if (soruIstatistikIDleriTBL_ != null)
            {
                if (soruIstIDleriBul(soruID) != null && soruIstatistikTBL_ == null)
                {
                    anaEkranaDonVeMesajGoster(y.sorularYuklenemedi);
                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> oncekiBolumlerGecildiMi(int soruID)
        {
            bool r = true;
            var t2 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=obg&_sid=" + soruID + "&_g=" + googleProfile_.Id));
            await t2.ConfigureAwait(false);
            if (t2.Result != null)
            {
                //sonuç 0 ise önceki bölümlerde geçilmemiş olan var, değilse devam et
                if (t2.Result == "0")
                {
                    r = false;

                    //eğer bölümlerde bi sıkıntı varsa problemi düzeltebilmesi olasılığı ile tekrar temel verileri çekmeyi deniyoruz
                    await temelVerileriCek();
                }
            }
            else
            {
                anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                return false;
            }

            return r;
        }

        public static void anaEkranaDonVeMesajGoster(string mesaj)
        {
            veriGonder vg = new veriGonder();
            vg.gonderilecekVeriAdi = "mesaj";
            vg.gonderilecekVeri = mesaj;

            if (c != null)
            {
                c.RunOnUiThread(delegate
                {
                    ekranGecisi(typeof(anaEkran), vg);
                    c.Finish();
                });
            }
        }

        public static void ortakAyarlar(Window window)
        {
            window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                window.SetDecorFitsSystemWindows(false);
                ViewGroup anaView = window.DecorView.RootView as ViewGroup;
                anaView.WindowInsetsController?.Hide(WindowInsets.Type.SystemBars());
            }
            else
            {
                #pragma warning disable 0618
                window.DecorView.SystemUiVisibility = (StatusBarVisibility)
                                         (SystemUiFlags.LowProfile
                                         | SystemUiFlags.Fullscreen
                                         | SystemUiFlags.HideNavigation
                                         | SystemUiFlags.Immersive
                                         | SystemUiFlags.ImmersiveSticky);
                #pragma warning restore 0618
            }
        }

        public static void menuPenceresiniAc(string sayfa = null)
        {
            try
            {
                c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                acikPencereleriKapat();

                ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                //loading çıkarıyoruz
                ProgressBar loadingBar = new ProgressBar(c);
                loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                loadingBar.Indeterminate = true;
                RelativeLayout rl = new RelativeLayout(c);
                rl.SetGravity(GravityFlags.Center);
                rl.AddView(loadingBar);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                anaView.AddView(rl, param);


                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View menu = inflater.Inflate(Resource.Layout.menu, null, false);


                int yukseklik = 1250;

                if (sayfa == "bulmaca") yukseklik = 1400;


                PopupWindow popupWindow = new PopupWindow(menu, LinearLayout.LayoutParams.WrapContent, px(yukseklik));

                popupWindow.Focusable = false;
                popupWindow.OutsideTouchable = false;

                View pcw = popupWindow.ContentView;

                ViewGroup popUpanaView = pcw.RootView as ViewGroup;


                RelativeLayout menuR1 = pcw.FindViewById<RelativeLayout>(Resource.Id.menuR1);
                menuR1.SetBackgroundResource(Resource.Drawable.p_arkaplan);

                c.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));


                LinearLayout menuL2 = pcw.FindViewById<LinearLayout>(Resource.Id.menuL2);
                menuL2.SetPadding(0, px(150), 0, px(50));
                var p = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
                p.AddRule(LayoutRules.CenterInParent);
                menuL2.LayoutParameters = p;
                menuL2.RequestLayout();

                ImageView menuBolumlereDonIco = pcw.FindViewById<ImageView>(Resource.Id.menuBolumlereDonIco);
                var p2 = new LinearLayout.LayoutParams(px(60), px(60));
                int _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuBolumlereDonIco.LayoutParameters = p2;
                menuBolumlereDonIco.RequestLayout();

                TextView menuBolumlereDonTxt = pcw.FindViewById<TextView>(Resource.Id.menuBolumlereDonTxt);
                menuBolumlereDonTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuBolumlereDonTxt.LayoutParameters = p2;
                menuBolumlereDonTxt.RequestLayout();

                ImageView menuMagazaIco = pcw.FindViewById<ImageView>(Resource.Id.menuMagazaIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuMagazaIco.LayoutParameters = p2;
                menuMagazaIco.RequestLayout();

                TextView menuMagazaTxt = pcw.FindViewById<TextView>(Resource.Id.menuMagazaTxt);
                menuMagazaTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuMagazaTxt.LayoutParameters = p2;
                menuMagazaTxt.RequestLayout();

                ImageView menuBasarimlarIco = pcw.FindViewById<ImageView>(Resource.Id.menuBasarimlarIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuBasarimlarIco.LayoutParameters = p2;
                menuBasarimlarIco.RequestLayout();

                TextView menuBasarimlarTxt = pcw.FindViewById<TextView>(Resource.Id.menuBasarimlarTxt);
                menuBasarimlarTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuBasarimlarTxt.LayoutParameters = p2;
                menuBasarimlarTxt.RequestLayout();

                ImageView menuAyarlarIco = pcw.FindViewById<ImageView>(Resource.Id.menuAyarlarIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuAyarlarIco.LayoutParameters = p2;
                menuAyarlarIco.RequestLayout();

                TextView menuAyarlarTxt = pcw.FindViewById<TextView>(Resource.Id.menuAyarlarTxt);
                menuAyarlarTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuAyarlarTxt.LayoutParameters = p2;
                menuAyarlarTxt.RequestLayout();

                ImageView menuMuzikIco = pcw.FindViewById<ImageView>(Resource.Id.menuMuzikIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuMuzikIco.LayoutParameters = p2;
                menuMuzikIco.RequestLayout();

                TextView menuMuzikTxt = pcw.FindViewById<TextView>(Resource.Id.menuMuzikTxt);
                menuMuzikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuMuzikTxt.LayoutParameters = p2;
                menuMuzikTxt.RequestLayout();

                TextView menuMuzikDurumTxt = pcw.FindViewById<TextView>(Resource.Id.menuMuzikDurumTxt);
                menuMuzikDurumTxt.SetPadding(0, 0, px(40), 0);
                menuMuzikDurumTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                _ = px(20);
                p.SetMargins(_, _, _, _);
                p.AddRule(LayoutRules.AlignParentRight);
                menuMuzikDurumTxt.LayoutParameters = p;
                menuMuzikDurumTxt.RequestLayout();

                ImageView menuSeslerIco = pcw.FindViewById<ImageView>(Resource.Id.menuSeslerIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuSeslerIco.LayoutParameters = p2;
                menuSeslerIco.RequestLayout();

                TextView menuSeslerTxt = pcw.FindViewById<TextView>(Resource.Id.menuSeslerTxt);
                menuSeslerTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuSeslerTxt.LayoutParameters = p2;
                menuSeslerTxt.RequestLayout();

                TextView menuSeslerDurumTxt = pcw.FindViewById<TextView>(Resource.Id.menuSeslerDurumTxt);
                menuSeslerDurumTxt.SetPadding(0, 0, px(40), 0);
                menuSeslerDurumTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                _ = px(20);
                p.SetMargins(_, _, _, _);
                p.AddRule(LayoutRules.AlignParentRight);
                menuSeslerDurumTxt.LayoutParameters = p;
                menuSeslerDurumTxt.RequestLayout();

                ImageView menuYeniliklerIco = pcw.FindViewById<ImageView>(Resource.Id.menuYeniliklerIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuYeniliklerIco.LayoutParameters = p2;
                menuYeniliklerIco.RequestLayout();

                TextView menuYeniliklerTxt = pcw.FindViewById<TextView>(Resource.Id.menuYeniliklerTxt);
                menuYeniliklerTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuYeniliklerTxt.LayoutParameters = p2;
                menuYeniliklerTxt.RequestLayout();

                ImageView menuCikisIco = pcw.FindViewById<ImageView>(Resource.Id.menuCikisIco);
                p2 = new LinearLayout.LayoutParams(px(60), px(60));
                _ = px(30);
                p2.SetMargins(px(80), _, _, _);
                p2.Gravity = GravityFlags.Center;
                menuCikisIco.LayoutParameters = p2;
                menuCikisIco.RequestLayout();

                TextView menuCikisTxt = pcw.FindViewById<TextView>(Resource.Id.menuCikisTxt);
                menuCikisTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                menuCikisTxt.LayoutParameters = p2;
                menuCikisTxt.RequestLayout();



                menuMagazaTxt.Text = y.menuMagaza_btn;

                LinearLayout menuMagaza = pcw.FindViewById<LinearLayout>(Resource.Id.menuMagaza);
                menuMagaza.Click += async delegate
                {
                    menuMagaza.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuMagaza.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuMagaza.SetBackgroundColor(r.menuButonArkaplanR);

                    popupWindow.Dismiss();

                    magazaPenceresiniAc();
                };


                menuBasarimlarTxt.Text = y.menuBasarimlar_btn;

                LinearLayout menuBasarimlar = pcw.FindViewById<LinearLayout>(Resource.Id.menuBasarimlar);
                menuBasarimlar.Click += async delegate
                {
                    menuBasarimlar.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuBasarimlar.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuBasarimlar.SetBackgroundColor(r.menuButonArkaplanR);

                    popupWindow.Dismiss();

                    profilPenceresiniAc();
                };


                menuAyarlarTxt.Text = y.menuAyarlar_btn;

                LinearLayout menuAyarlar = pcw.FindViewById<LinearLayout>(Resource.Id.menuAyarlar);
                menuAyarlar.Click += async delegate
                {
                    menuAyarlar.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuAyarlar.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuAyarlar.SetBackgroundColor(r.menuButonArkaplanR);

                    popupWindow.Dismiss();

                    ayarlarPenceresiniAc();
                };



                menuMuzikTxt.Text = y.menuMuzik_btn;

                if (oAyarlariTBL_.muzik)
                {
                    menuMuzikDurumTxt.Text = "AÇIK";
                    menuMuzikDurumTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                }
                else
                {
                    menuMuzikDurumTxt.Text = "KAPALI";
                    menuMuzikDurumTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                }

                RelativeLayout menuMuzik = pcw.FindViewById<RelativeLayout>(Resource.Id.menuMuzik);
                menuMuzik.Click += async delegate
                {
                    menuMuzik.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuMuzik.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuMuzik.SetBackgroundColor(r.menuButonArkaplanR);

                    if (menuMuzikDurumTxt.Text == "AÇIK")
                    {
                        await muzikAyar("kapa");

                        menuMuzikDurumTxt.Text = "KAPALI";
                        menuMuzikDurumTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await muzikAyar("aç");

                        menuMuzikDurumTxt.Text = "AÇIK";
                        menuMuzikDurumTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }

                    menuMuzik.Enabled = true;
                };



                menuSeslerTxt.Text = y.menuSesler_btn;

                if (oAyarlariTBL_.sesler)
                {
                    menuSeslerDurumTxt.Text = "AÇIK";
                    menuSeslerDurumTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                }
                else
                {
                    menuSeslerDurumTxt.Text = "KAPALI";
                    menuSeslerDurumTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                }

                RelativeLayout menuSesler = pcw.FindViewById<RelativeLayout>(Resource.Id.menuSesler);
                menuSesler.Click += async delegate
                {
                    menuSesler.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuSesler.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuSesler.SetBackgroundColor(r.menuButonArkaplanR);

                    if (menuSeslerDurumTxt.Text == "AÇIK")
                    {
                        await sesAyar("kapa");

                        menuSeslerDurumTxt.Text = "KAPALI";
                        menuSeslerDurumTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await sesAyar("aç");

                        menuSeslerDurumTxt.Text = "AÇIK";
                        menuSeslerDurumTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }

                    menuSesler.Enabled = true;
                };



                menuYeniliklerTxt.Text = y.menuYenilikler_btn;

                LinearLayout menuYenilikler = pcw.FindViewById<LinearLayout>(Resource.Id.menuYenilikler);
                menuYenilikler.Click += async delegate
                {
                    menuYenilikler.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuYenilikler.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuYenilikler.SetBackgroundColor(r.menuButonArkaplanR);

                    Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://www.temelfikir.com/yenilikler.html"));
                    c.StartActivity(browserIntent);

                    menuYenilikler.Enabled = true;
                };


                menuCikisTxt.Text = y.menuCikis_btn;

                LinearLayout menuCikis = pcw.FindViewById<LinearLayout>(Resource.Id.menuCikis);
                menuCikis.Click += async delegate
                {
                    menuCikis.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    menuCikis.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    menuCikis.SetBackgroundColor(r.menuButonArkaplanR);

                    popUpanaView.Visibility = ViewStates.Invisible;

                    mesajGoster(y.cikmayaEminMisiniz, () =>
                    {
                        popupWindow.Dismiss();

                        uygulamayiKapat();
                    }, true, () =>
                    {
                        popupWindow.Dismiss();
                        c.Window.SetSoftInputMode(SoftInput.StateHidden);
                        ortakAyarlar(c.Window);
                        menuPenceresiniAc();
                    }, y.cik_btn, y.cikma_btn);
                };

                if (sayfa == "bulmaca")
                {
                    menuBolumlereDonTxt.Text = y.menuBolumlereDon_btn;

                    LinearLayout menuBolumlereDon = pcw.FindViewById<LinearLayout>(Resource.Id.menuBolumlereDon);
                    menuBolumlereDon.Visibility = ViewStates.Visible;
                    menuBolumlereDon.Click += async delegate
                    {
                        menuBolumlereDon.Enabled = false;

                        sesEfektiCal("butonTiklandi");

                        menuBolumlereDon.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                        await Task.Delay(100);

                        menuBolumlereDon.SetBackgroundColor(r.menuButonArkaplanR);

                        if (!await bulmacaEkrani.geriCik())
                            menuBolumlereDon.Enabled = true;
                    };
                }

                Button menuKapatBtn = pcw.FindViewById<Button>(Resource.Id.menuKapatBtn);
                menuKapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                menuKapatBtn.Click += delegate
                {
                    menuKapatBtn.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();
                };

                popupWindow.DismissEvent += delegate
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    f.UnbindDrawables(pcw);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                };


                popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                BuPencereAcildi(popupWindow);
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static void uygulamayiKapat()
        {
            if (c != null)
                c.FinishAffinity();
            Process.KillProcess(Process.MyPid());
            System.Environment.Exit(0);
            Java.Lang.JavaSystem.Exit(1);
            return;
        }

        public static void UnbindDrawables(View view, bool runGC = true)
        {
            try
            {
                if (view.Background != null) view.Background.SetCallback(null);
                if (view is ImageView)
                {
                    var i = (ImageView)view;
                    i.SetImageBitmap(null);
                    i.SetImageDrawable(null);
                }
                else if (view is ViewGroup && !(view is AdapterView))
                {
                    var v = (ViewGroup)view;
                    for (int i = 0; i < v.ChildCount; i++)
                    {
                        UnbindDrawables(v.GetChildAt(i), false);
                    }
                    v.RemoveAllViews();
                }
                if (runGC) temizlikYap();
            }
            catch { }//kontrollü catch
        }

        public static string getDeviceName()
        {
            string manufacturer = Build.Manufacturer;
            string model = Build.Model;
            if (model.ToLower().StartsWith(manufacturer.ToLower()))
            {
                return capitalize(model);
            }
            else
            {
                return capitalize(manufacturer) + " " + model;
            }
        }


        private static string capitalize(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }
            char first = s[0];
            if (Java.Lang.Character.IsUpperCase(first))
            {
                return s;
            }
            else
            {
                return Java.Lang.Character.ToUpperCase(first) + s.Substring(1);
            }
        }
        private static string getAndroidVersion()
        {
            int sdk = Convert.ToInt32(Build.VERSION.SdkInt);
            switch (sdk)
            {
                case 1: return "(Android 1.0)";
                case 2: return "Petit Four" + "(Android 1.1)";
                case 3: return "Cupcake" + "(Android 1.5)";
                case 4: return "Donut" + "(Android 1.6)";
                case 5: return "Eclair" + "(Android 2.0)";
                case 6: return "Eclair" + "(Android 2.0.1)";
                case 7: return "Eclair" + "(Android 2.1)";
                case 8: return "Froyo" + "(Android 2.2)";
                case 9: return "Gingerbread" + "(Android 2.3)";
                case 10: return "Gingerbread" + "(Android 2.3.3)";
                case 11: return "Honeycomb" + "(Android 3.0)";
                case 12: return "Honeycomb" + "(Android 3.1)";
                case 13: return "Honeycomb" + "(Android 3.2)";
                case 14: return "Ice Cream Sandwich" + "(Android 4.0)";
                case 15: return "Ice Cream Sandwich" + "(Android 4.0.3)";
                case 16: return "Jelly Bean" + "(Android 4.1)";
                case 17: return "Jelly Bean" + "(Android 4.2)";
                case 18: return "Jelly Bean" + "(Android 4.3)";
                case 19: return "KitKat" + "(Android 4.4)";
                case 20: return "KitKat Watch" + "(Android 4.4)";
                case 21: return "Lollipop" + "(Android 5.0)";
                case 22: return "Lollipop" + "(Android 5.1)";
                case 23: return "Marshmallow" + "(Android 6.0)";
                case 24: return "Nougat" + "(Android 7.0)";
                case 25: return "Nougat" + "(Android 7.1.1)";
                case 26: return "Oreo" + "(Android 8.0)";
                case 27: return "Oreo" + "(Android 8.1)";
                case 28: return "Pie" + "(Android 9.0)";
                case 29: return "Q" + "(Android 10.0)";
                case 30: return "Android 11" + "";
                default: return "";
            }
        }

        public static async void logKaydet(string txt)
        {
            if (logRecording)
            {
                try
                {
                    txt =
                        ">>>>>>>>>>\n\n" +
                        "cihaz: " + getDeviceName() + "\n\n" +
                        "ver: " + getAndroidVersion() + "\n\n" +
                        "çözünürlük: " + ekranGenisligi + "x" + ekranYuksekligi + "\n\n" +
                        "tarih: " + DateTime.Now.ToString() + "\n\n" +
                        "google_id: " + (googleProfile_ != null ? "google_id = " + googleProfile_.Id : "boş") + "\n\n" +
                        "<<<<<<<<<< \n\n\n\n" +
                        txt;

                    Dictionary<string, string> veriler = new Dictionary<string, string>{
                        { "_l", "o"},
                        {"lg",txt }
                    };

                    var t = Task.Run(() => SendPostToURI(server_main_link, veriler));
                    await t.ConfigureAwait(false);
                }
                catch (Exception ex)//kontrollü catch
                {
                    mesajGoster("Log kaydedilirken hata oluştu. || " + ex);
                    return;
                }
            }
        }
        public static void ayarlarPenceresiniAc()
        {
            try
            {
                c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                acikPencereleriKapat();

                ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                //loading çıkarıyoruz
                ProgressBar loadingBar = new ProgressBar(c);
                loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                loadingBar.Indeterminate = true;
                RelativeLayout rl = new RelativeLayout(c);
                rl.SetGravity(GravityFlags.Center);
                rl.AddView(loadingBar);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                anaView.AddView(rl, param);


                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View ayarlar = inflater.Inflate(Resource.Layout.ayarlar, null, false);

                PopupWindow popupWindow = new PopupWindow(ayarlar, LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

                popupWindow.Focusable = false;
                popupWindow.OutsideTouchable = false;

                View pcw = popupWindow.ContentView;

                ViewGroup popUpanaView = pcw.RootView as ViewGroup;


                RelativeLayout ayarlarR1 = pcw.FindViewById<RelativeLayout>(Resource.Id.ayarlarR1);
                ayarlarR1.SetBackgroundResource(Resource.Drawable.p_arkaplan);

                c.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));


                ScrollView ayarlarScroll = pcw.FindViewById<ScrollView>(Resource.Id.ayarlarScroll);
                ayarlarScroll.SetPadding(0, 0, 0, px(200));
                var p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
                p2.SetMargins(0, px(250), 0, 0);
                p2.AddRule(LayoutRules.AlignParentBottom);
                ayarlarScroll.LayoutParameters = p2;
                ayarlarScroll.RequestLayout();

                TextView ayarlarSeslerTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarSeslerTxt);
                ayarlarSeslerTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                var p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                int _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarSeslerTxt.LayoutParameters = p;
                ayarlarSeslerTxt.RequestLayout();

                Switch ayarlarSeslerSwh = pcw.FindViewById<Switch>(Resource.Id.ayarlarSeslerSwh);
                ayarlarSeslerSwh.SetPadding(0, 0, px(40), 0);
                p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                p2.AddRule(LayoutRules.AlignParentRight);
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                ayarlarSeslerSwh.LayoutParameters = p2;
                ayarlarSeslerSwh.RequestLayout();

                TextView ayarlarMuzikTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarMuzikTxt);
                ayarlarMuzikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarMuzikTxt.LayoutParameters = p;
                ayarlarMuzikTxt.RequestLayout();

                Switch ayarlarMuzikSwh = pcw.FindViewById<Switch>(Resource.Id.ayarlarMuzikSwh);
                ayarlarMuzikSwh.SetPadding(0, 0, px(40), 0);
                p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                p2.AddRule(LayoutRules.AlignParentRight);
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                ayarlarMuzikSwh.LayoutParameters = p2;
                ayarlarMuzikSwh.RequestLayout();

                TextView ayarlarTitresimTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarTitresimTxt);
                ayarlarTitresimTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarTitresimTxt.LayoutParameters = p;
                ayarlarTitresimTxt.RequestLayout();

                Switch ayarlarTitresimSwh = pcw.FindViewById<Switch>(Resource.Id.ayarlarTitresimSwh);
                ayarlarTitresimSwh.SetPadding(0, 0, px(40), 0);
                p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                p2.AddRule(LayoutRules.AlignParentRight);
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                ayarlarTitresimSwh.LayoutParameters = p2;
                ayarlarTitresimSwh.RequestLayout();

                TextView ayarlarYanlisKelimeSilTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarYanlisKelimeSilTxt);
                ayarlarYanlisKelimeSilTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarYanlisKelimeSilTxt.LayoutParameters = p;
                ayarlarYanlisKelimeSilTxt.RequestLayout();

                Switch ayarlarYanlisKelimeSilSwh = pcw.FindViewById<Switch>(Resource.Id.ayarlarYanlisKelimeSilSwh);
                ayarlarYanlisKelimeSilSwh.SetPadding(0, 0, px(40), 0);
                p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, RelativeLayout.LayoutParams.WrapContent);
                p2.AddRule(LayoutRules.AlignParentRight);
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                ayarlarYanlisKelimeSilSwh.LayoutParameters = p2;
                ayarlarYanlisKelimeSilSwh.RequestLayout();

                ImageView ayarlarGoogleCikIco = pcw.FindViewById<ImageView>(Resource.Id.ayarlarGoogleCikIco);
                p = new LinearLayout.LayoutParams(px(60), px(60));
                p.Gravity = GravityFlags.Center;
                _ = px(30);
                p.SetMargins(80, _, _, _);
                ayarlarGoogleCikIco.LayoutParameters = p;
                ayarlarGoogleCikIco.RequestLayout();

                TextView ayarlarGoogleCikTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarGoogleCikTxt);
                ayarlarGoogleCikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarGoogleCikTxt.LayoutParameters = p;
                ayarlarGoogleCikTxt.RequestLayout();

                LinearLayout ayarlarKodGir = pcw.FindViewById<LinearLayout>(Resource.Id.ayarlarKodGir);
                ayarlarKodGir.SetPadding(px(100), px(50), px(100), px(50));

                TextView ayarlarKodGirTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarKodGirTxt);
                ayarlarKodGirTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarKodGirTxt.LayoutParameters = p;
                ayarlarKodGirTxt.RequestLayout();

                EditText ayarlarKodGirTxtBx = pcw.FindViewById<EditText>(Resource.Id.ayarlarKodGirTxtBx);
                ayarlarKodGirTxtBx.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarKodGirTxtBx.LayoutParameters = p;
                ayarlarKodGirTxtBx.RequestLayout();

                TextView ayarlarKodGirDurum = pcw.FindViewById<TextView>(Resource.Id.ayarlarKodGirDurum);
                ayarlarKodGirDurum.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, px(10), _, _);
                ayarlarKodGirDurum.LayoutParameters = p;
                ayarlarKodGirDurum.RequestLayout();

                Button ayarlarKodGirBtn = pcw.FindViewById<Button>(Resource.Id.ayarlarKodGirBtn);
                ayarlarKodGirBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(px(300), px(100));
                p.Gravity = GravityFlags.Center;
                _ = px(20);
                p.SetMargins(_, _, _, _);
                ayarlarKodGirBtn.LayoutParameters = p;
                ayarlarKodGirBtn.RequestLayout();

                Button ayarlarIlerleyis = pcw.FindViewById<Button>(Resource.Id.ayarlarIlerleyis);
                ayarlarIlerleyis.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(px(500), px(125));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(50), 0, 0);
                ayarlarIlerleyis.LayoutParameters = p;
                ayarlarIlerleyis.RequestLayout();

                Button ayarlarYardim = pcw.FindViewById<Button>(Resource.Id.ayarlarYardim);
                ayarlarYardim.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p = new LinearLayout.LayoutParams(px(500), px(125));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(50), 0, 0);
                ayarlarYardim.LayoutParameters = p;
                ayarlarYardim.RequestLayout();

                TextView ayarlarGizlilikTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarGizlilikTxt);
                ayarlarGizlilikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(50), 0, 0);
                ayarlarGizlilikTxt.LayoutParameters = p;
                ayarlarGizlilikTxt.RequestLayout();

                TextView ayarlarHizmetSartlariTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarHizmetSartlariTxt);
                ayarlarHizmetSartlariTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(20), 0, 0);
                ayarlarHizmetSartlariTxt.LayoutParameters = p;
                ayarlarHizmetSartlariTxt.RequestLayout();

                TextView ayarlarVerileriYonetTxt = pcw.FindViewById<TextView>(Resource.Id.ayarlarVerileriYonetTxt);
                ayarlarVerileriYonetTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(20), 0, 0);
                ayarlarVerileriYonetTxt.LayoutParameters = p;
                ayarlarVerileriYonetTxt.RequestLayout();

                TextView ayarlarVers = pcw.FindViewById<TextView>(Resource.Id.ayarlarVers);
                ayarlarVers.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(40));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, px(50), 0, 0);
                ayarlarVers.LayoutParameters = p;
                ayarlarVers.RequestLayout();


                ayarlarVers.Text = y.p(y.version, _version);


                ayarlarSeslerTxt.Text = y.ayarlarSesler_btn;

                ayarlarSeslerSwh.Checked = oAyarlariTBL_.sesler;

                if (ayarlarSeslerSwh.Checked)
                    ayarlarSeslerTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                else
                    ayarlarSeslerTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);

                RelativeLayout ayarlarSesler = pcw.FindViewById<RelativeLayout>(Resource.Id.ayarlarSesler);
                ayarlarSesler.Click += async delegate
                {
                    ayarlarSesler.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    ayarlarSesler.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    ayarlarSesler.SetBackgroundColor(r.pencereSeffafArkaplanR);

                    if (ayarlarSeslerSwh.Checked == true)
                    {
                        await sesAyar("kapa");

                        ayarlarSeslerSwh.Checked = false;
                        ayarlarSeslerTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await sesAyar("aç");

                        ayarlarSeslerSwh.Checked = true;
                        ayarlarSeslerTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }

                    ayarlarSesler.Enabled = true;
                };
                ayarlarSeslerSwh.Click += delegate
                {
                    ayarlarSesler.CallOnClick();
                    ayarlarSeslerSwh.Checked = !ayarlarSeslerSwh.Checked;
                };


                ayarlarMuzikTxt.Text = y.ayarlarMuzik_btn;

                ayarlarMuzikSwh.Checked = oAyarlariTBL_.muzik;

                if (ayarlarMuzikSwh.Checked)
                    ayarlarMuzikTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                else
                    ayarlarMuzikTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);

                RelativeLayout ayarlarMuzik = pcw.FindViewById<RelativeLayout>(Resource.Id.ayarlarMuzik);
                ayarlarMuzik.Click += async delegate
                {
                    ayarlarMuzik.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    ayarlarMuzik.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    ayarlarMuzik.SetBackgroundColor(r.pencereSeffafArkaplanR);

                    if (ayarlarMuzikSwh.Checked == true)
                    {
                        await muzikAyar("kapa");

                        ayarlarMuzikSwh.Checked = false;
                        ayarlarMuzikTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await muzikAyar("aç");

                        ayarlarMuzikSwh.Checked = true;
                        ayarlarMuzikTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }

                    ayarlarMuzik.Enabled = true;
                };
                ayarlarMuzikSwh.Click += delegate
                {
                    ayarlarMuzik.CallOnClick();
                    ayarlarMuzikSwh.Checked = !ayarlarMuzikSwh.Checked;
                };

                ayarlarTitresimTxt.Text = y.ayarlarTitresim_btn;

                ayarlarTitresimSwh.Checked = oAyarlariTBL_.titresim;

                if (ayarlarTitresimSwh.Checked)
                    ayarlarTitresimTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                else
                    ayarlarTitresimTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);

                RelativeLayout ayarlarTitresim = pcw.FindViewById<RelativeLayout>(Resource.Id.ayarlarTitresim);
                ayarlarTitresim.Click += async delegate
                {
                    ayarlarTitresim.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    ayarlarTitresim.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    ayarlarTitresim.SetBackgroundColor(r.pencereSeffafArkaplanR);

                    if (ayarlarTitresimSwh.Checked == true)
                    {
                        await titresimAyar("kapa");

                        ayarlarTitresimSwh.Checked = false;
                        ayarlarTitresimTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await titresimAyar("aç");

                        ayarlarTitresimSwh.Checked = true;
                        ayarlarTitresimTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }

                    ayarlarTitresim.Enabled = true;
                };
                ayarlarTitresimSwh.Click += delegate
                {
                    ayarlarTitresim.CallOnClick();
                    ayarlarTitresimSwh.Checked = !ayarlarTitresimSwh.Checked;
                };


                ayarlarYanlisKelimeSilTxt.Text = y.ayarlarYanlisKelimeSil_btn;

                ayarlarYanlisKelimeSilSwh.Checked = oAyarlariTBL_.yanlisKelimeSil;

                if (ayarlarYanlisKelimeSilSwh.Checked)
                    ayarlarYanlisKelimeSilTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                else
                    ayarlarYanlisKelimeSilTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);

                RelativeLayout ayarlarYanlisKelimeSil = pcw.FindViewById<RelativeLayout>(Resource.Id.ayarlarYanlisKelimeSil);
                ayarlarYanlisKelimeSil.Click += async delegate
                {
                    ayarlarYanlisKelimeSil.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    ayarlarYanlisKelimeSil.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    ayarlarYanlisKelimeSil.SetBackgroundColor(r.pencereSeffafArkaplanR);

                    if (ayarlarYanlisKelimeSilSwh.Checked == true)
                    {
                        await yanlisKelimeSilAyar("kapa");

                        ayarlarYanlisKelimeSilSwh.Checked = false;
                        ayarlarYanlisKelimeSilTxt.SetTextColor(r.menuSesMuzikKapaliYaziR);
                    }
                    else
                    {
                        await yanlisKelimeSilAyar("aç");

                        ayarlarYanlisKelimeSilSwh.Checked = true;
                        ayarlarYanlisKelimeSilTxt.SetTextColor(r.menuSesMuzikAcikYaziR);
                    }


                    ayarlarYanlisKelimeSil.Enabled = true;
                };
                ayarlarYanlisKelimeSilSwh.Click += delegate
                {
                    ayarlarYanlisKelimeSil.CallOnClick();
                    ayarlarYanlisKelimeSilSwh.Checked = !ayarlarYanlisKelimeSilSwh.Checked;
                };



                ayarlarGoogleCikTxt.Text = y.ayarlarGoogleOturumunuKapat_btn;

                LinearLayout ayarlarGoogleCik = pcw.FindViewById<LinearLayout>(Resource.Id.ayarlarGoogleCik);
                ayarlarGoogleCik.Click += async delegate
                {
                    ayarlarGoogleCik.Enabled = false;

                    ayarlarGoogleCik.SetBackgroundColor(r.menuButonTiklandiArkaplanR);

                    await Task.Delay(100);

                    ayarlarGoogleCik.SetBackgroundColor(r.pencereSeffafArkaplanR);

                    popUpanaView.Visibility = ViewStates.Invisible;

                    mesajGoster(y.ayarlarGoogledanCikilsinMi, () =>
                    {
                        _PlayGames.mGoogleSignInClient.SignOut();
                        _PlayGames.mGoogleSignInAccount = null;

                        popupWindow.Dismiss();

                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);
                        c.Window.ClearFlags(WindowManagerFlags.NotTouchable);


                        ekranGecisi(typeof(anaEkran));
                        c.Finish();
                    }, true, () =>
                    {
                        popupWindow.Dismiss();
                        c.Window.SetSoftInputMode(SoftInput.StateHidden);
                        ortakAyarlar(c.Window);
                        ayarlarPenceresiniAc();
                    }, y.evet_btn, y.hayir_btn);
                };


                c.Window.SetSoftInputMode(SoftInput.AdjustResize);
                popupWindow.SoftInputMode = SoftInput.AdjustPan;
                popupWindow.InputMethodMode = InputMethod.Needed;

                popupWindow.Focusable = true;

                ayarlarKodGirTxt.Text = y.ayarlarKodGirTxt;



                ayarlarKodGirTxtBx.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(20) });
                ayarlarKodGirTxtBx.SetMaxLines(1);
                ayarlarKodGirTxtBx.SetTypeface(ayarlarKodGirTxtBx.Typeface, TypefaceStyle.Bold);
                ayarlarKodGirTxtBx.SetSingleLine(true);



                ayarlarKodGirBtn.Text = y.ayarlarKullan_btn;
                ayarlarKodGirBtn.Click += async delegate
                {
                    ayarlarKodGirBtn.Enabled = false;
                    sesEfektiCal("butonTiklandi");

                    ayarlarKodGirDurum.Text = y.ayarlarYukleniyor;
                    ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirYaziR);

                    ayarlarKodGirTxtBx.Enabled = false;

                    ayarlarKodGirDurum.Visibility = ViewStates.Visible;
                    if (ayarlarKodGirTxtBx.Text != "")
                    {
                        var t4 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=kd&_g=" + googleProfile_.Id + "&kd=" + ayarlarKodGirTxtBx.Text));
                        await t4.ConfigureAwait(true);
                        if (t4.Result != null)
                        {
                            if (t4.Result == "1")
                            {
                                //Jeton bilgileri çekiliyor
                                var t5 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jt&_g=" + googleProfile_.Id));
                                await t5.ConfigureAwait(true);
                                if (t5.Result != null) jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                                else
                                {
                                    anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                    return;
                                }

                                TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                                ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";

                                ayarlarKodGirDurum.Text = y.ayarlarKodGirBasarili;
                                ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirBasariliYaziR);
                            }
                            else if (t4.Result == "2")
                            {
                                ayarlarKodGirDurum.Text = y.ayarlarKodGirKullanilmis;
                                ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirKullanilmisYaziR);
                            }
                            else if (t4.Result == "3")
                            {
                                ayarlarKodGirDurum.Text = y.ayarlarKodGirArtikGecersiz;
                                ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirArtikGecersizYaziR);
                            }
                            else
                            {
                                ayarlarKodGirDurum.Text = y.ayarlarKodGirGecersiz;
                                ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirGecersizYaziR);
                            }
                        }
                        else
                        {
                            anaEkranaDonVeMesajGoster(y.ayarlarKodGirHata);
                            return;
                        }
                    }
                    else
                    {
                        ayarlarKodGirDurum.Text = y.ayarlarKodGiriniz;
                        ayarlarKodGirDurum.SetTextColor(r.ayarlarKodGirYaziR);
                    }

                    ayarlarKodGirTxtBx.Enabled = true;
                    ayarlarKodGirBtn.Enabled = true;
                };


                ayarlarIlerleyis.Text = y.ayarlarIlerleyisiSifirla_btn;
                ayarlarIlerleyis.Click += delegate
                {
                    ayarlarIlerleyis.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popUpanaView.Visibility = ViewStates.Invisible;

                    mesajGoster(y.ayarlarIlerleyisSifirlanacak, async () =>
                    {
                        //sıfırlama kodları
                        var t6 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=is&_g=" + googleProfile_.Id));
                        await t6.ConfigureAwait(false);
                        if (t6.Result != "ok")
                        {
                            //sıfırlama başarısızsa
                            anaEkranaDonVeMesajGoster(y.ayarlarIlerleyisSifirlamaHatasi);
                            return;
                        }

                        ayarlarTBL_ = null;
                        googleProfile_ = null;
                        bolumlerTBL_ = null;
                        sorularTBL_ = null;
                        jetonlarTBL_ = null;
                        soruIstatistikTBL_ = null;
                        soruIstatistikIDleriTBL_ = null;
                        oAyarlariTBL_ = null;
                        basarimlarTBL_ = null;
                        basarimKontrolTBL_ = null;

                        f.bakimAktif = true;

                        c.RunOnUiThread(delegate
                        {
                            c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        });

                        await Task.Delay(4000);

                        c.RunOnUiThread(delegate
                        {
                            popupWindow.Dismiss();
                        });


                        anaEkranaDonVeMesajGoster(y.ayarlarIlerleyisSifirlamaBasarili);
                    }, true, () =>
                    {
                        popupWindow.Dismiss();
                        c.Window.SetSoftInputMode(SoftInput.StateHidden);
                        ortakAyarlar(c.Window);
                        ayarlarPenceresiniAc();
                    }, y.sifirla_btn, y.vazgec_btn);
                };



                ayarlarYardim.Text = y.ayarlarYardim_btn;
                ayarlarYardim.Click += delegate
                {
                    ayarlarYardim.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://www.temelfikir.com/yardim.html"));
                    c.StartActivity(browserIntent);

                    ayarlarYardim.Enabled = true;
                };


                ayarlarGizlilikTxt.Text = y.ayarlarGizlilik_btn;
                ayarlarGizlilikTxt.SetTypeface(ayarlarGizlilikTxt.Typeface, TypefaceStyle.Bold);
                ayarlarGizlilikTxt.Click += delegate
                {
                    ayarlarGizlilikTxt.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://www.temelfikir.com/gizlilik-politikasi.html"));
                    c.StartActivity(browserIntent);

                    ayarlarGizlilikTxt.Enabled = true;
                };

                ayarlarHizmetSartlariTxt.Text = y.ayarlarHizmet_btn;
                ayarlarHizmetSartlariTxt.SetTypeface(ayarlarHizmetSartlariTxt.Typeface, TypefaceStyle.Bold);
                ayarlarHizmetSartlariTxt.Click += delegate
                {
                    ayarlarHizmetSartlariTxt.Enabled = false;

                    Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://www.temelfikir.com/hizmet-kosullari.html"));
                    c.StartActivity(browserIntent);

                    ayarlarHizmetSartlariTxt.Enabled = true;
                };

                ayarlarVerileriYonetTxt.Text = y.ayarlarVerileriYonet_btn;
                ayarlarVerileriYonetTxt.SetTypeface(ayarlarVerileriYonetTxt.Typeface, TypefaceStyle.Bold);
                ayarlarVerileriYonetTxt.Click += delegate
                {
                    ayarlarVerileriYonetTxt.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);

                    //Verileri Yönet penceresi
                    verileriYonetPenceresiniAc();
                };


                Button ayarlarKapatBtn = pcw.FindViewById<Button>(Resource.Id.ayarlarKapatBtn);
                ayarlarKapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                ayarlarKapatBtn.Click += delegate
                {
                    ayarlarKapatBtn.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();
                };


                popupWindow.DismissEvent += (object s, EventArgs e) =>
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    f.UnbindDrawables(pcw);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    c.Window.SetSoftInputMode(SoftInput.StateHidden);
                    ortakAyarlar(c.Window);
                };


                popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                BuPencereAcildi(popupWindow);
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static void profilPenceresiniAc()
        {
            try
            {
                c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                acikPencereleriKapat();

                ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                //loading çıkarıyoruz
                ProgressBar loadingBar = new ProgressBar(c);
                loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                loadingBar.Indeterminate = true;
                RelativeLayout rl = new RelativeLayout(c);
                rl.SetGravity(GravityFlags.Center);
                rl.AddView(loadingBar);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                anaView.AddView(rl, param);


                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View profil = inflater.Inflate(Resource.Layout.profil, null, false);

                PopupWindow popupWindow = new PopupWindow(profil, LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

                popupWindow.Focusable = false;
                popupWindow.OutsideTouchable = false;

                View pcw = popupWindow.ContentView;


                RelativeLayout menuR1 = pcw.FindViewById<RelativeLayout>(Resource.Id.profilR1);
                menuR1.SetBackgroundResource(Resource.Drawable.p_arkaplan);

                c.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

                ViewGroup popUpanaView = pcw.RootView as ViewGroup;


                LinearLayout profilL2 = pcw.FindViewById<LinearLayout>(Resource.Id.profilL2);
                var p3 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
                p3.SetMargins(0, px(125), 0, 0);
                profilL2.LayoutParameters = p3;
                profilL2.RequestLayout();

                ImageView profil_PR = pcw.FindViewById<ImageView>(Resource.Id.profil_PR);
                var p2 = new LinearLayout.LayoutParams(px(180), px(180));
                p2.Gravity = GravityFlags.Center;
                profil_PR.LayoutParameters = p2;
                profil_PR.RequestLayout();

                TextView profil_kAdi = pcw.FindViewById<TextView>(Resource.Id.profil_kAdi);
                profil_kAdi.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                int _ = px(20);
                p2.SetMargins(_, _, _, _);
                profil_kAdi.LayoutParameters = p2;
                profil_kAdi.RequestLayout();

                TextView profil_basarimlarTxt = pcw.FindViewById<TextView>(Resource.Id.profil_basarimlarTxt);
                profil_basarimlarTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(60));
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                p2.Gravity = GravityFlags.Center;
                _ = px(20);
                p2.SetMargins(_, _, _, _);
                profil_basarimlarTxt.LayoutParameters = p2;
                profil_basarimlarTxt.RequestLayout();

                ScrollView profilScroll = pcw.FindViewById<ScrollView>(Resource.Id.profilScroll);
                p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p2.SetMargins(0, 0, 0, px(275));
                profilScroll.LayoutParameters = p2;
                profilScroll.RequestLayout();


                try
                {
                    Bitmap r = profilGorseli_;

                    int borderSize = px(5);

                    Bitmap bmpWithBorder = Bitmap.CreateBitmap(r.Width + borderSize * 2, r.Height + borderSize * 2, r.GetConfig());
                    Canvas canvas = new Canvas(bmpWithBorder);
                    canvas.DrawColor(Color.Gray);
                    canvas.DrawBitmap(r, borderSize, borderSize, null);

                    //çift border gri ve beyaz

                    Bitmap bmpWithBorder2 = Bitmap.CreateBitmap(bmpWithBorder.Width + borderSize * 2, bmpWithBorder.Height + borderSize * 2, bmpWithBorder.GetConfig());
                    Canvas canvas2 = new Canvas(bmpWithBorder2);
                    canvas2.DrawColor(Color.White);
                    canvas2.DrawBitmap(bmpWithBorder, borderSize, borderSize, null);

                    profil_PR.SetImageBitmap(bmpWithBorder2);

                    bmpWithBorder = null;
                    r = null;
                }
                catch (Exception ex)
                {
                    string e = y.p(y.hataOlustu, ex);
                    if (logRecording)
                    {
                        main_log += e + "\n\n";
                    }
                }


                profil_kAdi.Text = googleProfile_.DisplayName;

                TextView profil_jetonlarTxt = pcw.FindViewById<TextView>(Resource.Id.profil_basarimlarTxt);
                profil_jetonlarTxt.Text = y.profilBasarimlar;
                profil_jetonlarTxt.SetTypeface(profil_jetonlarTxt.Typeface, TypefaceStyle.Bold);



                LinearLayout profil_basarimlarL = pcw.FindViewById<LinearLayout>(Resource.Id.profil_basarimlarL);



                for (int i = 0; i < basarimlarTBL_.Length; i++)
                {
                    LinearLayout basarim = new LinearLayout(c);
                    basarim.Tag = "profil_basarimL" + i;
                    basarim.Orientation = Android.Widget.Orientation.Horizontal;
                    basarim.SetBackgroundColor(r.basarimKutusuArkaplanR);


                    var p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    p.SetMargins(0, px(20), 0, px(20));
                    profil_basarimlarL.AddView(basarim, p);


                    LinearLayout basarimSol = new LinearLayout(c);
                    basarimSol.Orientation = Android.Widget.Orientation.Vertical;
                    basarimSol.SetBackgroundColor(r.basarimKutusuSolBolumArkaplanR);

                    p = new LinearLayout.LayoutParams(px(200), ViewGroup.LayoutParams.MatchParent);
                    basarim.AddView(basarimSol, p);


                    ImageView basarimGorseli = new ImageView(c);
                    basarimGorseli.Tag = "profil_basarimGorseli" + i;
                    basarimGorseli.ContentDescription = "";
                    basarimGorseli.Focusable = false;
                    basarimGorseli.SetBackgroundResource(Resource.Drawable.p_googleIco);
                    basarimGorseli.SetPadding(px(60), 0, px(60), 0);

                    p = new LinearLayout.LayoutParams(px(120), px(120));
                    p.SetMargins(0, px(30), 0, px(20));
                    p.Gravity = GravityFlags.Center;
                    basarimSol.AddView(basarimGorseli, p);




                    LinearLayout basarimOdulMiktariL = new LinearLayout(c);
                    basarimOdulMiktariL.Orientation = Android.Widget.Orientation.Horizontal;


                    TextView basarimOdulMiktari = new TextView(c);
                    basarimOdulMiktari.Tag = "profil_basarimOdulMiktari" + i;
                    basarimOdulMiktari.Text = basarimlarTBL_[i].odul + "";
                    basarimOdulMiktari.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(48));
                    basarimOdulMiktari.SetTextColor(r.basarimKutusuYaziR);
                    basarimOdulMiktari.SetTypeface(basarimOdulMiktari.Typeface, TypefaceStyle.Bold);
                    basarimOdulMiktari.TextAlignment = TextAlignment.Center;

                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    p.SetMargins(0, 0, px(10), 0);
                    p.Gravity = GravityFlags.Center;
                    basarimOdulMiktariL.AddView(basarimOdulMiktari, p);


                    ImageView basarimOdulMiktariJetonGorseli = new ImageView(c);
                    basarimOdulMiktariJetonGorseli.ContentDescription = "";
                    basarimOdulMiktariJetonGorseli.Focusable = false;
                    basarimOdulMiktariJetonGorseli.SetBackgroundResource(Resource.Drawable.jeton);

                    p = new LinearLayout.LayoutParams(px(40), px(40));
                    p.SetMargins(0, px(3), 0, 0);
                    p.Gravity = GravityFlags.Center;
                    basarimOdulMiktariL.AddView(basarimOdulMiktariJetonGorseli, p);


                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
                    p.Gravity = GravityFlags.Center;
                    basarimSol.AddView(basarimOdulMiktariL, p);


                    bool _odulAlindi = false;

                    //ödül alındıysa rengini değiştiriyoruz
                    if (basarimKontrolTBL_ != null)
                    {
                        for (int i2 = 0; i2 < basarimKontrolTBL_.Length; i2++)
                        {
                            if (basarimKontrolTBL_[i2].basarimID == basarimlarTBL_[i].id)
                            {
                                if (basarimKontrolTBL_[i2].odulAlindi)
                                {
                                    _odulAlindi = true;
                                    basarim.SetBackgroundColor(r.basarimKutusuBasarimAlindiArkaplanR);
                                    basarimSol.SetBackgroundColor(r.basarimKutusuArkaplanR);
                                }
                            }
                        }
                    }


                    LinearLayout basarimSag = new LinearLayout(c);
                    basarimSag.Orientation = Android.Widget.Orientation.Vertical;

                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    basarim.AddView(basarimSag, p);


                    TextView basarimAdi = new TextView(c);
                    basarimAdi.Tag = "profil_basarimAdi" + i;
                    basarimAdi.Text = basarimlarTBL_[i].baslik;
                    basarimAdi.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                    basarimAdi.SetTextColor(r.basarimKutusuYaziR);
                    basarimAdi.SetTypeface(basarimAdi.Typeface, TypefaceStyle.Bold);
                    basarimAdi.TextAlignment = TextAlignment.Center;

                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    p.SetMargins(px(20), px(30), px(20), 0);
                    p.Gravity = GravityFlags.Center;
                    basarimSag.AddView(basarimAdi, p);


                    TextView basarimAciklamasi = new TextView(c);
                    basarimAciklamasi.Tag = "profil_basarimAciklamasi" + i;
                    basarimAciklamasi.Text = basarimlarTBL_[i].aciklama;
                    basarimAciklamasi.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                    basarimAciklamasi.SetTextColor(r.basarimKutusuYaziR);
                    basarimAciklamasi.TextAlignment = TextAlignment.Center;

                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    p.SetMargins(px(20), px(5), px(20), 0);
                    p.Gravity = GravityFlags.Center;
                    basarimSag.AddView(basarimAciklamasi, p);


                    TextView basarimDurumu = new TextView(c);
                    basarimDurumu.Tag = "profil_basarimDurumu" + i;
                    basarimDurumu.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                    basarimDurumu.SetTextColor(r.basarimKutusuYaziR);
                    basarimDurumu.TextAlignment = TextAlignment.Center;
                    basarimDurumu.SetTypeface(basarimDurumu.Typeface, TypefaceStyle.Bold);

                    if (_odulAlindi)
                    {
                        basarimDurumu.Text = y.profilBasarimAlindi;
                    }
                    else if (toplamGecilenBolumSayisi >= basarimlarTBL_[i].hedefDeger)
                    {
                        basarimDurumu.Text = y.profilBasarimTamamlandi;
                        basarim.SetBackgroundColor(r.basarimKutusuTmmlandiArkaplanR);
                    }
                    else
                        basarimDurumu.Text += "(" + toplamGecilenBolumSayisi + " / " + basarimlarTBL_[i].hedefDeger + ")";
                    
                    p = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    p.SetMargins(px(20), px(30), px(20), px(30));
                    p.Gravity = GravityFlags.Center;
                    basarimSag.AddView(basarimDurumu, p);


                    if (!_odulAlindi)
                    {
                        basarim.Click += async delegate
                        {
                            sesEfektiCal("butonTiklandi");

                            int bid = Convert.ToInt32(basarim.Tag.ToString().Replace("profil_basarimL", ""));
                            if (toplamGecilenBolumSayisi >= basarimlarTBL_[bid].hedefDeger)
                            {
                                bool odulAlinmadi = true;

                                if (basarimKontrolTBL_ != null)
                                {
                                    for (int i2 = 0; i2 < basarimKontrolTBL_.Length; i2++)
                                    {
                                        if (basarimKontrolTBL_[i2].basarimID == basarimlarTBL_[bid].id)
                                        {
                                            if (basarimKontrolTBL_[i2].odulAlindi)
                                            {
                                                odulAlinmadi = false;
                                            }
                                        }
                                    }
                                }

                                //ödül daha önce alınmadıysa ödülü alıyor
                                if (odulAlinmadi)
                                {
                                    var t7 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=boa&_g=" + googleProfile_.Id + "&bid=" + basarimlarTBL_[bid].id));
                                    await t7.ConfigureAwait(true);
                                    if (t7.Result == "ok")
                                    {
                                        sesEfektiCal("basarimAcildi");


                                        jetonlarTBL_.jetonSayisi = jetonlarTBL_.jetonSayisi + basarimlarTBL_[bid].odul;

                                        TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                                        ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";

                                        basarimDurumu.Text = y.profilBasarimAlindi;
                                        basarim.SetBackgroundColor(r.basarimKutusuBasarimAlindiArkaplanR);
                                        basarimSol.SetBackgroundColor(r.basarimKutusuArkaplanR);

                                        //başarımları kontrol ediyoruz
                                        bool basarimKontrolR = await f.basarimKontrol();
                                        f.basarimKontrolUI(basarimKontrolR);
                                    }
                                    else
                                    {
                                        anaEkranaDonVeMesajGoster(y.profilBasarimAlinamadi);
                                        return;
                                    }
                                }


                            }
                        };
                    }

                }


                Button profilKapatBtn = pcw.FindViewById<Button>(Resource.Id.profilKapatBtn);
                profilKapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                profilKapatBtn.Click += delegate
                {
                    sesEfektiCal("butonTiklandi");

                    profil_PR.Dispose();

                    popupWindow.Dismiss();
                };


                Button profil_achiBtn = pcw.FindViewById<Button>(Resource.Id.profil_achiBtn);
                profil_achiBtn.SetBackgroundResource(Resource.Drawable.p_achi);//X
                profil_achiBtn.Click += delegate
                {
                    sesEfektiCal("butonTiklandi");

                    f.PlayGames__.ShowAchievements();
                };

                Button profil_leaderBtn = pcw.FindViewById<Button>(Resource.Id.profil_leaderBtn);
                profil_leaderBtn.SetBackgroundResource(Resource.Drawable.p_leader);//X
                profil_leaderBtn.Click += delegate
                {
                    sesEfektiCal("butonTiklandi");

                    f.PlayGames__.ShowLeaderboards();
                };


                popupWindow.DismissEvent += delegate
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    f.UnbindDrawables(pcw);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                };


                popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                BuPencereAcildi(popupWindow);
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static void ustMenuOlustur(string sayfa, int suAnkiSayfa = 0, string bolumAdi = null)
        {
            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

            RelativeLayout ustMenu = null;

            if (sayfa == "bolumler")
            {
                ustMenu = anaView.FindViewById(Resource.Id.ustMenu_bolumler) as RelativeLayout;
            }
            else if (sayfa == "bulmaca")
            {
                ustMenu = anaView.FindViewById(Resource.Id.ustMenu_bulmacaEkrani) as RelativeLayout;
            }

            ustMenu.RemoveAllViews();

            FrameLayout pg_bildirimFL = new FrameLayout(c);
            pg_bildirimFL.Tag = "pg_bildirimFL";
            var p2 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            p2.AddRule(LayoutRules.AlignParentLeft);
            p2.AddRule(LayoutRules.CenterVertical);
            p2.SetMargins(px(25), 0, 0, 0);
            ustMenu.AddView(pg_bildirimFL, p2);


            ImageView profilGorseli = new ImageView(c);
            profilGorseli.Tag = "profilGorseli";
            profilGorseli.ContentDescription = "";
            profilGorseli.Focusable = false;
            profilGorseli.SetScaleType(ImageView.ScaleType.FitXy);
            profilGorseli.SoundEffectsEnabled = false;
            var p3 = new FrameLayout.LayoutParams(px(100), px(100));

            try
            {
                profilGorseli.SetImageBitmap(profilGorseli_);
            }
            catch (Exception ex)
            {
                if (logRecording)
                {
                    string e = y.p(y.hataOlustu, ex);
                    main_log += e + "\n\n";
                }
            }

            profilGorseli.Click += delegate
            {
                sesEfektiCal("butonTiklandi");

                profilGorseli.Enabled = false;

                profilPenceresiniAc();

                profilGorseli.Enabled = true;
            };


            pg_bildirimFL.AddView(profilGorseli, p3);



            ImageView pg_bildirimOverlay = new ImageView(c);
            pg_bildirimOverlay.Tag = "pg_bildirimOverlay";
            pg_bildirimOverlay.ContentDescription = "";
            pg_bildirimOverlay.SetScaleType(ImageView.ScaleType.FitXy);
            pg_bildirimOverlay.Visibility = ViewStates.Invisible;
            pg_bildirimOverlay.Focusable = false;
            pg_bildirimOverlay.SetBackgroundResource(Resource.Drawable.pg_bildirim);
            pg_bildirimOverlay.SoundEffectsEnabled = false;

            var p4 = new FrameLayout.LayoutParams(px(100), px(100));

            pg_bildirimOverlay.Click += delegate
            {
                sesEfektiCal("butonTiklandi");

                pg_bildirimOverlay.Enabled = false;

                profilPenceresiniAc();

                pg_bildirimOverlay.Enabled = true;
            };

            pg_bildirimFL.AddView(pg_bildirimOverlay, p4);



            LinearLayout baslikVeJetonL = new LinearLayout(c);
            baslikVeJetonL.Orientation = Android.Widget.Orientation.Horizontal;
            var p5 = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            p5.AddRule(LayoutRules.CenterInParent);
            p5.AddRule(LayoutRules.CenterVertical);
            ustMenu.AddView(baslikVeJetonL, p5);



            TextView ustBaslikTxt = new TextView(c);
            ustBaslikTxt.Tag = "ustBaslikTxt";
            ustBaslikTxt.TextAlignment = TextAlignment.Gravity;
            ustBaslikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
            ustBaslikTxt.Gravity = GravityFlags.Center;
            var p6 = new LinearLayout.LayoutParams(px(500), ViewGroup.LayoutParams.MatchParent);
            p6.Gravity = GravityFlags.Center;

            if (sayfa == "bolumler")
            {
                ustBaslikTxt.Text = y.p(y.ustBaslikBolumYazisi, suAnkiSayfa, bolumAdi);
            }
            else if (sayfa == "bulmaca")
            {
                ustBaslikTxt.Text = y.p(y.ustBaslikBulmacaYazisi, bolumAdi, suAnkiSayfa);
            }

            baslikVeJetonL.AddView(ustBaslikTxt, p6);




            LinearLayout ustJetonL = new LinearLayout(c);
            ustJetonL.Orientation = Android.Widget.Orientation.Horizontal;
            ustJetonL.Focusable = false;
            var p7 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            p7.SetMargins(0, 0, px(50), 0);

            ustJetonL.Click += delegate
            {
                sesEfektiCal("butonTiklandi");

                ustJetonL.Enabled = false;

                magazaPenceresiniAc();

                ustJetonL.Enabled = true;
            };

            baslikVeJetonL.AddView(ustJetonL, p7);


            TextView ustJetonTxt = new TextView(c);
            ustJetonTxt.Tag = "ustJetonTxt";
            ustJetonTxt.TextAlignment = TextAlignment.Gravity;
            ustJetonTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(48));
            ustJetonTxt.SetTypeface(ustJetonTxt.Typeface, TypefaceStyle.Bold);
            ustJetonTxt.Gravity = GravityFlags.Center;
            var p8 = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);
            p8.SetMargins(0, 0, px(10), 0);
            p8.Gravity = GravityFlags.Center;

            ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";


            ustJetonL.AddView(ustJetonTxt, p8);



            ImageView ustJetonGorsel = new ImageView(c);
            ustJetonGorsel.Tag = "ustJetonGorsel";
            ustJetonGorsel.ContentDescription = "";
            ustJetonGorsel.SetScaleType(ImageView.ScaleType.FitXy);
            ustJetonGorsel.SetBackgroundResource(Resource.Drawable.jeton);
            var p9 = new LinearLayout.LayoutParams(px(40), px(40));
            p9.SetMargins(0, px(3), 0, 0);
            p9.Gravity = GravityFlags.Center;
            ustJetonL.AddView(ustJetonGorsel, p9);



            Button menuBtn = new Button(c);
            menuBtn.Tag = "menuBtn";
            menuBtn.TextAlignment = TextAlignment.Gravity;
            menuBtn.Gravity = GravityFlags.Center;
            menuBtn.SetBackgroundResource(Resource.Drawable.btn_menu);
            var p10 = new RelativeLayout.LayoutParams(px(100), px(100));
            p10.AddRule(LayoutRules.AlignParentRight);
            p10.AddRule(LayoutRules.CenterVertical);
            p10.SetMargins(0, 0, px(25), 0);

            menuBtn.Click += (sender, args) =>
            {
                sesEfektiCal("butonTiklandi");

                menuBtn.Enabled = false;

                menuPenceresiniAc(sayfa);

                menuBtn.Enabled = true;
            };

            ustMenu.AddView(menuBtn, p10);
        }

        public static void verileriYonetPenceresiniAc()
        {
            try
            {
                c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                acikPencereleriKapat();

                ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                //loading çıkarıyoruz
                ProgressBar loadingBar = new ProgressBar(c);
                loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                loadingBar.Indeterminate = true;
                RelativeLayout rl = new RelativeLayout(c);
                rl.SetGravity(GravityFlags.Center);
                rl.AddView(loadingBar);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                anaView.AddView(rl, param);


                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View verileriYonetL = inflater.Inflate(Resource.Layout.verileriYonet, null, false);

                int width = LinearLayout.LayoutParams.WrapContent;
                int height = LinearLayout.LayoutParams.WrapContent;
                PopupWindow popupWindow = new PopupWindow(verileriYonetL, width, height);

                popupWindow.Focusable = false;
                popupWindow.OutsideTouchable = false;

                View pcw = popupWindow.ContentView;

                ViewGroup popUpanaView = pcw.RootView as ViewGroup;


                TextView verileriYonetTxt = pcw.FindViewById<TextView>(Resource.Id.verileriYonetTxt);
                verileriYonetTxt.Text = y.verileriYonetYazisi;


                Button verileriYonetKapatBtn = pcw.FindViewById<Button>(Resource.Id.verileriYonetKapatBtn);
                verileriYonetKapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                verileriYonetKapatBtn.Click += delegate
                {
                    verileriYonetKapatBtn.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();
                    ayarlarPenceresiniAc();
                };

                Button verileriYonetIptalBtn = pcw.FindViewById<Button>(Resource.Id.verileriYonetIptalBtn);
                verileriYonetIptalBtn.Text = y.iptal_btn;
                verileriYonetIptalBtn.Click += delegate
                {
                    verileriYonetIptalBtn.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();
                    ayarlarPenceresiniAc();
                };

                Button verileriYonetSilBtn = pcw.FindViewById<Button>(Resource.Id.verileriYonetSilBtn);
                verileriYonetSilBtn.Text = y.sil_btn;
                verileriYonetSilBtn.Click += delegate
                {
                    verileriYonetSilBtn.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    mesajGoster(y.herSeySilinecek, async () =>
                    {
                        //BURADA İLERLEYİŞİ SIFIRLA İŞLEMİNİN AYNISINI YAPIYORUZ ÇÜNKÜ O DA ZATEN HER ŞEYİ SİLİYOR
                        var t6 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=is&_g=" + googleProfile_.Id));
                        await t6.ConfigureAwait(false);
                        if (t6.Result != "ok")
                        {
                            //sıfırlama başarısızsa
                            anaEkranaDonVeMesajGoster(y.herSeySilinecekHata);
                            return;
                        }

                        ayarlarTBL_ = null;
                        googleProfile_ = null;
                        bolumlerTBL_ = null;
                        sorularTBL_ = null;
                        jetonlarTBL_ = null;
                        soruIstatistikTBL_ = null;
                        soruIstatistikIDleriTBL_ = null;
                        oAyarlariTBL_ = null;
                        basarimlarTBL_ = null;
                        basarimKontrolTBL_ = null;

                        f.bakimAktif = true;
                        
                        c.RunOnUiThread(delegate
                        {
                            c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        });

                        await Task.Delay(4000);

                        //googledan da çıkış yaptırıyoruz
                        _PlayGames.mGoogleSignInClient.SignOut();
                        _PlayGames.mGoogleSignInAccount = null;

                        c.RunOnUiThread(delegate
                        {
                            popupWindow.Dismiss();
                        });

                        anaEkranaDonVeMesajGoster(y.herSeySilinecekBasarili);

                        return;
                    }, true, () =>
                    {
                        popupWindow.Dismiss();
                        ayarlarPenceresiniAc();
                    }, y.sil_btn, y.vazgec_btn);
                };


                TextView verileriYonetSilTxt = pcw.FindViewById<TextView>(Resource.Id.verileriYonetSilTxt);
                verileriYonetSilTxt.Text = y.kisiselVerileriSil_btn;
                verileriYonetSilTxt.Click += delegate
                {
                    verileriYonetSilTxt.Enabled = false;

                    sesEfektiCal("butonTiklandi");

                    verileriYonetSilTxt.Enabled = false;
                    verileriYonetSilTxt.Visibility = ViewStates.Gone;

                    verileriYonetTxt.Text = y.verileriYonetSiliniyor;

                    verileriYonetSilBtn.Visibility = ViewStates.Visible;
                    verileriYonetIptalBtn.Visibility = ViewStates.Visible;

                    verileriYonetSilTxt.Enabled = true;
                };


                popupWindow.DismissEvent += delegate
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    f.UnbindDrawables(pcw);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    c.Window.SetSoftInputMode(SoftInput.StateHidden);
                    ortakAyarlar(c.Window);
                };


                popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                BuPencereAcildi(popupWindow);
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static async Task<bool> basarimKontrol()
        {
            bool herhangiTamamlananVarMi = false;

            //başarım kontrol tablosunu çekiyoruz
            var t9 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=bkc&_g=" + googleProfile_.Id));
            await t9.ConfigureAwait(false);
            if (t9.Result != null) basarimKontrolTBL_ = JsonConvert.DeserializeObject<basarimKontrolTBL[]>(t9.Result);
            else
            {
                f.anaEkranaDonVeMesajGoster(y.basarimlarYuklenemedi);
                return false;
            }

            if (basarimKontrolTBL_ == null)
            {
                f.anaEkranaDonVeMesajGoster(y.basarimlarYuklenemedi);
                return false;
            }

            for (int i = 0; i < basarimlarTBL_.Length; i++)
            {
                //hedef bölüm sayısına ulaşıldıysa başarım tamamlandı demektir
                if (toplamGecilenBolumSayisi >= basarimlarTBL_[i].hedefDeger)
                {
                    //daha önce başarım tamamlanıp ödülü alındıysa yeni bir tamamlanan yok demektir
                    if (basarimKontrolTBL_ != null)
                    {
                        for (int i2 = 0; i2 < basarimKontrolTBL_.Length; i2++)
                        {
                            if (basarimKontrolTBL_[i2].basarimID == basarimlarTBL_[i].id)
                            {
                                if (!basarimKontrolTBL_[i2].odulAlindi)
                                {
                                    herhangiTamamlananVarMi = true;
                                }
                            }
                        }
                    }
                    else if (toplamGecilenBolumSayisi == 1)
                    {
                        herhangiTamamlananVarMi = true;
                    }
                }
            }

            return herhangiTamamlananVarMi;
        }

        public static void basarimKontrolUI(bool basarimKontrolR)
        {
            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;
            ImageView pg_bildirim = anaView.FindViewWithTag("pg_bildirimOverlay") as ImageView;
            if (basarimKontrolR)
                pg_bildirim.Visibility = ViewStates.Visible;

            else
                pg_bildirim.Visibility = ViewStates.Invisible;
        }

        public static void magazaPenceresiniAc()
        {
            try
            {
                c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                acikPencereleriKapat();

                ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                //loading çıkarıyoruz
                ProgressBar loadingBar = new ProgressBar(c);
                loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                loadingBar.Indeterminate = true;
                RelativeLayout rl = new RelativeLayout(c);
                rl.SetGravity(GravityFlags.Center);
                rl.AddView(loadingBar);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                anaView.AddView(rl, param);


                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                View profil = inflater.Inflate(Resource.Layout.magaza, null, false);

                PopupWindow popupWindow = new PopupWindow(profil, LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

                popupWindow.Focusable = false;
                popupWindow.OutsideTouchable = false;

                View pcw = popupWindow.ContentView;

                RelativeLayout menuR1 = pcw.FindViewById<RelativeLayout>(Resource.Id.magazaR1);
                menuR1.SetBackgroundResource(Resource.Drawable.p_arkaplan);

                c.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));


                ViewGroup popUpanaView = pcw.RootView as ViewGroup;


                LinearLayout magazaL2 = pcw.FindViewById<LinearLayout>(Resource.Id.magazaL2);
                var p2 = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
                p2.SetMargins(0, px(210), 0, 0);
                magazaL2.LayoutParameters = p2;
                magazaL2.RequestLayout();

                TextView jetonBasligiTxt = pcw.FindViewById<TextView>(Resource.Id.jetonBasligiTxt);
                int _ = px(30);
                jetonBasligiTxt.SetPadding(_, _, _, _);
                jetonBasligiTxt.SetTypeface(jetonBasligiTxt.Typeface, TypefaceStyle.Bold);
                jetonBasligiTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));

                ScrollView magazaScroll = pcw.FindViewById<ScrollView>(Resource.Id.magazaScroll);
                magazaScroll.SetPadding(0, 0, 0, px(200));
                var p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, px(1250));
                p.Gravity = GravityFlags.CenterHorizontal;
                p.SetMargins(px(5), px(18), 0, 0);
                magazaScroll.LayoutParameters = p;
                magazaScroll.RequestLayout();

                LinearLayout jetonPaketi1 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi1);
                p = new LinearLayout.LayoutParams(px(350), px(500));
                p.SetMargins(px(5), 0, px(25), 0);
                jetonPaketi1.LayoutParameters = p;
                jetonPaketi1.RequestLayout();

                TextView jetonMiktariTxt1 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt1);
                _ = px(30);
                jetonMiktariTxt1.SetPadding(_, _, _, _);
                jetonMiktariTxt1.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt1.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt1.LayoutParameters = p;
                jetonMiktariTxt1.RequestLayout();

                ImageView jetonGorseli1 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli1);
                p = new LinearLayout.LayoutParams(px(300), px(200));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli1.LayoutParameters = p;
                jetonGorseli1.RequestLayout();

                Button jetonFiyatBtn1 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn1);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn1.LayoutParameters = p;
                jetonFiyatBtn1.RequestLayout();

                LinearLayout jetonPaketi2 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi2);
                p = new LinearLayout.LayoutParams(px(350), px(500));
                jetonPaketi2.LayoutParameters = p;
                jetonPaketi2.RequestLayout();

                TextView jetonMiktariTxt2 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt2);
                _ = px(30);
                jetonMiktariTxt2.SetPadding(_, _, _, _);
                jetonMiktariTxt2.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt2.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt2.LayoutParameters = p;
                jetonMiktariTxt2.RequestLayout();

                ImageView jetonGorseli2 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli2);
                p = new LinearLayout.LayoutParams(px(300), px(200));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli2.LayoutParameters = p;
                jetonGorseli2.RequestLayout();

                Button jetonFiyatBtn2 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn2);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn2.LayoutParameters = p;
                jetonFiyatBtn2.RequestLayout();

                LinearLayout magaza_jetonPaketiSatir2 = pcw.FindViewById<LinearLayout>(Resource.Id.magaza_jetonPaketiSatir2);
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.SetMargins(0, px(25), 0, 0);
                magaza_jetonPaketiSatir2.LayoutParameters = p;
                magaza_jetonPaketiSatir2.RequestLayout();

                LinearLayout jetonPaketi3 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi3);
                p = new LinearLayout.LayoutParams(px(350), px(600));
                p.SetMargins(px(5), 0, px(25), 0);
                jetonPaketi3.LayoutParameters = p;
                jetonPaketi3.RequestLayout();

                TextView jetonMiktariTxt3 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt3);
                _ = px(30);
                jetonMiktariTxt3.SetPadding(_, _, _, _);
                jetonMiktariTxt3.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt3.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt3.LayoutParameters = p;
                jetonMiktariTxt3.RequestLayout();

                ImageView jetonGorseli3 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli3);
                p = new LinearLayout.LayoutParams(px(300), px(267));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli3.LayoutParameters = p;
                jetonGorseli3.RequestLayout();

                Button jetonFiyatBtn3 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn3);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn3.LayoutParameters = p;
                jetonFiyatBtn3.RequestLayout();

                LinearLayout jetonPaketi4 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi4);
                p = new LinearLayout.LayoutParams(px(350), px(600));
                jetonPaketi4.LayoutParameters = p;
                jetonPaketi4.RequestLayout();

                TextView jetonMiktariTxt4 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt4);
                _ = px(30);
                jetonMiktariTxt4.SetPadding(_, _, _, _);
                jetonMiktariTxt4.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt4.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt4.LayoutParameters = p;
                jetonMiktariTxt4.RequestLayout();

                ImageView jetonGorseli4 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli4);
                p = new LinearLayout.LayoutParams(px(300), px(267));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli4.LayoutParameters = p;
                jetonGorseli4.RequestLayout();

                Button jetonFiyatBtn4 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn4);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn4.LayoutParameters = p;
                jetonFiyatBtn4.RequestLayout();

                LinearLayout magaza_jetonPaketiSatir3 = pcw.FindViewById<LinearLayout>(Resource.Id.magaza_jetonPaketiSatir3);
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.SetMargins(0, px(25), 0, 0);
                magaza_jetonPaketiSatir3.LayoutParameters = p;
                magaza_jetonPaketiSatir3.RequestLayout();

                LinearLayout jetonPaketi5 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi5);
                p = new LinearLayout.LayoutParams(px(350), px(600));
                p.SetMargins(px(5), 0, px(25), 0);
                jetonPaketi5.LayoutParameters = p;
                jetonPaketi5.RequestLayout();

                TextView jetonMiktariTxt5 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt5);
                _ = px(30);
                jetonMiktariTxt5.SetPadding(_, _, _, _);
                jetonMiktariTxt5.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt5.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt5.LayoutParameters = p;
                jetonMiktariTxt5.RequestLayout();

                ImageView jetonGorseli5 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli5);
                p = new LinearLayout.LayoutParams(px(300), px(267));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli5.LayoutParameters = p;
                jetonGorseli5.RequestLayout();

                Button jetonFiyatBtn5 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn5);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn5.LayoutParameters = p;
                jetonFiyatBtn5.RequestLayout();

                LinearLayout jetonPaketi6 = pcw.FindViewById<LinearLayout>(Resource.Id.jetonPaketi6);
                p = new LinearLayout.LayoutParams(px(350), px(600));
                jetonPaketi6.LayoutParameters = p;
                jetonPaketi6.RequestLayout();

                TextView jetonMiktariTxt6 = pcw.FindViewById<TextView>(Resource.Id.jetonMiktariTxt6);
                _ = px(30);
                jetonMiktariTxt6.SetPadding(_, _, _, _);
                jetonMiktariTxt6.SetTypeface(jetonMiktariTxt1.Typeface, TypefaceStyle.Bold);
                jetonMiktariTxt6.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(45));
                p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonMiktariTxt6.LayoutParameters = p;
                jetonMiktariTxt6.RequestLayout();

                ImageView jetonGorseli6 = pcw.FindViewById<ImageView>(Resource.Id.jetonGorseli6);
                p = new LinearLayout.LayoutParams(px(300), px(267));
                p.Gravity = GravityFlags.Center;
                p.SetMargins(0, 0, 0, px(30));
                jetonGorseli6.LayoutParameters = p;
                jetonGorseli6.RequestLayout();

                Button jetonFiyatBtn6 = pcw.FindViewById<Button>(Resource.Id.jetonFiyatBtn6);
                p = new LinearLayout.LayoutParams(px(290), px(100));
                p.Gravity = GravityFlags.Center;
                jetonFiyatBtn6.LayoutParameters = p;
                jetonFiyatBtn6.RequestLayout();


                jetonBasligiTxt.Text = y.magazaBasligi;
                jetonMiktariTxt1.Text = y.jeton2;
                jetonGorseli1.SetBackgroundResource(Resource.Drawable.magazaJeton1);
                jetonFiyatBtn1.SetBackgroundResource(Resource.Drawable.magazaReklamBtn);//jetonFiyatBtn1.Text = "₺0";
                jetonFiyatBtn1.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popUpanaView.Visibility = ViewStates.Invisible;

                    popupWindow.Dismiss();
                    c.Window.SetSoftInputMode(SoftInput.StateHidden);
                    ortakAyarlar(c.Window);

                    await magazaReklamGoster();
                };



                jetonMiktariTxt2.Text = y.jeton20;
                jetonGorseli2.SetBackgroundResource(Resource.Drawable.magazaJeton2);
                jetonFiyatBtn2.Text = y.jeton20fiyat;
                jetonFiyatBtn2.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    magazaSatinAlinacakJetonSayisi = 20;
                    await jetonSatinAl();
                };



                jetonMiktariTxt3.Text = y.jeton50;
                jetonGorseli3.SetBackgroundResource(Resource.Drawable.magazaJeton3);
                jetonFiyatBtn3.Text = y.jeton50fiyat;
                jetonFiyatBtn3.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    magazaSatinAlinacakJetonSayisi = 50;
                    await jetonSatinAl();
                };



                jetonMiktariTxt4.Text = y.jeton120;
                jetonGorseli4.SetBackgroundResource(Resource.Drawable.magazaJeton4);
                jetonFiyatBtn4.Text = y.jeton120fiyat;
                jetonFiyatBtn4.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    magazaSatinAlinacakJetonSayisi = 120;
                    await jetonSatinAl();
                };



                jetonMiktariTxt5.Text = y.jeton320;
                jetonGorseli5.SetBackgroundResource(Resource.Drawable.magazaJeton5);
                jetonFiyatBtn5.Text = y.jeton320fiyat;
                jetonFiyatBtn5.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    magazaSatinAlinacakJetonSayisi = 320;
                    await jetonSatinAl();
                };



                jetonMiktariTxt6.Text = y.jeton750;
                jetonGorseli6.SetBackgroundResource(Resource.Drawable.magazaJeton6);
                jetonFiyatBtn6.Text = y.jeton750fiyat;
                jetonFiyatBtn6.Click += async delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();

                    magazaSatinAlinacakJetonSayisi = 750;
                    await jetonSatinAl();
                };



                Button magazaKapatBtn = pcw.FindViewById<Button>(Resource.Id.magazaKapatBtn);
                magazaKapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                magazaKapatBtn.Click += delegate
                {
                    sesEfektiCal("butonTiklandi");

                    popupWindow.Dismiss();
                };

                popupWindow.DismissEvent += delegate
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    f.UnbindDrawables(pcw);
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                };


                popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                BuPencereAcildi(popupWindow);
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        private static bool magazaVideoIzlendiMi = false, magazaVideoYuklenemedi = false;
        public static async Task magazaReklamGoster()
        {
            c.Window.AddFlags(WindowManagerFlags.NotTouchable);

            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

            Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

            //loading çıkarıyoruz
            ProgressBar loadingBar = new ProgressBar(c);
            loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
            loadingBar.Indeterminate = true;
            RelativeLayout rl = new RelativeLayout(c);
            rl.Tag = "loadingrl";
            rl.SetGravity(GravityFlags.Center);
            rl.AddView(loadingBar);
            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            anaView.AddView(rl, param);


            Action yap = () =>
            {
                c.RunOnUiThread(delegate
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(anaView.FindViewWithTag("loadingrl"));
                    c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    c.Window.SetSoftInputMode(SoftInput.StateHidden);
                    ortakAyarlar(c.Window);
                    magazaPenceresiniAc();

                    magazaVideoYuklenemedi = false;
                });
            };

            var t7 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jtk&_g=" + googleProfile_.Id));
            await t7.ConfigureAwait(false);
            if (t7.Result == "ok")
            {
                Action magazaReklamiSifirla = delegate
                {
                    _GoogleAds._rewardedAd = null;
                    _GoogleAds.RewardedAdIsLoaded = false;
                    _GoogleAds.RewardedAdIsFailed = false;

                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();
                };

                Action magazaReklamiKapandi = async delegate
                {
                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();

                    if (!magazaVideoYuklenemedi)
                    {
                        if (magazaVideoIzlendiMi)
                        {
                            //Eğer reklam başarı ile gösterildiyse jetonunu veriyoruz
                            var t8 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jta&_g=" + googleProfile_.Id));
                            await t8.ConfigureAwait(false);
                            if (t8.Result == "ok")
                            {
                                await Task.Delay(500);

                                //Jeton bilgileri çekiliyor
                                var t5 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jt&_g=" + googleProfile_.Id));
                                await t5.ConfigureAwait(false);
                                if (t5.Result != null) jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                                else
                                {
                                    anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                    return;
                                }

                                c.RunOnUiThread(delegate
                                {
                                    ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;
                                    TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                                    ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";

                                    magazaVideoIzlendiMi = false;

                                    magazaReklamiSifirla();
                                    mesajGoster(y.ucretsizJetonAlindi, yap);
                                });
                            }
                            else
                            {
                                anaEkranaDonVeMesajGoster(y.ucretsizJetonHata);
                                return;
                            }
                        }
                        else
                        {
                            await Task.Delay(500);
                            magazaReklamiSifirla();
                            mesajGoster(y.ucretsizJetonReklamKapatildi, yap);
                        }
                    }
                    else
                    {
                        await Task.Delay(500);
                        magazaReklamiSifirla();
                        mesajGoster(y.ucretsizJetonReklamGoruntulenemedi, yap);
                    }
                };


                AdRequest adRequest = new AdRequest.Builder().Build();

                var _r = new _GoogleAds._RewardedAdLoadCallback();
                _r._OnRewardedAdLoaded = (s, e) =>
                {
                    if (_GoogleAds.RewardedAdIsLoaded && !_GoogleAds.RewardedAdIsFailed)
                    {
                        if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Pause();
                        sesEfektiDurdur();

                        var _f = new _GoogleAds._FullScreenContentCallback();
                        _f._OnAdDismissedFullScreenContent = (s, e) =>
                        {
                            magazaReklamiKapandi();
                        };
                        _f._OnAdFailedToShowFullScreenContent = async (s, e) =>
                        {
                            magazaVideoYuklenemedi = true;

                            await Task.Delay(500);
                            magazaReklamiSifirla();
                            mesajGoster(y.reklamYuklenemedi, yap);
                        };
                        _f._OnAdShowedFullScreenContent = (s, e) =>
                        {
                            _GoogleAds._rewardedAd = null;
                        };
                        _GoogleAds._rewardedAd.FullScreenContentCallback = _f;

                        var rewardListener = new _GoogleAds._IOnUserEarnedRewardListener();
                        rewardListener.OnRewarded = (s, e) =>
                        {
                            magazaVideoIzlendiMi = true;
                        };

                        _GoogleAds._rewardedAd.Show(c, rewardListener);
                    }
                };
                _r._OnAdFailedToLoad = (s, e) =>
                {
                    magazaVideoIzlendiMi = false;

                    magazaReklamiSifirla();

                    mesajGoster(y.reklamYuklenemedi, yap);
                };

                c.RunOnUiThread(delegate
                {
                    Android.Gms.Ads.Hack.RewardedAd.Load(c, admob_magaza_reklam_id, adRequest, _r);
                });
            }
            else if (t7.Result != "")
            {
                mesajGoster(y.p(y.ucretsizJetonHakkiBitti, t7.Result), yap);
            }
            else
            {
                anaEkranaDonVeMesajGoster(y.ucretsizJetonReklamKontroluYapilamadi);
                return;
            }
        }


        public static AppCompatActivity _c;
        public static int __soruID;
        public static bool gecisReklamiGosterilecek;

        public static void gecisReklaminiHazirEt()
        {
            try
            {
                Action gecisReklamiSifirla = delegate
                {
                    _GoogleAds._interstitialAd = null;
                    _GoogleAds.InterstitialAdIsLoaded = false;
                    _GoogleAds.InterstitialAdIsFailed = false;
                    _c = null;
                    __soruID = 0;
                    gecisReklamiGosterilecek = false;

                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();
                };

                gecisReklamiSifirla();

                Action gecisReklamiGosterildi = delegate
                {
                    if (_c != null && __soruID != 0)
                    {
                        if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();

                        _c.Window.SetSoftInputMode(SoftInput.StateHidden);
                        ortakAyarlar(_c.Window);

                        _c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        f.tumNesneleriDeaktifEt();

                        var c = _c;
                        int sid = __soruID;

                        gecisReklamiSifirla();
                        bolumuBaslat(sid);
                    }
                };

                AdRequest adRequest = new AdRequest.Builder().Build();

                var _i = new _GoogleAds._InterstitialAdLoadCallback();
                _i._OnInterstitialAdLoaded = (s, e) =>
                {
                    var _f = new _GoogleAds._FullScreenContentCallback();
                    _f._OnAdDismissedFullScreenContent = (s, e) =>
                    {
                        gecisReklamiGosterildi();
                    };
                    _f._OnAdFailedToShowFullScreenContent = (s, e) =>
                    {
                        gecisReklamiSifirla();
                    };
                    _f._OnAdShowedFullScreenContent = (s, e) =>
                    {
                        _GoogleAds._interstitialAd = null;
                    };

                    _GoogleAds._interstitialAd.FullScreenContentCallback = _f;
                };
                _i._OnAdFailedToLoad = (s, e) =>
                {
                    gecisReklamiSifirla();
                };

                Android.Gms.Ads.Hack.InterstitialAd.Load(c, admob_gecis_reklam_id, adRequest, _i);

                gecisReklamiGosterilecek = true;
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static void gecisReklamiGosterVeGec(int soruID)
        {
            try
            {
                if (_GoogleAds.InterstitialAdIsLoaded && !_GoogleAds.InterstitialAdIsFailed)
                {
                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Pause();
                    sesEfektiDurdur();

                    _c = c;
                    __soruID = soruID;

                    f.yukleniyorOlustur();
                    _GoogleAds._interstitialAd.Show(c);
                }
                else
                {
                    c.Window.SetSoftInputMode(SoftInput.StateHidden);
                    ortakAyarlar(c.Window);

                    c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                    f.tumNesneleriDeaktifEt();

                    bolumuBaslat(soruID);
                }
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
        }

        public static _BillingClient m_BillingClient;
        public static int magazaSatinAlinacakJetonSayisi = 0;
        public static int magazaMsjSayisi = 0;
        public static bool magazaPending = false;

        public static void magazaMesaji(string m)
        {
            c.RunOnUiThread(async () =>
            {
                acikPencereleriKapat();
                c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                c.Window.SetSoftInputMode(SoftInput.StateHidden);
                ortakAyarlar(c.Window);
                await Task.Delay(500);
                mesajGoster(m, delegate
                {
                    magazaMsjSayisi--;
                    if (magazaMsjSayisi <= 0)
                        magazaPenceresiniAc();
                });
            });
        }
        public static async Task<bool> magazaJetonVer(string donenJson)
        {
            bool r = false;

            //Eğer satın alma başarı ile tamamlandıysa jetonunu veriyoruz


            Dictionary<string, string> veriler = new Dictionary<string, string>{
                { "_d", "jsa"},
                { "_p0", googleProfile_.Id},
                { "_p1", magazaSatinAlinacakJetonSayisi.ToString()},
                { "_p2", donenJson}
            };

            var t = Task.Run(() => SendPostToURI(server_main_link, veriler));
            await t.ConfigureAwait(false);
            if (t.Result == "ok")
            {

                //Jeton bilgileri çekiliyor
                var t5 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jt&_g=" + googleProfile_.Id));
                await t5.ConfigureAwait(false);
                if (t5.Result != null) jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                else
                {
                    r = false;
                    anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                }

                if (jetonlarTBL_ == null)
                {
                    r = false;
                    anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                }

                c.RunOnUiThread(() =>
                {
                    ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                    TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                    ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";
                });

                magazaPending = false;

                magazaMsjSayisi++;
                magazaMesaji(y.jetonSatinAlindi);

                r = true;
            }
            else
            {
                anaEkranaDonVeMesajGoster(y.jetonYuklenirkenHata);
                r = false;
            }

            return r;
        }
        public static async Task magazaJetonKontrolu(Purchase purc)
        {
            //satın alınan ürün idleri kadar döngü
            foreach (var sku in purc.Skus)
            {
                string siparisID;
                //ilk girişse ve bekleyen bir alım varsa onun sku idsini giriyoruz
                if (magazaSatinAlinacakJetonSayisi == 0) siparisID = sku;
                else siparisID = magazaSatinAlinacakJetonSayisi + "jeton";

                //alınma başarılı ise ve sipariş istenilen ürün ise
                if (purc.PurchaseState == PurchaseState.Purchased && sku == siparisID)
                {
                    //Play Consoledaki base64 kodu ile alışverişi onaylama yaptırıyoruz
                    if (!_Security.VerifyValidSignature(purc.OriginalJson, purc.Signature))
                    {
                        magazaMsjSayisi++;
                        magazaMesaji(y.jetonSatinAlmaGecersiz);
                        return;
                    }
                    // else satın alma geçerli ama onaylanmadıysa
                    if (!purc.IsAcknowledged)
                    {
                        AcknowledgePurchaseParams acknowledgePurchaseParams =
                                AcknowledgePurchaseParams.NewBuilder()
                                        .SetPurchaseToken(purc.PurchaseToken)
                                        .Build();

                        m_BillingClient._OnAcknowledgePurchaseResponse = async (s, e) =>
                        {
                            var p0 = s as BillingResult;

                            //satın alma geçerli ve artık onaylandıysa
                            if (p0.ResponseCode == BillingResponseCode.Ok)
                            {
                                //jetonu veriyoruz ve başarıyla verildiyse harcandığını apiye bildiriyoruz
                                if (await magazaJetonVer(purc.OriginalJson))
                                    m_BillingClient.ConsumePurchase(purc.PurchaseToken);
                            }
                        };

                        //onaylama talebi yapıyoruz
                        m_BillingClient.billingClient.AcknowledgePurchase(acknowledgePurchaseParams, m_BillingClient);
                    }
                    //else satın alma hem geçerli hem onaylandıysa
                    else
                    {
                        //jetonu veriyoruz ve başarıyla verildiyse harcandığını apiye bildiriyoruz
                        if (await magazaJetonVer(purc.OriginalJson))
                            m_BillingClient.ConsumePurchase(purc.PurchaseToken);
                    }
                }
                else if (purc.PurchaseState == PurchaseState.Pending && sku == siparisID)
                {
                    magazaPending = true;
                    magazaMsjSayisi++;
                    magazaMesaji(y.jetonSatinAlmaBeklemede);
                }
                else if (purc.PurchaseState == PurchaseState.Unspecified && sku == siparisID)
                {
                    magazaMsjSayisi++;
                    magazaMesaji(y.jetonSatinAlmaAnlasilamadi);
                }
                else
                {
                    magazaMsjSayisi++;
                    magazaMesaji(y.jetonSatinAlmaBilinemiyor);
                }
            }
        }
        public static async Task magazaAyarlariYukle()
        {
            if (m_BillingClient == null)
            {
                m_BillingClient = new _BillingClient();


                m_BillingClient._OnBillingServiceDisconnected = delegate
                {
                    magazaMsjSayisi++;
                    magazaMesaji(y.jetonSatinAlmaBaglantiKesildi);
                };
                m_BillingClient._OnBillingSetupFinished = (s, e) =>
                {
                    var p0 = s as BillingResult;

                    //ödeme kurulumu başarılı ise
                    if (p0.ResponseCode == BillingResponseCode.Ok)
                    {
                        //ürünleri yüklüyoruz
                        m_BillingClient.LoadPurchases();
                    }
                    else
                    {
                        magazaMsjSayisi++;
                        magazaMesaji(y.p(y.jetonSatinAlmaAyarlanamadi, p0.ResponseCode));
                    }
                };
                m_BillingClient._OnPurchasesUpdated = async (s, e) =>
                {
                    var ss = s as object[];
                    var p0 = ss[0] as BillingResult;
                    var p1 = ss[1] as IList<Purchase>;

                    if (p0.ResponseCode == BillingResponseCode.Ok && p1 != null)
                    {
                        //satın alınan ürün miktarı kadar döngü
                        foreach (var purc in p1)
                        {
                            await magazaJetonKontrolu(purc);
                        }
                    }
                    else if (p0.ResponseCode == BillingResponseCode.UserCancelled)
                    {
                        magazaMsjSayisi++;
                        magazaMesaji(y.jetonSatinAlmaIptalEdildi);
                    }
                    else if (p0.ResponseCode == BillingResponseCode.ItemAlreadyOwned)
                    {
                        magazaMsjSayisi++;
                        magazaMesaji(y.jetonSatinAlmaBekliyor);
                    }
                    else
                    {
                        magazaMsjSayisi++;
                        if (magazaPending)
                            magazaMesaji(y.jetonSatinAlmaOnayBekliyor);
                        else
                            magazaMesaji(y.jetonSatinAlmaHata);
                    }
                };
                m_BillingClient._OnSkuDetailsResponse = (s, e) =>
                {
                    var ss = s as object[];
                    var p0 = ss[0] as BillingResult;
                    var p1 = ss[1] as IList<SkuDetails>;

                    if (p0.ResponseCode == BillingResponseCode.Ok)
                    {
                        //sku (ürün) detayları google play consoledan başarı ile çekildi
                        m_BillingClient.InitProductAdapter(p1);

                        //magazaSatinAlinacakOgePenceresiniAc(c, p1);
                    }
                    else
                    {
                        magazaMsjSayisi++;
                        magazaMesaji(y.p(y.jetonSatinAlmaUrunHatasi, p0.ResponseCode));
                    }
                };

                m_BillingClient._OnConsumeResponse = (s, e) =>
                {
                    var ss = s as object[];
                    var p0 = ss[0] as BillingResult;
                    var p1 = ss[1] as string;

                    //ürünü başarı ile harcandığını bildir sil & temizle (tekrar eden harcamaları önlemek için)
                    if (p0.ResponseCode == BillingResponseCode.Ok && p1 != null)
                    {
                        //ürün token'i başarı ile kaldırıldı
                    }
                    else
                    {
                        //genelde pending (kart onayı bekleme) sırasında harcama yapıldığında buraya giriyor
                        //magazaMsjSayisi++;
                        //magazaMesaji("Satın alım sırasında bazı sorunlar oluştu. :: " + p0.ResponseCode, c);
                    }
                };

            }

            
            //queryde pending durumunda olanlar okeylendiyse jeton ver
            if (m_BillingClient.billingClient.IsReady)
            {
                m_BillingClient._IpurchasedIPurchasesResponseListener = async (s, e) =>
                {
                    var ss = s as object[];
                    var p0 = ss[0] as BillingResult;
                    var p1 = ss[1] as IList<Purchase>;

                    if (p0.ResponseCode == BillingResponseCode.Ok && p1 != null)
                    {
                        //satın alınan ürün miktarı kadar döngü
                        foreach (var purc in p1)
                        {
                            await magazaJetonKontrolu(purc);
                        }
                    }
                };
            }
        }
        public static void magazaSatinAlinacakOgePenceresiniAc(IList<SkuDetails> skudetails = null)
        {
            if (skudetails == null)
            {
                if (m_BillingClient.SkuDetails != null)
                    skudetails = m_BillingClient.SkuDetails;
            }

            if (m_BillingClient != null)
            {
                if (skudetails != null)
                {
                    foreach (var i in skudetails)
                    {
                        string siparisID = magazaSatinAlinacakJetonSayisi + "jeton";
                        string productId = i.Sku;
                        if (productId == siparisID)
                        {
                            //alınan bütün ürünleri harcıyoruz veya temizliyoruz
                            //m_BillingClient.ClearOrConsumeAllPurchases();

                            //hangi ürün alınmak isteniyorsa onunla eşit ürün bilgilerini bul ve alışverişi başlat
                            m_BillingClient.PurchaseNow(i);
                            break;
                        }
                    }
                }
            }
        }
        public static async Task jetonSatinAl()
        {
            c.Window.AddFlags(WindowManagerFlags.NotTouchable);

            await magazaAyarlariYukle();

            magazaSatinAlinacakOgePenceresiniAc();
        }




        private static bool sandikVideoIzlendiMi = false, sandikVideoYuklenemedi = false;
        public static string sandikJetonR, sandikIslem;
        public static bool IsRewardedVideoLoadedFirst = true;
        public static async Task sandikReklamGoster(int verilecekJetonSayisi, bool reklamGoster)
        {
            sandikVideoYuklenemedi = false;
            string reklamGosterildi = "h";

            c.Window.AddFlags(WindowManagerFlags.NotTouchable);

            ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

            Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

            //loading çıkarıyoruz
            ProgressBar loadingBar = new ProgressBar(c);
            loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
            loadingBar.Indeterminate = true;
            RelativeLayout rl = new RelativeLayout(c);
            rl.Tag = "loading";
            rl.SetGravity(GravityFlags.Center);
            rl.AddView(loadingBar);
            var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
            anaView.AddView(rl, param);

            Task jetonVer = new Task(async delegate
            {
                //jetonunu veriyoruz
                var t8 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jts&_j=" + verilecekJetonSayisi + "&_t=" + reklamGosterildi + "&_g=" + googleProfile_.Id));
                await t8.ConfigureAwait(false);
                if (t8.Result == "ok")
                {
                    //Jeton bilgileri çekiliyor
                    var t5 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jt&_g=" + googleProfile_.Id));
                    await t5.ConfigureAwait(false);
                    if (t5.Result != null) jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                    else
                    {
                        anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                        return;
                    }

                    sandikVideoIzlendiMi = false;

                    sandikJetonR = y.p(y.sandikJetonKazandiniz, verilecekJetonSayisi);

                    c.RunOnUiThread(async delegate
                    {
                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);

                        await sandikGoster();
                    });
                }
                else
                {
                    anaEkranaDonVeMesajGoster(y.ucretsizJetonHata);
                    return;
                }

                return;
            });


            if (reklamGoster)
            {

                Action sandikReklamiSifirla = delegate
                {
                    _GoogleAds._rewardedAd = null;
                    _GoogleAds.RewardedAdIsLoaded = false;
                    _GoogleAds.RewardedAdIsFailed = false;

                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();
                };

                Action sandikReklamiKapandi = async delegate
                {
                    if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Start();

                    IsRewardedVideoLoadedFirst = true;

                    if (!sandikVideoYuklenemedi)
                    {
                        if (sandikVideoIzlendiMi)
                        {
                            reklamGosterildi = "e";
                            jetonVer.Start();
                            await jetonVer.ConfigureAwait(false);
                        }
                        else
                        {
                            anaView.Foreground = varsayilanArkaplan;
                            anaView.RemoveView(rl);

                            sandikJetonR = "kapatildi";
                        }
                    }
                    else
                    {
                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);

                        sandikJetonR = "yuklenemedi";
                    }
                };

                AdRequest adRequest = new AdRequest.Builder().Build();

                var _r = new _GoogleAds._RewardedAdLoadCallback();
                _r._OnRewardedAdLoaded = (s, e) =>
                {
                    if (_GoogleAds.RewardedAdIsLoaded && !_GoogleAds.RewardedAdIsFailed)
                    {
                        if (IsRewardedVideoLoadedFirst)
                        {
                            IsRewardedVideoLoadedFirst = false;
                            if (oAyarlariTBL_.muzik) if (playerBG != null) playerBG.Pause();
                            sesEfektiDurdur();

                            var _f = new _GoogleAds._FullScreenContentCallback();
                            _f._OnAdDismissedFullScreenContent = (s, e) =>
                            {
                                sandikReklamiKapandi();
                            };
                            _f._OnAdFailedToShowFullScreenContent = (s, e) =>
                            {
                                anaView.Foreground = varsayilanArkaplan;
                                anaView.RemoveView(rl);
                                sandikVideoYuklenemedi = true;

                                sandikReklamiSifirla();

                                sandikJetonR = "yuklenemedi";
                            };
                            _f._OnAdShowedFullScreenContent = (s, e) =>
                            {
                                _GoogleAds._rewardedAd = null;
                            };
                            _GoogleAds._rewardedAd.FullScreenContentCallback = _f;

                            var rewardListener = new _GoogleAds._IOnUserEarnedRewardListener();
                            rewardListener.OnRewarded = (s, e) =>
                            {
                                sandikVideoIzlendiMi = true;
                            };

                            _GoogleAds._rewardedAd.Show(c, rewardListener);
                        }
                    }
                };
                _r._OnAdFailedToLoad = (s, e) =>
                {
                    anaView.Foreground = varsayilanArkaplan;
                    anaView.RemoveView(rl);
                    sandikVideoYuklenemedi = true;

                    sandikReklamiSifirla();

                    sandikJetonR = "yuklenemedi";
                };

                Android.Gms.Ads.Hack.RewardedAd.Load(c, admob_sandik_reklam_id, adRequest, _r);
            }
            else
            {
                jetonVer.Start();
                await jetonVer.ConfigureAwait(false);
            }
        }

        public static async Task sandikGoster()
        {
            try
            {
                if (sandikJetonR == "kapatildi" || sandikJetonR == "yuklenemedi")
                {
                    sandikJetonR = null;
                    sandikIslem = null;
                }
                else
                {
                    c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                    acikPencereleriKapat();

                    //açılırken problem olmaması için biraz bekletiyoruz
                    if (sandikJetonR == null && sandikIslem == null) await Task.Delay(600);

                    ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                    Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);
                    //loading çıkarıyoruz
                    ProgressBar loadingBar = new ProgressBar(c);
                    loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                    loadingBar.Indeterminate = true;
                    RelativeLayout rl = new RelativeLayout(c);
                    rl.SetGravity(GravityFlags.Center);
                    rl.AddView(loadingBar);
                    var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    anaView.AddView(rl, param);


                    LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                    View sandikAcL = inflater.Inflate(Resource.Layout.sandikAc, null, false);


                    int width = LinearLayout.LayoutParams.WrapContent;
                    int height = LinearLayout.LayoutParams.WrapContent;
                    PopupWindow popupWindow = new PopupWindow(sandikAcL, width, height);

                    popupWindow.Focusable = false;
                    popupWindow.OutsideTouchable = false;

                    View pcw = popupWindow.ContentView;

                    LinearLayout sandikAcUstL = pcw.FindViewById<LinearLayout>(Resource.Id.sandikAcUstL);
                    sandikAcUstL.SetPadding(px(60), px(130), px(60), px(160));

                    TextView baslikTxt = pcw.FindViewById<TextView>(Resource.Id.baslikTxt);
                    baslikTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(55));
                    var p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    p2.Gravity = GravityFlags.Center;
                    int _ = px(20);
                    p2.SetMargins(_, _, _, _);
                    baslikTxt.LayoutParameters = p2;
                    baslikTxt.RequestLayout();

                    ImageView sandikGorseli = pcw.FindViewById<ImageView>(Resource.Id.sandikGorseli);
                    p2 = new LinearLayout.LayoutParams(px(900), px(900));
                    _ = px(30);
                    p2.SetMargins(_, _, _, _);
                    p2.Gravity = GravityFlags.Center;
                    sandikGorseli.LayoutParameters = p2;
                    sandikGorseli.RequestLayout();


                    Button sandikAcBtn = pcw.FindViewById<Button>(Resource.Id.sandikAcBtn);
                    p2 = new LinearLayout.LayoutParams(px(600), px(100));
                    _ = px(20);
                    p2.SetMargins(_, _, _, _);
                    p2.Gravity = GravityFlags.Center;
                    sandikAcBtn.LayoutParameters = p2;
                    sandikAcBtn.RequestLayout();

                    Button sandikIptalBtn = pcw.FindViewById<Button>(Resource.Id.sandikIptalBtn);
                    p2 = new LinearLayout.LayoutParams(px(600), px(100));
                    _ = px(20);
                    p2.SetMargins(_, _, _, _);
                    p2.Gravity = GravityFlags.Center;
                    sandikIptalBtn.LayoutParameters = p2;
                    sandikIptalBtn.RequestLayout();


                    baslikTxt.SetTypeface(baslikTxt.Typeface, TypefaceStyle.Bold);
                    baslikTxt.Text = y.gunlukUcretsizSandik;

                    sandikGorseli.SetBackgroundResource(Resource.Drawable.sandik1);

                    sandikIptalBtn.Text = y.acma_btn;
                    sandikIptalBtn.Click += delegate
                    {
                        sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();

                        c.Window.SetSoftInputMode(SoftInput.StateHidden);
                        ortakAyarlar(c.Window);

                        sandikIslem = y.bitti_btn;
                        sandikJetonR = null;
                    };


                    sandikAcBtn.Text = y.sandikAc_btn;

                    popupWindow.DismissEvent += delegate
                    {
                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);
                        f.UnbindDrawables(pcw);
                        c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    };

                    //ALLAHIN BELASI YER
                    popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                    BuPencereAcildi(popupWindow);


                    if ((sandikIslem == y.izleVeSandikAc_btn || sandikIslem == y.sandikAc_btn) && sandikJetonR != null)
                    {
                        sandikIptalBtn.Visibility = ViewStates.Gone;
                        sandikAcBtn.Enabled = false;
                        sandikAcBtn.Visibility = ViewStates.Invisible;

                        sesEfektiCal("kutuAciliyor");

                        await Task.Delay(3000);

                        sandikGorseli.SetBackgroundResource(Resource.Drawable.sandik2);

                        await Task.Delay(1000);


                        TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                        ustJetonTxt.Text = jetonlarTBL_.jetonSayisi + "";

                        if (sandikJetonR != null)
                            baslikTxt.Text = sandikJetonR;


                        sesEfektiCal("kutuAcildi");

                        sandikGorseli.SetBackgroundResource(Resource.Drawable.sandik3);


                        if (sandikIslem == y.sandikAc_btn)
                            sandikAcBtn.Text = y.tamam_btn;
                        else if (sandikIslem == y.izleVeSandikAc_btn)
                            sandikAcBtn.Text = y.kapat_btn;

                        sandikAcBtn.Enabled = true;
                        sandikAcBtn.Visibility = ViewStates.Visible;


                        sandikJetonR = null;
                    }

                    sandikAcBtn.Click += async delegate
                    {
                        sesEfektiCal("butonTiklandi");

                        if (sandikAcBtn.Text == y.sandikAc_btn)
                        {
                            sandikAcBtn.Enabled = false;
                            sandikAcBtn.Visibility = ViewStates.Invisible;


                            popupWindow.Dismiss();


                            Random rnd = new Random();
                            int rastgeleJeton = rnd.Next(1, 10);

                            sandikIslem = y.sandikAc_btn;

                            await sandikReklamGoster(rastgeleJeton, false);
                        }
                        else if (sandikAcBtn.Text == y.tamam_btn)
                        {
                            baslikTxt.Text = y.videoIzleVeSandikAc;
                            sandikGorseli.SetBackgroundResource(Resource.Drawable.sandik1);

                            sandikAcBtn.Text = y.izleVeSandikAc_btn;
                            sandikIptalBtn.Visibility = ViewStates.Visible;
                        }
                        else if (sandikAcBtn.Text == y.izleVeSandikAc_btn)
                        {
                            sandikIptalBtn.Visibility = ViewStates.Gone;
                            sandikAcBtn.Enabled = false;
                            sandikAcBtn.Visibility = ViewStates.Invisible;

                            popupWindow.Dismiss();

                            sandikIslem = y.izleVeSandikAc_btn;

                            await sandikReklamGoster(10, true);
                        }
                        else if (sandikAcBtn.Text == y.kapat_btn)
                        {
                            popupWindow.Dismiss();

                            c.Window.SetSoftInputMode(SoftInput.StateHidden);
                            ortakAyarlar(c.Window);

                            sandikIslem = y.bitti_btn;
                            sandikJetonR = null;
                        }
                    };

                }
            }
            catch (Exception ex)
            {
                string e = y.p(y.hataOlustu, ex);
                if (logRecording)
                {
                    main_log += e + "\n\n";
                }
                anaEkranaDonVeMesajGoster(e);
                return;
            }
            return;
        }

        public static async Task<bool> gunlukSandikKontrolu()
        {
            var t7 = Task.Run(() => GetResponseFromURI(server_main_link + "?_d=jtd&_g=" + googleProfile_.Id));
            await t7.ConfigureAwait(false);
            if (t7.Result == "yes")
            {
                //günlük jeton hakkı geldi
                return true;
            }
            else if (t7.Result == "no")
            {
                //Henüz günlük jeton hakkın yenilenmedi.
                return false;
            }
            else
            {
                anaEkranaDonVeMesajGoster(y.gunlukJetonHata);
                return false;
            }
        }

        public static void titret(int sure)
        {
            try
            {
                if (oAyarlariTBL_ != null)
                {
                    if (oAyarlariTBL_.titresim)
                    {
                        var v = (Vibrator)Application.Context.GetSystemService(Application.VibratorService);
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                        {
                            v.Vibrate(VibrationEffect.CreateOneShot(sure, VibrationEffect.DefaultAmplitude));
                        }
                        else
                        {
                            #pragma warning disable 0618
                            v.Vibrate(sure);
                            #pragma warning restore 0618
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (logRecording)
                {
                    string e = y.p(y.hataOlustu, ex);
                    main_log += e + "\n\n";
                }
            }
        }


        public static int sonMuzik = 0, muzikSayisi = 6;

        public static Android.Media.MediaPlayer playerBG;
        public static void muzikBaslatDurdur()
        {
            if (oAyarlariTBL_ != null)
            {
                if (oAyarlariTBL_.muzik)
                {
                    if (playerBG == null)
                    {
                        playerBG = new Android.Media.MediaPlayer();
                        playerBG.Looping = false;
                        playerBG.SetAudioAttributes(
                            new Android.Media.AudioAttributes.Builder()
                            .SetUsage(Android.Media.AudioUsageKind.Media)
                            .SetFlags(Android.Media.AudioFlags.LowLatency)
                            .SetContentType(Android.Media.AudioContentType.Music).Build());

                        //loop çalışmadığı için el ile bittiğinde tekrar başlatıyoruz
                        playerBG.Completion += delegate
                        {
                            if (oAyarlariTBL_ != null)
                            {
                                if (oAyarlariTBL_.muzik)
                                {
                                //müzik bittiğinde sonraki müziğe geçiyoruz
                                sonMuzik++;

                                //son müzikse 1. müziğe dönüyoruz
                                if (sonMuzik >= muzikSayisi) sonMuzik = 1;


                                    playerBG.Reset();
                                    try
                                    {
                                        int nm = 1;
                                        if (sonMuzik == 1) nm = Resource.Raw.m1;
                                        else if (sonMuzik == 2) nm = Resource.Raw.m2;
                                        else if(sonMuzik == 3) nm = Resource.Raw.m3;
                                        else if(sonMuzik == 4) nm = Resource.Raw.m4;
                                        else if(sonMuzik == 5) nm = Resource.Raw.m5;
                                        else if (sonMuzik == 6) nm = Resource.Raw.m6;
                                        Android.Content.Res.AssetFileDescriptor file = c.Resources.OpenRawResourceFd(nm);
                                        playerBG.SetDataSource(file.FileDescriptor, file.StartOffset, file.Length);
                                    }
                                    catch { }//kontrollü catch
                                try
                                    {
                                        playerBG.Prepare();
                                    }
                                    catch { }//kontrollü catch

                                playerBG.Start();
                                }
                            }
                        };


                        //ilk açılış ise rastgele bir müzikten başlıyoruz
                        if (sonMuzik == 0) sonMuzik = new Random().Next(1, muzikSayisi + 1);

                        playerBG.Reset();
                        try
                        {
                            int nm = 1;
                            if (sonMuzik == 1) nm = Resource.Raw.m1;
                            else if (sonMuzik == 2) nm = Resource.Raw.m2;
                            else if (sonMuzik == 3) nm = Resource.Raw.m3;
                            else if (sonMuzik == 4) nm = Resource.Raw.m4;
                            else if (sonMuzik == 5) nm = Resource.Raw.m5;
                            else if (sonMuzik == 6) nm = Resource.Raw.m6;
                            Android.Content.Res.AssetFileDescriptor file = c.Resources.OpenRawResourceFd(nm);
                            playerBG.SetDataSource(file.FileDescriptor, file.StartOffset, file.Length);
                        }
                        catch { }//kontrollü catch

                        try
                        {
                            playerBG.Prepare();
                        }
                        catch { }//kontrollü catch

                        playerBG.Start();
                    }
                    else
                    {
                        if (playerBG.IsPlaying) playerBG.Pause();
                        else playerBG.Start();
                    }

                }
                else
                {
                    if (playerBG != null) playerBG.Pause();
                }
            }
        }

        //yeni ses efekti eklenirken fxIds sayısı arttırılacak
        public static Android.Media.SoundPool sp_basarimAcildi, sp_bolumGecildi, sp_butonTiklandi,
            sp_harfAl, sp_ipucu, sp_kelimeAcildi, sp_kelimeAciliyor, sp_klavyeBasildi,
            sp_kutuAcildi, sp_kutuAciliyor, sp_kutuTiklandi, sp_yanlisCevap;

        public static int[] fxIds = new int[12];
        private static int fxSay = 0;

        private static Android.Media.SoundPool _sy(Android.Media.SoundPool sp, int nm)
        {
            //ses efektini yükle
            Android.Media.AudioAttributes aa = new Android.Media.AudioAttributes.Builder().SetContentType(Android.Media.AudioContentType.Music).SetUsage(Android.Media.AudioUsageKind.Game).Build();
            sp = new Android.Media.SoundPool.Builder().SetMaxStreams(3).SetAudioAttributes(aa).Build();
            fxIds[fxSay] = sp.Load(c.Resources.OpenRawResourceFd(nm), 1);
            fxSay++;
            return sp;
        }
        public static void sesEfektleriniYukle()
        {
            sp_basarimAcildi = _sy(sp_basarimAcildi, Resource.Raw.basarimAcildi);
            sp_bolumGecildi = _sy(sp_bolumGecildi, Resource.Raw.bolumGecildi);
            sp_butonTiklandi = _sy(sp_butonTiklandi, Resource.Raw.butonTiklandi);
            sp_harfAl = _sy(sp_harfAl, Resource.Raw.harfAl);
            sp_ipucu = _sy(sp_ipucu, Resource.Raw.ipucu);
            sp_kelimeAcildi = _sy(sp_kelimeAcildi, Resource.Raw.kelimeAcildi);
            sp_kelimeAciliyor = _sy(sp_kelimeAciliyor, Resource.Raw.kelimeAciliyor);
            sp_klavyeBasildi = _sy(sp_klavyeBasildi, Resource.Raw.klavyeBasildi);
            sp_kutuAcildi = _sy(sp_kutuAcildi, Resource.Raw.kutuAcildi);
            sp_kutuAciliyor = _sy(sp_kutuAciliyor, Resource.Raw.kutuAciliyor);
            sp_kutuTiklandi = _sy(sp_kutuTiklandi, Resource.Raw.kutuTiklandi);
            sp_yanlisCevap = _sy(sp_yanlisCevap, Resource.Raw.yanlisCevap);
        }
        public static void sesEfektiDurdur()
        {
            try
            {
                sp_basarimAcildi.Stop(fxIds[0]);
                sp_bolumGecildi.Stop(fxIds[1]);
                sp_butonTiklandi.Stop(fxIds[2]);
                sp_harfAl.Stop(fxIds[3]);
                sp_ipucu.Stop(fxIds[4]);
                sp_kelimeAcildi.Stop(fxIds[5]);
                sp_kelimeAciliyor.Stop(fxIds[6]);
                sp_klavyeBasildi.Stop(fxIds[7]);
                sp_kutuAcildi.Stop(fxIds[8]);
                sp_kutuAciliyor.Stop(fxIds[9]);
                sp_kutuTiklandi.Stop(fxIds[10]);
                sp_yanlisCevap.Stop(fxIds[11]);
            }
            catch { }//kontrollü catch
        }
        public static void sesEfektiCal(string v)
        {
            titret(100);

            if (oAyarlariTBL_ != null)
            {
                if (oAyarlariTBL_.sesler)
                {
                    int fxID = 0;
                    Android.Media.SoundPool sp = null;

                    if (v == "basarimAcildi")
                    {
                        fxID = fxIds[0];
                        sp = sp_basarimAcildi;
                    }
                    else if (v == "bolumGecildi")
                    {
                        fxID = fxIds[1];
                        sp = sp_bolumGecildi;
                    }
                    else if (v == "butonTiklandi")
                    {
                        fxID = fxIds[2];
                        sp = sp_butonTiklandi;
                    }
                    else if (v == "harfAl")
                    {
                        fxID = fxIds[3];
                        sp = sp_harfAl;
                    }
                    else if (v == "ipucu")
                    {
                        fxID = fxIds[4];
                        sp = sp_ipucu;
                    }
                    else if (v == "kelimeAcildi")
                    {
                        fxID = fxIds[5];
                        sp = sp_kelimeAcildi;
                    }
                    else if (v == "kelimeAciliyor")
                    {
                        fxID = fxIds[6];
                        sp = sp_kelimeAciliyor;
                    }
                    else if (v == "klavyeBasildi")
                    {
                        fxID = fxIds[7];
                        sp = sp_klavyeBasildi;
                    }
                    else if (v == "kutuAcildi")
                    {
                        fxID = fxIds[8];
                        sp = sp_kutuAcildi;
                    }
                    else if (v == "kutuAciliyor")
                    {
                        fxID = fxIds[9];
                        sp = sp_kutuAciliyor;
                    }
                    else if (v == "kutuTiklandi")
                    {
                        fxID = fxIds[10];
                        sp = sp_kutuTiklandi;
                    }
                    else if (v == "yanlisCevap")
                    {
                        fxID = fxIds[11];
                        sp = sp_yanlisCevap;
                    }

                    if (sp != null)
                    {
                        sp.Stop(fxID);
                        sp.Play(fxID, 1, 1, 1, 0, 1);
                    }
                }
                else
                {
                    sesEfektiDurdur();
                }
            }
        }
        public static async Task muzikAyar(string durum)
        {
            if (durum == "aç")
            {
                oAyarlariTBL_.muzik = true;

                muzikBaslatDurdur();
            }
            else if (durum == "kapa")
            {
                oAyarlariTBL_.muzik = false;

                muzikBaslatDurdur();
            }
            await ayarKaydet();
        }

        public static async Task sesAyar(string durum)
        {
            if (durum == "aç")
            {
                oAyarlariTBL_.sesler = true;

            }
            else if (durum == "kapa")
            {
                oAyarlariTBL_.sesler = false;

            }
            await ayarKaydet();
        }

        public static async Task titresimAyar(string durum)
        {
            if (durum == "aç")
            {
                oAyarlariTBL_.titresim = true;

            }
            else if (durum == "kapa")
            {
                oAyarlariTBL_.titresim = false;

            }
            await ayarKaydet();
        }


        public static async Task yanlisKelimeSilAyar(string durum)
        {
            if (durum == "aç")
            {
                oAyarlariTBL_.yanlisKelimeSil = true;

            }
            else if (durum == "kapa")
            {
                oAyarlariTBL_.yanlisKelimeSil = false;

            }
            await ayarKaydet();
        }

        public static async Task ayarKaydet()
        {
            Dictionary<string, string> veriler = new Dictionary<string, string>{
                { "_d", "ayk"},
                { "_p0", googleProfile_.Id},
                { "_p1", oAyarlariTBL_.sesler.ToString()},
                { "_p2", oAyarlariTBL_.muzik.ToString()},
                { "_p3", oAyarlariTBL_.titresim.ToString()},
                { "_p4", oAyarlariTBL_.yanlisKelimeSil.ToString()}
            };

            var t = Task.Run(() => SendPostToURI(server_main_link, veriler));
            await t.ConfigureAwait(false);
            if (t.Result != "ok")
            {
                //veriler kaydedilemediyse
            }
            //fonks.mesajGoster(t.Result, this);

        }

        public static void mesajGoster(string msj, Action yap = null, bool iptalBtnAktif = false, Action yap2 = null, string buton1txt = "", string buton2txt = "")
        {
            try
            {
                c.RunOnUiThread(async delegate
                {
                    c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                    if (buton1txt == "") buton1txt = y.tamam_btn;
                    if (buton2txt == "") buton2txt = y.iptal_btn;

                    ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                    Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                    //loading çıkarıyoruz
                    ProgressBar loadingBar = new ProgressBar(c);
                    loadingBar.IndeterminateDrawable = LD(loadingBar.IndeterminateDrawable);
                    loadingBar.Indeterminate = true;
                    RelativeLayout rl = new RelativeLayout(c);
                    rl.SetGravity(GravityFlags.Center);
                    rl.AddView(loadingBar);
                    var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    anaView.AddView(rl, param);

                    await Task.Delay(300);

                    LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Android.Content.Context.LayoutInflaterService);
                    View mesajGosterL = inflater.Inflate(Resource.Layout.mesajGoster, null, false);

                    int width = LinearLayout.LayoutParams.WrapContent;
                    int height = LinearLayout.LayoutParams.WrapContent;
                    PopupWindow popupWindow = new PopupWindow(mesajGosterL, width, height);

                    popupWindow.Focusable = false;
                    popupWindow.OutsideTouchable = false;

                    View pcw = popupWindow.ContentView;


                    LinearLayout mesajGosterUstL = pcw.FindViewById<LinearLayout>(Resource.Id.mesajGosterUstL);
                    int _ = px(60);
                    mesajGosterUstL.SetPadding(_, _, _, _);

                    TextView mesajGosterTxt = pcw.FindViewById<TextView>(Resource.Id.mesajGosterTxt);
                    mesajGosterTxt.SetMaxLines(20);
                    mesajGosterTxt.MovementMethod = Android.Text.Method.ScrollingMovementMethod.Instance;
                    mesajGosterTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, px(50));
                    var p2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    p2.Gravity = GravityFlags.Center;
                    _ = px(20);
                    p2.SetMargins(_, _, _, _);
                    mesajGosterTxt.LayoutParameters = p2;
                    mesajGosterTxt.RequestLayout();

                    Button tmmBtn = pcw.FindViewById<Button>(Resource.Id.tmmBtn);
                    p2 = new LinearLayout.LayoutParams(px(400), px(100));
                    p2.Gravity = GravityFlags.Center;
                    p2.SetMargins(0, px(60), 0, 0);
                    tmmBtn.LayoutParameters = p2;
                    tmmBtn.RequestLayout();

                    Button iptalBtn = pcw.FindViewById<Button>(Resource.Id.iptalBtn);
                    p2 = new LinearLayout.LayoutParams(px(400), px(100));
                    p2.Gravity = GravityFlags.Center;
                    p2.SetMargins(0, px(30), 0, 0);
                    iptalBtn.LayoutParameters = p2;
                    iptalBtn.RequestLayout();


                    mesajGosterTxt.Text = msj;


                    tmmBtn.Text = buton1txt;
                    tmmBtn.Click += delegate
                    {
                        sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();

                        if (yap != null) yap();
                    };

                    if (iptalBtnAktif)
                    {
                        iptalBtn.Text = buton2txt;
                        iptalBtn.Visibility = ViewStates.Visible;
                        iptalBtn.Click += delegate
                        {
                            sesEfektiCal("butonTiklandi");

                            popupWindow.Dismiss();

                            if (yap2 != null) yap2();
                        };
                    }

                    popupWindow.DismissEvent += delegate
                    {
                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);
                        f.UnbindDrawables(pcw);
                        c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                        BuPencereKapandi(popupWindow);
                    };


                    popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                    BuPencereAcildi(popupWindow);
                });
            }
            catch (Exception ex)
            {
                if (logRecording)
                {
                    string e = y.p(y.hataOlustu, ex);
                    main_log += e + "\n\n";
                }

                ekranGecisi(typeof(anaEkran));
                c.Finish();
                return;
            }
        }

        //önbelleğe alma problemleri ile uygulamanın çökmemesi için her sayfa geçişinde görsel öğelerde manuel temizlik yapıyoruz
        public static void gorselleriTemizle()
        {
            try
            {
                /*ViewGroup anaView = c.Window.DecorView.RootView as ViewGroup;

                if (c is acilisEkrani)
                {
                    ImageView TFpng = c.FindViewById<ImageView>(Resource.Id.TFpng);
                    TFpng.SetImageBitmap(null);
                    TFpng.Dispose();
                }
                else if (c is anaEkran)
                {
                    LinearLayout aL = c.FindViewById<LinearLayout>(Resource.Id.anaEkranL);
                    aL.SetBackgroundResource(Resource.Color.mtrl_btn_transparent_bg_color);
                }
                else if (c is bolumler)
                {
                    ImageView bolumGorseli = c.FindViewById<ImageView>(Resource.Id.bolumGorseli);
                    bolumGorseli.SetImageBitmap(null);
                    bolumGorseli.Dispose();
                }
                else if (c is bulmacaEkrani)
                {
                    ImageView soruGorseli = c.FindViewById<ImageView>(Resource.Id.soruGorseli);
                    soruGorseli.SetImageBitmap(null);
                    soruGorseli.Dispose();
                }

                if (c is bolumler || c is bulmacaEkrani)
                {
                    ImageView profilGorseli = anaView.FindViewWithTag("profilGorseli") as ImageView;
                    profilGorseli.SetImageBitmap(null);
                    profilGorseli.Dispose();
                }

                f.UnbindDrawables(anaView);*/
                f.UnbindDrawables(c.Window.DecorView);
            }
            catch { }
        }

        public static void temizlikYap()
        {
            try
            {
                Java.Lang.JavaSystem.Gc();
                Java.Lang.Runtime.GetRuntime().Gc();
                Java.Lang.JavaSystem.RunFinalization();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }
            catch { }//kontrollü catch
        }

        public static void ekranGecisi(Type ekran, params veriGonder[] ekVeriler)
        {
            c.Window.TransitionBackgroundFadeDuration = 0;
            Intent intent = new Intent(c, typeof(yukleniyor));
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            intent.PutExtra("gecilecekEkran", JsonConvert.SerializeObject(ekran));
            if (ekVeriler.Length != 0)
            {
                intent.PutExtra("ekVeriler", JsonConvert.SerializeObject(ekVeriler));
            }

            f.gorselleriTemizle();

            if (c != null)
                c.FinishAffinity();

            c.StartActivity(intent);
            c.OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            c.FinishAfterTransition();
        }

        public static Bitmap yenidenBoyutlandir(Bitmap originalImage, int widthToScale, int heightToScale)
        {
            Bitmap resizedBitmap = Bitmap.CreateBitmap(widthToScale, heightToScale, Bitmap.Config.Argb8888);

            float originalWidth = originalImage.Width;
            float originalHeight = originalImage.Height;

            Canvas canvas = new Canvas(resizedBitmap);

            float scale = widthToScale / originalWidth;

            float xTranslation = 0.0f;
            float yTranslation = (heightToScale - originalHeight * scale) / 2.0f;

            Matrix transformation = new Matrix();
            transformation.PostTranslate(xTranslation, yTranslation);
            transformation.PreScale(scale, scale);

            Paint paint = new Paint();
            paint.FilterBitmap = true;

            canvas.DrawBitmap(originalImage, transformation, paint);

            //boyutunu da yarı yarıya küçültüyoruz
            resizedBitmap = boyutKucult(resizedBitmap);

            return resizedBitmap;
        }

        public static Bitmap boyutKucult(Bitmap b, int kucultulmeYuzdesi = 25)
        {
            Bitmap cB = null;

            byte[] compressedData = null;
            using (var stream = new System.IO.MemoryStream())
            {
                b.Compress(Bitmap.CompressFormat.Jpeg, 100 - kucultulmeYuzdesi, stream);
                compressedData = stream.ToArray();
            }

            using (var stream = new System.IO.MemoryStream(compressedData))
            {
                cB = BitmapFactory.DecodeStream(stream);
            }

            compressedData = null;

            return cB;
        }


    }


}