using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.OS;
using Android.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Newtonsoft.Json;

namespace gorbul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.Bulmaca", MainLauncher = false, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class bulmacaEkrani : AppCompatActivity
    {
        private const int belirlenenGorselSahneSiniri = 4, belirlenenGorselSaneSuresi = 120;
        private static ArrayList bilinenHarfler = new ArrayList();
        private ArrayList satirdakiKutuIDleri = new ArrayList();
        private ArrayList kutuIDsiHangiSatiraAit = new ArrayList();
        private static ArrayList acilanHarfler = new ArrayList();
        private static System.Timers.Timer gorselSahneSayaci, toplamSureSayaci;
        private string orjinalCevap, sonSeciliKutuID, klavyeKomutu, orjinalSoru;
        private char[] tumHarfler;
        private static int soruID, gorselSahneSuresi = belirlenenGorselSaneSuresi, suAnkiGorselSahneSuresi, gorselSahneSayisi = 1,
            toplamHarfSayisi, toplamSure = 0, toplamPuan = 0, yanlisDenemeSayisi = 0, harfAlmaSayisi = 0, toplamSatirSayisi = 0,
            uzaklastirmaSayisi = 0, gorselSahneSiniri = belirlenenGorselSahneSiniri;
        private static bool bolumGecildi = false, yanlisCevapVerildi = false, suAnkiKlavyeDurumu = true, kelimeAcildi = false, soruIstatistikEklendi = false;
        private string[] tuslar = new string[3];
        private Bitmap[] gorseller;
        private string bolumAdi = "";
        private static bool isleniyor = false;
        private int bolumSonuBeklemeSuresi = 2000, bolumlerdenGelenSoruID;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            f.temizlikYap();
        }
        protected override void OnResume()
        {
            base.OnResume();
            f.ortakAyarlar(Window);
        }
        public async override void OnBackPressed()
        {
            //işlenme anlarında geri dönüş deaktif ediliyor
            if (isleniyor) return;

            await geriCik();
        }

        public static async Task<bool> geriCik()
        {
            if (isleniyor) return false;

            gorselSahneSayaci.Stop();
            toplamSureSayaci.Stop();

            await soruIstatistikGuncelle("geriCik");

            if (f.gecisReklamiGosterilecek)
            {
                f.gecisReklamiGosterVeGec(-1);
            }
            else
            {
                f.ekranGecisi(typeof(bolumler));
                f.c.Finish();
            }

            return true;
        }

        //Ekran döndürülünce görseli tam ekran yap
        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            try
            {
                FrameLayout gorselCevirFL = FindViewById<FrameLayout>(Resource.Id.gorselCevirFL);

                ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);

                ViewGroup anaView = Window.DecorView.RootView as ViewGroup;

                RelativeLayout ustMenu = anaView.FindViewWithTag("ustMenu") as RelativeLayout;

                if (newConfig.Orientation == Android.Content.Res.Orientation.Portrait)
                {
                    ustMenu.Visibility = ViewStates.Visible;

                    var _1f =
                        new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                    gorselCevirFL.LayoutParameters = _1f;


                    soruGorseli.SetBackgroundColor(Color.Transparent);

                    int soruGorseli_height = f.px(500);

                    //ekran yüksekiği belli yüksekliklerden sonra belli sayıya göre belirlenir (3te biri gibi)
                    if (f.ekranYuksekligi > 1920) soruGorseli_height = Convert.ToInt32(Math.Round(f.ekranYuksekligi / 3));
                    if (f.ekranYuksekligi > 2400 && f.ekranGenisligi > 1440) soruGorseli_height = Convert.ToInt32(Math.Round(f.ekranYuksekligi / 2.5));

                    var _1 =
                        new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, soruGorseli_height);
                    _1.Gravity = GravityFlags.NoGravity;
                    soruGorseli.LayoutParameters = _1;
                }
                else if (newConfig.Orientation == Android.Content.Res.Orientation.Landscape)
                {
                    f.acikPencereleriKapat();

                    ustMenu.Visibility = ViewStates.Gone;

                    var _2f =
                        new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    gorselCevirFL.LayoutParameters = _2f;


                    var _2 =
                        new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    _2.Gravity = GravityFlags.Center;
                    soruGorseli.SetBackgroundColor(r.soruGorseliYanEkranArkaplanR);
                    soruGorseli.LayoutParameters = _2;

                }
            }
            catch (Exception ex)
            {
                f.log(ex);
            }
        }

        //uygulama arkaplana alınırken verileri güncelliyoruz
        protected async override void OnPause()
        {
            base.OnPause();

            await soruIstatistikGuncelle("geriCik");
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.bulmacaEkrani);

                f.c = this;

                bolumlerdenGelenSoruID = Convert.ToInt32(JsonConvert.DeserializeObject<string>(Intent.GetStringExtra("soruID")));

                //eğer soruID son soru id'sinden büyükse bolumlere dön
                if (f.sorularTBL_ != null)
                {
                    if (bolumlerdenGelenSoruID > f.sorularTBL_[f.sorularTBL_.Length - 1].id)
                    {
                        f.ekranGecisi(typeof(bolumler));
                        f.c.Finish();
                    }
                }

                //static yapıldıktan sonra problem çıkmaması için her girişte staticlerin hepsini sıfırlıyoruz
                bilinenHarfler = new ArrayList();
                acilanHarfler = new ArrayList();
                gorselSahneSayaci = null;
                toplamSureSayaci = null;
                soruID = 0;
                gorselSahneSuresi = belirlenenGorselSaneSuresi;

                //süre ilk 20 bölümde 30 saniye,
                //sonraki 40 bölüm (yani 60. soruya kadar) 60 saniye,
                //sonraki 90 bölüm (yani 150. soruya kadar) 120 saniye,
                //150. sorudan sonraki bölümler süresiz (sadece butonla yakınlaştırma yapılabilir)
                if (soruID <= 20) gorselSahneSuresi = 30;
                else if (soruID <= 60) gorselSahneSuresi = 60;
                else if (soruID <= 150) gorselSahneSuresi = 120;
                else if (soruID > 150)
                {
                    gorselSahneSuresi = 0;
                }

                suAnkiGorselSahneSuresi = gorselSahneSuresi;

                gorselSahneSayisi = 1;
                toplamHarfSayisi = 0;
                toplamSure = 0;
                toplamPuan = 0;
                yanlisDenemeSayisi = 0;
                harfAlmaSayisi = 0;
                toplamSatirSayisi = 0;
                uzaklastirmaSayisi = 0;
                gorselSahneSiniri = belirlenenGorselSahneSiniri;
                bolumGecildi = false;
                yanlisCevapVerildi = false;
                suAnkiKlavyeDurumu = true;
                kelimeAcildi = false;
                soruIstatistikEklendi = false;
                isleniyor = false;

                if (RequestedOrientation != Android.Content.PM.ScreenOrientation.Portrait)
                    RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

                f.boyutlariOlustur("bulmacaEkrani");

                f.ortakAyarlar(Window);

                f.yukleniyorOlustur();

                bulmacaEkraniYukle();
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }


        async void bulmacaEkraniYukle()
        {
            try
            {
                if (Intent.GetStringExtra("soruID") == null)
                {
                    f.anaEkranaDonVeMesajGoster(y.hataOlustu);
                    return;
                }

                int suAnkiSayfa = 0;
                suAnkiSayfa = (int)Math.Ceiling(((double)bolumlerdenGelenSoruID / (double)bolumler.bolumBasinaSoruSayisi));

                foreach (var bolum in f.bolumlerTBL_)
                {
                    if (bolum.id == suAnkiSayfa) bolumAdi = bolum.bolumAdi;
                }

                //Üst menüyü oluşturuyoruz
                f.ustMenuOlustur("bulmaca", bolumlerdenGelenSoruID, bolumAdi);

                //başarımları kontrol ediyoruz
                bool basarimKontrolR = await f.basarimKontrol();
                f.basarimKontrolUI(basarimKontrolR);

                await Task.Delay(1);

                foreach (sorularTBL x in f.sorularTBL_)
                {
                    if (x.id == bolumlerdenGelenSoruID)
                    {
                        soruID = x.id;
                        orjinalSoru = x.soru;
                        orjinalCevap = x.cevap;
                        gorselSahneSiniri = x.gorselSayisi;
                        break;
                    }
                }

                if (soruID == 0)
                {
                    f.anaEkranaDonVeMesajGoster(y.hataOlustu);
                    return;
                }

                //önceki bölümlerin hepsi geçilmişse devam, değilse dur
                if (!await f.oncekiBolumlerGecildiMi(soruID))
                {
                    f.anaEkranaDonVeMesajGoster(y.oncekiBolumlerGecilmemis);
                    return;
                }

                //bölüm ayarlarını güncellenmiş olma ihtimalina karşın bu soruID'sine ait TEK kaydı tekrar çekiyoruz
                if (!await f.bolumAyarlariCek(soruID))
                {
                    f.anaEkranaDonVeMesajGoster(y.bolumlerYuklenemedi);
                    return;
                }

                //Şu anki soru görselini çekiyoruz 1080x500px
                await gorselSahnesiAtla();

                try
                {
                    ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                    bool hasDrawable = (soruGorseli.Drawable != null);
                    if (!hasDrawable)
                    {
                        f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                        return;
                    }
                }
                catch
                {
                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                    return;
                }

                await Task.Delay(1);

                Button harfAlBtn = FindViewById<Button>(Resource.Id.harfAlBtn);
                harfAlBtn.Text = y.harfAc_btn;
                Button uzaklastirBtn = FindViewById<Button>(Resource.Id.uzaklastirBtn);
                uzaklastirBtn.Text = y.ipucu_btn;
                Button kelimeyiAcBtn = FindViewById<Button>(Resource.Id.kelimeyiAcBtn);
                kelimeyiAcBtn.Text = y.kelimeyiAc_btn;

                //Sorunun bulunacağı TextView'ı çekiyoruz
                TextView soru = FindViewById<TextView>(Resource.Id.soru);
                soru.Text = orjinalSoru;

                string cevap = orjinalCevap;

                //Cevabı büyük harfe çeviriyoruz
                cevap = cevap.ToUpper(new System.Globalization.CultureInfo(y.globalization_CultureInfo, false));

                //Cevabı boşluktan satırlara bölüyoruz
                string[] cevapSatiri = cevap.Split(" ");

                //Cevaptaki boşlukları siliyoruz
                cevap = cevap.Replace(" ", "");

                tumHarfler = cevap.ToCharArray();

                toplamHarfSayisi = cevap.Length;

                int suAnkiGenelSira = 0;
                int suAnkiSatir = 0;
                foreach (string satir in cevapSatiri)
                {
                    //Her satır için kelimesini atıyoruz
                    cevap = satir;

                    //Cevaptaki harf sayısını belirliyoruz
                    int harfSayisi = cevap.Length;

                    //Cevabı harflerine ayırıyoruz
                    char[] harfler = cevap.ToCharArray();

                    //Cevaptaki harf sayısı kadar textBox oluşturmak için kapsayıcı üst layout alanı belirliyoruz (div gibi)
                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari); //root layout

                    //Hangi satira eklenecekse o satiri belirliyoruz
                    LinearLayout cevapSatiriLayout = cevapKutulari.FindViewWithTag(suAnkiSatir.ToString()) as LinearLayout;
                    cevapSatiriLayout.SetPadding(0, 0, 0, f.px(20));

                    var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    param.SetMargins(f.px(10), 0, f.px(10), 0);


                    //Cevaptaki harf sayısı kadar textBox cloneluyoruz
                    for (int i = 0; i < harfSayisi; i++)
                    {
                        int harfID = i;
                        //Cevaptaki harf sayısı kadar textBox oluşturmak için textbox özelliklerini belirliyoruz
                        EditText cvp = new EditText(this);
                        cvp.Tag = suAnkiSatir + "," + harfID;//textboxun idsi örneğin başındaki sayı ile su anki satırı 2. ile de kendi içinde hangi nesne olduğunu çözüyoruz "1,1", "1,2" "2,1"
                        cvp.Text = "";
                        cvp.SetWidth(f.px(100));
                        cvp.SetHeight(f.px(100));
                        cvp.SetPadding(f.px(20), f.px(10), f.px(20), f.px(10));
                        cvp.TextAlignment = TextAlignment.Center;
                        cvp.SetTextColor(r.kutuYaziR);
                        cvp.SetBackgroundColor(r.kutuArkaplanR);
                        cvp.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(60));
                        cvp.SetMaxLines(1);
                        int maxLength = 1;
                        cvp.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(maxLength) });
                        cvp.SetTypeface(cvp.Typeface, TypefaceStyle.Bold);
                        cvp.SetSingleLine(true);
                        cvp.SetSelectAllOnFocus(true);
                        cvp.SetRawInputType(InputTypes.Null);
                        cvp.Focusable = true;
                        cvp.FocusableInTouchMode = true;
                        cvp.ShowSoftInputOnFocus = false;
                        cvp.LongClickable = false;
                        cvp.SetHighlightColor(Color.Transparent);

                        cevapSatiriLayout.AddView(cvp, param);

                        EditText editText = cvp;

                        editText.Touch += (sender, args) =>
                        {
                            if ((args as TextView.TouchEventArgs).Event.Action == MotionEventActions.Down)
                            {
                                if (editText.Enabled)
                                {
                                    f.sesEfektiCal("kutuTiklandi");

                                    kutuSec(cevapKutulari, editText);

                                    sonSeciliKutuID = editText.Tag.ToString();
                                }
                            }
                        };

                        satirdakiKutuIDleri.Add(harfID);
                        kutuIDsiHangiSatiraAit.Add(suAnkiSatir);
                        suAnkiGenelSira++;
                    }


                    suAnkiSatir++;
                    toplamSatirSayisi++;
                }

                //Şimdi harf alma butonunu işlevsel hale getiriyoruz
                harfAlBtn.Click += async (object sender, EventArgs e) =>
                {
                    if (f.c is bulmacaEkrani)
                    {
                        harfAlBtn.Enabled = false;
                        f.c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        isleniyor = true;

                        //hızlı ses tepkisi almak için veritabanı doğrulamasını beklemeden eldeki veri üzerinden kontrol yapılıyor (sadece ses için)
                        bool caldi = false;
                        if (f.jetonlarTBL_ != null)
                        {
                            if (f.jetonlarTBL_.jetonSayisi >= f.harfAlJetonUcreti)
                            {
                                f.sesEfektiCal("harfAl");
                                caldi = true;
                            }
                        }
                        if (!caldi) f.sesEfektiCal("butonTiklandi");

                        //jeton kontrolü ve jeton harcama
                        var t4 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=hjt&_j=hal&_g=" + f.googleProfile_.Id));
                        await t4.ConfigureAwait(true);
                        if (t4.Result != null)
                        {
                            ViewGroup anaView = Window.DecorView.RootView as ViewGroup;
                            TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                            if (t4.Result == "ok")
                            {
                                //Jeton bilgileri çekiliyor
                                var t5 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=jt&_g=" + f.googleProfile_.Id));
                                await t5.ConfigureAwait(true);
                                if (t5.Result != null) f.jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                                else
                                {
                                    f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                    return;
                                }


                                await harfAl();


                                ustJetonTxt.Text = f.jetonlarTBL_.jetonSayisi + "";

                                f.c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                                harfAlBtn.Enabled = true;


                                isleniyor = false;
                            }
                            else if (t4.Result.IndexOf("yjy_") >= 0)
                            {
                                int guncelJeton = Convert.ToInt32(t4.Result.Replace("yjy_", ""));

                                //jeton miktarını günceliyle yeniliyoruz
                                ustJetonTxt.Text = guncelJeton + "";

                                isleniyor = false;

                                f.mesajGoster(y.jetonYok, () =>
                                {
                                    f.magazaPenceresiniAc();
                                    harfAlBtn.Enabled = true;
                                }, true, () =>
                                {
                                    harfAlBtn.Enabled = true;
                                }, y.magazaAc_btn, y.kapat_btn);
                            }
                        }
                        else
                        {
                            f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                            return;
                        }
                    }
                };


                //Şimdi görseli uzaklaştır butonunu işlevsel hale getiriyoruz
                uzaklastirBtn.Click += async (object sender, EventArgs e) =>
                {
                    if (f.c is bulmacaEkrani)
                    {
                        uzaklastirBtn.Enabled = false;
                        f.c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        isleniyor = true;

                        //hızlı ses tepkisi almak için veritabanı doğrulamasını beklemeden eldeki veri üzerinden kontrol yapılıyor (sadece ses için)
                        bool caldi = false;
                        if (f.jetonlarTBL_ != null)
                        {
                            if (f.jetonlarTBL_.jetonSayisi >= f.ipucuJetonUcreti)
                            {
                                f.sesEfektiCal("ipucu");
                                caldi = true;
                            }
                        }
                        if (!caldi) f.sesEfektiCal("butonTiklandi");


                        //uzaklaştırma sayısı sondaysa uzaklaştırma yapmıyoruz
                        if (uzaklastirmaSayisi != (gorselSahneSiniri - 2))
                        {
                            //jeton kontrolü ve jeton harcama
                            var t4 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=hjt&_j=ipg&_g=" + f.googleProfile_.Id));
                            await t4.ConfigureAwait(true);
                            if (t4.Result != null)
                            {
                                ViewGroup anaView = Window.DecorView.RootView as ViewGroup;
                                TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;
                                if (t4.Result == "ok")
                                {
                                    //Jeton bilgileri çekiliyor
                                    var t5 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=jt&_g=" + f.googleProfile_.Id));
                                    await t5.ConfigureAwait(true);
                                    if (t5.Result != null) f.jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                                    else
                                    {
                                        f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                        return;
                                    }


                                    await gorseliUzaklastir(uzaklastirBtn);


                                    ustJetonTxt.Text = f.jetonlarTBL_.jetonSayisi + "";


                                    isleniyor = false;
                                    f.c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                                }
                                else if (t4.Result.IndexOf("yjy_") >= 0)
                                {
                                    int guncelJeton = Convert.ToInt32(t4.Result.Replace("yjy_", ""));

                                    //jeton miktarını günceliyle yeniliyoruz
                                    ustJetonTxt.Text = guncelJeton + "";

                                    isleniyor = false;

                                    f.mesajGoster(y.jetonYok, () =>
                                    {
                                        f.magazaPenceresiniAc();
                                        uzaklastirBtn.Enabled = true;
                                    }, true, () =>
                                    {
                                        uzaklastirBtn.Enabled = true;
                                    }, y.magazaAc_btn, y.kapat_btn);
                                }
                            }
                            else
                            {
                                f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                return;
                            }
                        }
                        else
                        {
                            isleniyor = false;
                            f.c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                        }

                    }
                };


                //Şimdi kelimeyi aç butonunu işlevsel hale getiriyoruz
                kelimeyiAcBtn.Click += async (object sender, EventArgs e) =>
                {
                    if (f.c is bulmacaEkrani)
                    {
                        kelimeyiAcBtn.Enabled = false;
                        f.c.Window.AddFlags(WindowManagerFlags.NotTouchable);
                        isleniyor = true;

                        //hızlı ses tepkisi almak için veritabanı doğrulamasını beklemeden eldeki veri üzerinden kontrol yapılıyor (sadece ses için)
                        bool caldi = false;
                        if (f.jetonlarTBL_ != null)
                        {
                            if (f.jetonlarTBL_.jetonSayisi >= f.kelimeAcJetonUcreti)
                            {
                                f.sesEfektiCal("kelimeAcildi");
                                caldi = true;
                            }
                        }
                        if (!caldi) f.sesEfektiCal("butonTiklandi");

                        //jeton kontrolü ve jeton harcama
                        var t4 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=hjt&_j=kla&_g=" + f.googleProfile_.Id));
                        await t4.ConfigureAwait(true);
                        if (t4.Result != null)
                        {
                            ViewGroup anaView = Window.DecorView.RootView as ViewGroup;
                            TextView ustJetonTxt = anaView.FindViewWithTag("ustJetonTxt") as TextView;

                            if (t4.Result == "ok")
                            {
                                //Jeton bilgileri çekiliyor
                                var t5 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=jt&_g=" + f.googleProfile_.Id));
                                await t5.ConfigureAwait(true);
                                if (t5.Result != null) f.jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t5.Result);
                                else
                                {
                                    f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                                    return;
                                }

                                ustJetonTxt.Text = f.jetonlarTBL_.jetonSayisi + "";

                                kelimeAcildi = true;
                                await gecilmeyiKaydet();
                                await kelimeyiAcmaAnimasyonu();
                            }
                            else if (t4.Result.IndexOf("yjy_") >= 0)
                            {
                                isleniyor = false;
                                f.c.Window.ClearFlags(WindowManagerFlags.NotTouchable);

                                int guncelJeton = Convert.ToInt32(t4.Result.Replace("yjy_", ""));

                                //jeton miktarını günceliyle yeniliyoruz
                                ustJetonTxt.Text = guncelJeton + "";

                                f.mesajGoster(y.jetonYok, () =>
                                {
                                    f.magazaPenceresiniAc();
                                    kelimeyiAcBtn.Enabled = true;
                                }, true, () =>
                                {
                                    kelimeyiAcBtn.Enabled = true;
                                }, y.magazaAc_btn, y.kapat_btn);
                            }
                        }
                        else
                        {
                            f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                            return;
                        }
                    }
                };

                klavyeOlustur();

                await Task.Delay(1);

                ImageView gorselCevir = FindViewById<ImageView>(Resource.Id.gorselCevir);
                //ekran düzse yan çevir yansa düz çevir
                gorselCevir.Touch += (sender, args) =>
                {
                    //sadece basma durumunda çalışmalı, diğer türlü tüm durumlarda çalışıyor up, move gibi
                    if ((args as ImageView.TouchEventArgs).Event.Action == MotionEventActions.Down)
                    {
                        if (RequestedOrientation == Android.Content.PM.ScreenOrientation.Portrait)
                            RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
                        else RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
                    }
                };


                //tekrar başlatma seçeneklerini oluşturuyoruz
                Button tekrarBaslatBtn = FindViewById<Button>(Resource.Id.tekrarBaslatBtn);

                tekrarBaslatBtn.Click += async (s, a) =>
                {
                    f.sesEfektiCal("butonTiklandi");

                    bolumGecildi = false;
                    bilinenHarfler.Clear();
                    acilanHarfler.Clear();
                    toplamSure = 0;
                    yanlisDenemeSayisi = 0;
                    gorselSahneSayisi = 1;
                    suAnkiGorselSahneSuresi = gorselSahneSuresi;
                    uzaklastirmaSayisi = 0;
                    harfAlmaSayisi = 0;
                    kelimeAcildi = false;
                    toplamPuan = 0;


                    if (soruID > 150)
                    {
                        TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                        gorselSuresiTxt.Text = y.geriSayimKapali;
                        TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                        gorselSayisiTxt.Text = y.p(y.gorselSayisi, gorselSahneSayisi);

                        uzaklastirBtn.SetTextColor(r.butonAktifYaziR);
                        uzaklastirBtn.Enabled = true;
                    }


                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);

                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        haT.Text = "";
                        haT.SetTextColor(r.kutuYaziR);
                        haT.SetBackgroundColor(r.kutuArkaplanR);
                        haT.Enabled = true;
                    }


                    toplamSureSayaci.Start();

                    if (soruID <= 150)
                        gorselSahneSayaci.Start();

                    butonlarinDurumunuDegistir(true);
                    klavyeDurumuDegistir(true);

                    ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                    soruScroll.Enabled = true;

                    await gorselSahnesiAtla();

                    try
                    {
                        ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                        bool hasDrawable = (soruGorseli.Drawable != null);
                        if (!hasDrawable)
                        {
                            f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                            return;
                        }
                    }
                    catch
                    {
                        f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                        return;
                    }

                    LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye);
                    klavye.Visibility = ViewStates.Visible;

                    LinearLayout tekrarLayout = FindViewById<LinearLayout>(Resource.Id.tekrarLayout);
                    tekrarLayout.Visibility = ViewStates.Gone;
                };

                Button tekrarBolumlereDon = FindViewById<Button>(Resource.Id.tekrarBolumlereDon);
                //tekrarBolumlereDon.Text = "Bölümlere Dön";

                tekrarBolumlereDon.Click += async (s, a) =>
                {
                    f.sesEfektiCal("butonTiklandi");

                    await geriCik();
                };


                Button tekrarSonrakiBolum = FindViewById<Button>(Resource.Id.tekrarSonrakiBolum);
                tekrarSonrakiBolum.Click += delegate
                {
                    f.sesEfektiCal("butonTiklandi");

                    Window.AddFlags(WindowManagerFlags.NotTouchable);

                    f.gecisReklamiGosterVeGec(soruID + 1);
                };


                //sayaçları eski veriler çekilmeden önce oluşturuyoruz
                toplamSureSayaci = new System.Timers.Timer();
                toplamSureSayaci.Interval = 1000;

                gorselSahneSayaci = new System.Timers.Timer();
                gorselSahneSayaci.Interval = 1000;


                //Eski veriler boş değilse tekrar çekiyoruz
                if (f.soruIstatistikTBL_ != null)
                {
                    soruIstatistikTBL sri = f.soruIstatistikTBL_;

                    bolumGecildi = sri.bolumGecildi;

                    foreach (string bilinenHarf in sri.hangiHarflerAcildi.Split("|"))
                    {
                        if (bilinenHarf != "") bilinenHarfler.Add(bilinenHarf);
                    }

                    foreach (string acilanharf in sri.acilanHarfler.Split("|"))
                    {
                        if (acilanharf != "") acilanHarfler.Add(Convert.ToInt32(acilanharf));
                    }

                    toplamSure = sri.toplamSure;
                    yanlisDenemeSayisi = sri.yanlisDenemeSayisi;
                    gorselSahneSayisi = sri.gorselSahneSayisi;
                    suAnkiGorselSahneSuresi = sri.suAnkiGorselSahneSuresi;
                    uzaklastirmaSayisi = sri.uzaklastirmaSayisi;
                    harfAlmaSayisi = sri.harfAlmaSayisi;
                    kelimeAcildi = sri.kelimeAcildi;
                    toplamPuan = sri.toplamPuan;

                    if (gorselSahneSayisi != 1)
                    {
                        await gorselSahnesiAtla();

                        try
                        {
                            ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                            bool hasDrawable = (soruGorseli.Drawable != null);
                            if (!hasDrawable)
                            {
                                f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                                return;
                            }
                        }
                        catch
                        {
                            f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                            return;
                        }
                    }

                    if (gorselSahneSayisi == (gorselSahneSiniri - 1))
                    {
                        gorselHakkiBitti(uzaklastirBtn);
                    }

                    //Daha önce bitirilen harfleri gösteriyoruz
                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);

                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        if (bilinenHarfler.IndexOf(satirID + "," + kutuID) != -1)
                        {
                            haT.Text = tumHarfler[i].ToString();
                            haT.Enabled = false;
                            haT.SetTextColor(r.harfAlYaziR);
                            haT.SetBackgroundColor(r.harfAlArkaplanR);
                        }
                    }



                    //Bölüm daha önce geçildiyse her şeyi deaktif ediyoruz ve butonları gösteriyoruz
                    if (sri.bolumGecildi)
                    {
                        toplamSureSayaci.Stop();

                        bolumGecildi = sri.bolumGecildi;

                        if (soruID <= 150)
                            gorselSahneSayaci.Stop();

                        butonlarinDurumunuDegistir(false);
                        klavyeDurumuDegistir(false);

                        ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                        soruScroll.Enabled = false;

                        await gorselSahnesiAtla(true);

                        try
                        {
                            ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                            bool hasDrawable = (soruGorseli.Drawable != null);
                            if (!hasDrawable)
                            {
                                f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                                return;
                            }
                        }
                        catch
                        {
                            f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                            return;
                        }

                        //Daha önce bitirilen tüm harfleri gösteriyoruz
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                            haT.Text = tumHarfler[i].ToString();
                            haT.Enabled = false;
                            if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                            {
                                if (kelimeAcildi)
                                {
                                    haT.SetTextColor(r.kelimeAcYaziR);
                                    haT.SetBackgroundColor(r.kelimeAcArkaplanR);
                                }
                                else
                                {
                                    haT.SetTextColor(r.bolumGecYaziR);
                                    haT.SetBackgroundColor(r.bolumGecArkaplanR);
                                }
                            }
                            else
                            {
                                haT.SetTextColor(r.harfAlYaziR);
                                haT.SetBackgroundColor(r.harfAlArkaplanR);
                            }
                        }

                        //klavyeyi gizliyoruz
                        LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye);
                        klavye.Visibility = ViewStates.Gone;

                        //tekrar başlatma seçeneklerini yüklüyoruz
                        LinearLayout tekrarLayout = FindViewById<LinearLayout>(Resource.Id.tekrarLayout);
                        tekrarLayout.Visibility = ViewStates.Visible;
                    }
                }


                //İşlemler tamamlandığında gorsel sahne süresi geri saymaya başlıyor
                gorselSahneSayaci.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    RunOnUiThread(async () =>
                    {
                        uzaklastirBtn.Enabled = false;
                        uzaklastirBtn.SetTextColor(r.butonDeaktifYaziR);

                        if (suAnkiGorselSahneSuresi == 0)
                        {
                            if (gorselSahneSayisi != (gorselSahneSiniri - 1))
                            {
                                gorselSahneSayisi++;
                                //Burada görsel sahnesini atlıyoruz ve bir tık kolaylaştırıyoruz
                                await gorselSahnesiAtla();

                                try
                                {
                                    ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                                    bool hasDrawable = (soruGorseli.Drawable != null);
                                    if (!hasDrawable)
                                    {
                                        f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                                        return;
                                    }
                                }
                                catch
                                {
                                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                                    return;
                                }

                                suAnkiGorselSahneSuresi = gorselSahneSuresi;
                            }
                            else
                            {
                                suAnkiGorselSahneSuresi = 0;
                                gorselSahneSayaci.Stop();
                                gorselSahneSayaci.Enabled = false;
                            }

                            await soruIstatistikGuncelle("sahneBitti");
                        }


                        if (gorselSahneSayisi == (gorselSahneSiniri - 1))
                        {
                            gorselHakkiBitti(uzaklastirBtn);
                        }
                        else if (gorselSahneSayisi < (gorselSahneSiniri - 1))
                        {
                            TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                            gorselSuresiTxt.Text = y.p(y.snSonraIpucu, suAnkiGorselSahneSuresi);
                            TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                            gorselSayisiTxt.Text = y.p(y.gorselSayisi, gorselSahneSayisi);

                            if (uzaklastirmaSayisi != (gorselSahneSiniri - 1))
                            {
                                if (!bolumGecildi)
                                {
                                    uzaklastirBtn.SetTextColor(r.butonAktifYaziR);
                                    uzaklastirBtn.Enabled = true;
                                }
                            }
                        }

                        if (suAnkiGorselSahneSuresi > 0)
                            suAnkiGorselSahneSuresi--;

                    });
                };
                //bölüm geçilmediyse ve bölüm 150den düşükse başlat
                if (!bolumGecildi && soruID <= 150)
                {
                    gorselSahneSayaci.Start();
                }

                //150. bölümden sonrası için geri sayım ipucunu kapatıyoruz
                if (!bolumGecildi && soruID > 150)
                {
                    TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                    gorselSuresiTxt.Text = y.geriSayimKapali;
                    TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                    gorselSayisiTxt.Text = y.p(y.gorselSayisi, gorselSahneSayisi); ;
                }


                //İşlemler tamamlandığında gorsel sahne süresi geri saymaya başlıyor
                toplamSureSayaci.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    RunOnUiThread(() =>
                    {
                        toplamSure++;
                    });
                };
                //bölüm geçilmediyse başlat
                if (!bolumGecildi) toplamSureSayaci.Start();


                f.yukleniyorKaldir();


                RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;


                f.PlayGames__.Yukle();
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }


        void kutuSec(LinearLayout cevapKutulari, EditText kutu)
        {
            try
            {
                if (sonSeciliKutuID != null)
                {
                    //önceki seçili kutunun boyasını eski haline getiriyoruz
                    string[] bolx = sonSeciliKutuID.Split(',');
                    string satirIDx = bolx[0];

                    LinearLayout csLl = cevapKutulari.FindViewWithTag(satirIDx) as LinearLayout;
                    EditText haTl = csLl.FindViewWithTag(sonSeciliKutuID) as EditText;

                    if (haTl.Enabled)
                    {
                        haTl.SetTextColor(r.kutuYaziR);
                        haTl.SetBackgroundColor(r.kutuArkaplanR);
                    }
                }

                if (kutu.Enabled)
                {
                    kutu.SetTextColor(r.kutuSeciliYaziR);
                    kutu.SetBackgroundColor(r.kutuSeciliArkaplanR);
                }
            }
            catch (Exception ex)
            {
                f.log(ex);
            }
        }

        public void gorselHakkiBitti(Button uzaklastirBtn)
        {
            try
            {
                TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                gorselSuresiTxt.Text = y.ipucuHakkiBitti;
                TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                gorselSayisiTxt.Text = y.sonGorsel;

                uzaklastirBtn.Enabled = false;
                uzaklastirBtn.SetTextColor(r.butonDeaktifYaziR);

                suAnkiGorselSahneSuresi = 0;
                gorselSahneSayaci.Stop();
                gorselSahneSayaci.Enabled = false;
            }
            catch (Exception ex)
            {
                f.log(ex);
            }
        }

        public async Task gecilmeyiKaydet()
        {
            try
            {
                f.c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                bolumGecildi = true;

                toplamSureSayaci.Stop();

                //Tüm etkileşim yapılabilecek şeyleri deaktif yapıyoruz
                gorselSahneSayaci.Stop();

                butonlarinDurumunuDegistir(false);
                klavyeDurumuDegistir(false);

                ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                soruScroll.Enabled = false;

                //Görseli son sahneye atlatıyoruz
                await gorselSahnesiAtla(true);

                try
                {
                    ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                    bool hasDrawable = (soruGorseli.Drawable != null);
                    if (!hasDrawable)
                    {
                        f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                        return;
                    }
                }
                catch
                {
                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                    return;
                }

                await puanlaVeGuncelle();
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }

        public async Task yanlisCevapKaydet()
        {
            try
            {
                f.c.Window.AddFlags(WindowManagerFlags.NotTouchable);

                bool suAnkiSayacDurumu = gorselSahneSayaci.Enabled;
                if (suAnkiSayacDurumu)
                {
                    gorselSahneSayaci.Stop();
                    gorselSahneSayaci.Enabled = false;
                }

                yanlisCevapVerildi = true;

                yanlisDenemeSayisi++;

                butonlarinDurumunuDegistir(false);
                klavyeDurumuDegistir(false);

                await soruIstatistikGuncelle("yanlisCevapVerildi");

                if (suAnkiSayacDurumu)
                {
                    gorselSahneSayaci.Enabled = true;
                    gorselSahneSayaci.Start();
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }

        public async Task kelimeyiAcmaAnimasyonu()
        {
            try
            {
                //await kısmında pencere değiştirilirse hata vermemesi için sadece bulmaca ekranında aç
                if (f.c is bulmacaEkrani)
                {
                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);

                    //Harf kutularını deaktif yapıyoruz ve renklerini düzenliyoruz
                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        Color rnk = (haT.Background as ColorDrawable).Color;
                        if (rnk == r.kutuArkaplanR) haT.SetTextColor(r.kutuYaziR);

                        haT.Enabled = false;

                    }


                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                        {
                            haT.Text = tumHarfler[i].ToString();
                            haT.SetTextColor(r.kelimeAcYaziR);
                            haT.SetBackgroundColor(r.kelimeAcArkaplanR);

                            //açılmış harfler için de bir süre bekletiyoruz
                            int animasyonSuresi = 2400 / toplamHarfSayisi;

                            await Task.Delay(animasyonSuresi);
                        }


                    }

                    await bolumGecmeAnimasyonu();
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }

        private static bool canShowFlagEmoji(string emoji)
        {
            Paint paint = new Paint();
            try
            {
                return paint.HasGlyph(emoji);
            }
            catch (Java.Lang.NoSuchMethodError)
            {
                float flagWidth = paint.MeasureText(emoji);
                float standardWidth = paint.MeasureText("\uD83D\uDC27"); // U+1F427 Penguin
                return flagWidth < standardWidth * 1.25;
            }
            catch (Exception ex)
            {
                f.log(ex);
                return false;
            }
        }

        string getEmojiByUnicode(int unicode)
        {
            try
            {
                var s = new string(Java.Lang.Character.ToChars(unicode));
                return canShowFlagEmoji(s) ? s : "";
            }
            catch (Exception ex)
            {
                f.log(ex);
                return "";
            }
        }

        public async Task gorselSahnesiAtla(bool sonSahne = false)
        {
            //await kısmında pencere değiştirilirse hata vermemesi için sadece bulmaca ekranında aç
            if (f.c is bulmacaEkrani)
            {
                try
                {
                    if (sonSahne)
                    {
                        TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                        TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);

                        int[]
                            ie = y.iyiEmojiler,
                            oe = y.ortaEmojiler,
                            ko = y.kotuEmojiler,
                            ka = y.kelimeAcEmojiler;

                        var r = new Random();

                        //rastgele emojiler bul
                        string
                            iyiEmoji = getEmojiByUnicode(ie[r.Next(ie.Length)]),
                            ortaEmoji = getEmojiByUnicode(oe[r.Next(oe.Length)]),
                            kotuEmoji = getEmojiByUnicode(ko[r.Next(ko.Length)]),
                            kelimeAcEmoji = getEmojiByUnicode(ka[r.Next(ka.Length)]);

                        string gorselGoruldu_y = "", snKala_y = "", ipucu_y = "", kelimeAcildi_y = "";

                        if (suAnkiGorselSahneSuresi > 0 && soruID <= 150) snKala_y = y.p(y.snKala, suAnkiGorselSahneSuresi);

                        if (gorselSahneSayisi == 1) gorselGoruldu_y = y.p(y.gorselGecmedenAcildi, iyiEmoji, snKala_y);
                        else if (gorselSahneSayisi == (gorselSahneSiniri - 1)) gorselGoruldu_y = y.p(y.sonGorselGoruldu, kotuEmoji);
                        else gorselGoruldu_y = y.p(y.adetGorseldeAcildi, ortaEmoji, gorselSahneSayisi, snKala_y);

                        //diğer satır için tekrar yeni rastgele emojiler bul
                        iyiEmoji = getEmojiByUnicode(ie[r.Next(ie.Length)]);
                        ortaEmoji = getEmojiByUnicode(oe[r.Next(oe.Length)]);
                        kotuEmoji = getEmojiByUnicode(ko[r.Next(ko.Length)]);

                        if (uzaklastirmaSayisi == 0 && gorselSahneSayisi == 1) ipucu_y = y.p(y.ipucuYok, iyiEmoji);
                        else if (uzaklastirmaSayisi == 2) ipucu_y = y.p(y.sonIpucu, kotuEmoji);
                        else if (uzaklastirmaSayisi == 0) ipucu_y = "";
                        else ipucu_y = y.p(y.adetIpucu, ortaEmoji, uzaklastirmaSayisi);

                        if (kelimeAcildi) kelimeAcildi_y = y.p(y.kelimeAcildi, kelimeAcEmoji);

                        //en sonda "ayraç" varsa siliyoruz
                        string d = ipucu_y + kelimeAcildi_y;
                        try
                        {
                            if (d.Substring(d.Length - 3) == y.gorselAltiYaziAyraci)
                                d = d.Substring(0, d.Length - 3);
                        }
                        catch { }//kontrollü catch

                        gorselSuresiTxt.Text = gorselGoruldu_y;
                        gorselSayisiTxt.Text = d;
                    }

                    ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);

                    //ilk giriş ise tüm görselleri de rame alıyoruz (her uzaklaştırmada yavaşlama olmaması için)
                    if (gorseller == null)
                    {
                        gorseller = new Bitmap[gorselSahneSiniri];

                        for (int i = 0; i < gorselSahneSiniri; i++)
                        {
                            //link şu şekilde olmalı: "/soruGorselleri/1/1.jpg" , "/soruGorselleri/1/2.jpg" , "/soruGorselleri/1/3.jpg"
                            //yani klasör uygulamadaki sorunun idsini, sonraki gorsellerde gorsel sahne idsini belirliyor
                            string dosyaAdi = soruID + "/" + (i + 1) + ".jpg";

                            Stream localFile = null;
                            /*try
                            {
                                localFile = Assets.Open("soruGorselleri/" + dosyaAdi);
                            }
                            catch//kontrollü catch
                            {
                                localFile = null;
                            }*/

                            if (localFile == null)
                            {
                                string url = "";
                                if (f.hasServerTime)
                                    url = f.server_bulmaca_gorseli_link + f.tokenOlustur() + "/" + dosyaAdi;

                                //localde görseller bulunmuyorsa yani yeni bir soru veritabanından eklenmişse, görselleri sunucudan çekiyoruz
                                var t = Task.Run(() => f.GetImageBitmapFromUrl(url));
                                await t.ConfigureAwait(true);
                                if (t.Result != null)
                                {
                                    gorseller[i] = t.Result;
                                    t.Dispose();
                                }
                                else
                                {
                                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                                    return;
                                }
                            }
                            else gorseller[i] = BitmapFactory.DecodeStream(localFile);
                        }
                    }

                    var gorsel = gorseller[(sonSahne ? gorselSahneSiniri : gorselSahneSayisi) - 1];

                    soruGorseli.SetImageBitmap(gorsel);

                }
                catch (ObjectDisposedException ex)
                {
                    f.log(ex);
                }
                catch (Exception ex)
                {
                    f.log(ex);
                    return;
                }
            }
        }
        public async Task gorseliUzaklastir(Button uzaklastirBtn)
        {
            try
            {
                uzaklastirmaSayisi++;
                uzaklastirBtn.Enabled = false;
                uzaklastirBtn.SetTextColor(r.butonDeaktifYaziR);

                if (uzaklastirmaSayisi != (gorselSahneSiniri - 1))
                {

                    gorselSahneSayisi++;
                    await gorselSahnesiAtla();

                    try
                    {
                        ImageView soruGorseli = FindViewById<ImageView>(Resource.Id.soruGorseli);
                        bool hasDrawable = (soruGorseli.Drawable != null);
                        if (!hasDrawable)
                        {
                            f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                            return;
                        }
                    }
                    catch
                    {
                        f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                        return;
                    }

                    if (soruID <= 150) suAnkiGorselSahneSuresi = gorselSahneSuresi;

                    await soruIstatistikGuncelle("gorselUzaklastirildi");


                    if (soruID > 150)
                    {
                        TextView gorselSayisiTxt = FindViewById<TextView>(Resource.Id.gorselSayisiTxt);
                        gorselSayisiTxt.Text = y.p(y.gorselSayisi, gorselSahneSayisi);

                        uzaklastirBtn.Enabled = true;
                        uzaklastirBtn.SetTextColor(r.butonAktifYaziR);


                        if (uzaklastirmaSayisi == (gorselSahneSiniri - 2))
                        {
                            TextView gorselSuresiTxt = FindViewById<TextView>(Resource.Id.gorselSuresiTxt);
                            gorselSuresiTxt.Text = y.ipucuHakkiBitti;
                            gorselSayisiTxt.Text = y.sonGorsel;

                            uzaklastirBtn.Enabled = false;
                            uzaklastirBtn.SetTextColor(r.butonDeaktifYaziR);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public void klavyeOlustur()
        {
            try
            {
                tuslar[0] = y.klavyeSatir1;
                tuslar[1] = y.klavyeSatir2;
                tuslar[2] = y.klavyeSatir3 + "\u2190";//sondaki geri butonu

                char[] _tuslar;
                int suAnkiSatir = 0;

                foreach (string satir in tuslar)
                {
                    //Satırdaki harf sayısını belirliyoruz
                    int tusSayisi = satir.Length;

                    //Tuşları harflerine ayırıyoruz
                    _tuslar = satir.ToCharArray();

                    //Tuşların harf sayısı kadar textBox oluşturmak için kapsayıcı üst layout alanı belirliyoruz (div gibi)
                    LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye); //root layout

                    //Hangi satira eklenecekse o satiri belirliyoruz
                    LinearLayout klavyeSatiriLayout = klavye.FindViewWithTag(suAnkiSatir.ToString()) as LinearLayout;
                    klavyeSatiriLayout.SetPadding(0, 0, 0, f.px(10));

                    var param = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                    param.SetMargins(f.px(5), 0, f.px(5), 0);


                    //Cevaptaki harf sayısı kadar textBox cloneluyoruz
                    for (int i = 0; i < tusSayisi; i++)
                    {
                        int harfID = i;
                        //Harf sayısı kadar tuş oluşturmak için tuş özelliklerini belirliyoruz
                        TextView tus = new TextView(this);
                        tus.Text = _tuslar[harfID].ToString();
                        tus.Tag = _tuslar[harfID];
                        tus.SetWidth(f.px(80));
                        tus.SetHeight(f.px(80));
                        tus.SetPadding(f.px(15), f.px(5), f.px(15), f.px(5));
                        tus.TextAlignment = TextAlignment.Center;
                        tus.SetTextColor(r.klavyeAktifYaziR);
                        tus.SetBackgroundColor(r.klavyeArkaplanR);
                        tus.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(60));
                        tus.SetMaxLines(1);
                        int maxLength = 1;
                        tus.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(maxLength) });
                        tus.SetSingleLine(true);
                        tus.SetSelectAllOnFocus(true);
                        tus.SetRawInputType(InputTypes.Null);
                        tus.Focusable = true;
                        tus.FocusableInTouchMode = true;
                        tus.LongClickable = false;
                        tus.SetHighlightColor(Color.Transparent);
                        klavyeSatiriLayout.AddView(tus, param);

                        //Tuşa basıldığında textBoxları kontrol ediyoruz 
                        TextView textvw = tus;
                        textvw.Touch += async (sender, args) =>
                        {
                            //sadece basma durumunda çalışmalı, diğer türlü tüm durumlarda çalışıyor up, move gibi
                            if ((args as TextView.TouchEventArgs).Event.Action == MotionEventActions.Down)
                            {
                                Window.AddFlags(WindowManagerFlags.NotTouchable);

                                if (suAnkiKlavyeDurumu && !isleniyor && !yanlisCevapVerildi)
                                {
                                    isleniyor = true;

                                    textvw.Enabled = false;

                                    klavyeRenkDegis(false);

                                    f.sesEfektiCal("klavyeBasildi");


                                    Color oncekiRenk = new Color(textvw.CurrentTextColor);
                                    Color oncekiRenk2 = (textvw.Background as ColorDrawable).Color;

                                    textvw.SetTextColor(r.klavyeBasildigindakiYaziR);
                                    textvw.SetBackgroundColor(r.klavyeBasildigindakiArkaplanR);

                                    await Task.Delay(100);

                                    tus.SetTextColor(oncekiRenk);
                                    tus.SetBackgroundColor(oncekiRenk2);


                                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);
                                    EditText sonKutu = null;

                                    //eğer seçim boş ise ilk aktif kutuyu bul (harf alınmamış kutuyu bul)
                                    if (sonSeciliKutuID == null)
                                    {
                                        sonKutu = sonKalinanKutuyuBul(cevapKutulari);
                                    }
                                    else
                                    {
                                        //eğer seçim boş değilse devam
                                        string[] bol = sonSeciliKutuID.Split(',');
                                        string satirIDx = bol[0];

                                        LinearLayout csL2_ = cevapKutulari.FindViewWithTag(satirIDx) as LinearLayout;
                                        sonKutu = csL2_.FindViewWithTag(sonSeciliKutuID) as EditText;
                                    }


                                    //silme tuşu ise
                                    if (textvw.Text == "\u2190")
                                    {
                                        //harf açılmamışsa
                                        if (sonKutu.Enabled)
                                        {
                                            if (sonKutu.Text != "")
                                                sonKutu.EditableText?.Clear();
                                            else
                                            {
                                                klavyeKomutu = "geriGit";
                                                sonKutu.Text = "X";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //harf açılmamışsa
                                        if (sonKutu.Enabled)
                                            sonKutu.Text = textvw.Text;
                                    }

                                    textvw?.ClearFocus();

                                    await tusAyarlamalari(cevapKutulari, sonKutu);

                                    klavyeRenkDegis(true);

                                    textvw.Enabled = true;

                                    isleniyor = false;
                                }

                                Window.ClearFlags(WindowManagerFlags.NotTouchable);
                            }

                        };

                    }


                    suAnkiSatir++;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        EditText sonKalinanKutuyuBul(LinearLayout cevapKutulari)
        {
            EditText sonKutu = null;

            try
            {
                int WsatirSay = 0, WkutuSay = 0;
                while (true)
                {
                    LinearLayout Zsatir = null;
                    try
                    {
                        Zsatir = cevapKutulari.FindViewWithTag(WsatirSay.ToString()) as LinearLayout;
                    }
                    catch { }//kontrollü catch

                    if (Zsatir != null)
                    {
                        int simdikiSatirdaKacKutuVar = 0;
                        for (int i = 0; i < kutuIDsiHangiSatiraAit.Count; i++)
                        {
                            if (kutuIDsiHangiSatiraAit[i].ToString() == WsatirSay.ToString()) simdikiSatirdaKacKutuVar++;
                        }
                        simdikiSatirdaKacKutuVar -= 1;

                        while (true)
                        {
                            EditText Zkutu = null;
                            try
                            {
                                Zkutu = Zsatir.FindViewWithTag(WsatirSay + "," + WkutuSay) as EditText;
                            }
                            catch { }//kontrollü catch

                            if (Zkutu != null)
                            {
                                if (Zkutu.Enabled)
                                {
                                    sonSeciliKutuID = WsatirSay + "," + WkutuSay;
                                    sonKutu = Zkutu;
                                    break;
                                }
                            }

                            WkutuSay++;
                            if (WkutuSay >= simdikiSatirdaKacKutuVar) break;
                        }
                    }
                    if (sonKutu != null) break;
                    else if (WsatirSay <= toplamSatirSayisi) WsatirSay++;
                    else break;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }

            return sonKutu;
        }

        public async Task tusAyarlamalari(LinearLayout cevapKutulari, TextView sonKutu)
        {
            try
            {
                if (f.c is bulmacaEkrani)
                {
                    isleniyor = true;
                    if (!yanlisCevapVerildi && suAnkiKlavyeDurumu)
                    {
                        if (!bolumGecildi)
                        {
                            bool digitsVarMi = false;
                            string digits = y.klavyeTumHarfler;
                            char[] chars = digits.ToCharArray();
                            for (int i2 = 0; i2 < chars.Length; i2++)
                            {
                                if (chars[i2].ToString() == sonKutu.Text)
                                {
                                    digitsVarMi = true;
                                    break;
                                }
                            }

                            if (digitsVarMi)
                            {
                                //son seçim boşsa kalınan kutuyu bul (muhtemel ilk giriş)
                                if (sonSeciliKutuID == null)
                                    sonKalinanKutuyuBul(cevapKutulari);



                                //Tüm textboxlar dolu mu diye kontrol ediyoruz
                                int suAnkiGirilenCevapSayisi = 0;
                                int suAnkiDogruCevapSayisi = 0;
                                int kacinciNesne = 0;
                                for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                                {
                                    string satirID = "", kutuID = "";
                                    satirID = kutuIDsiHangiSatiraAit[i].ToString();
                                    kutuID = satirdakiKutuIDleri[i].ToString();

                                    LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                                    EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                                    if (haT.Text != "")
                                    {
                                        string satirID2 = "", kutuID2 = "";
                                        int kacinciNesne2 = 0;
                                        for (int i2 = 0; i2 < satirdakiKutuIDleri.Count; i2++)
                                        {
                                            if (kacinciNesne2 == kacinciNesne)
                                            {
                                                satirID2 = kutuIDsiHangiSatiraAit[i2].ToString();
                                                kutuID2 = satirdakiKutuIDleri[i2].ToString();
                                                break;
                                            }
                                            kacinciNesne2++;
                                        }

                                        LinearLayout csL2 = cevapKutulari.FindViewWithTag(satirID2) as LinearLayout;
                                        EditText haT2 = csL2.FindViewWithTag(satirID2 + "," + kutuID2) as EditText;

                                        if (haT2.Tag == haT.Tag)
                                        {
                                            if (tumHarfler[kacinciNesne].ToString() == haT.Text)
                                            {
                                                suAnkiDogruCevapSayisi++;
                                            }
                                        }
                                        suAnkiGirilenCevapSayisi++;
                                    }

                                    kacinciNesne++;
                                }



                                //Sonraki harf kutusuna otomatik geçiyoruz
                                string[] ebol = sonKutu.Tag.ToString().Split(",");
                                string simdikiSatirID = ebol[0];
                                string simdikiHarfID = ebol[1];
                                string sonrakiSatirID = (Convert.ToInt32(simdikiSatirID) + 1).ToString();
                                string sonrakiHarfID = simdikiSatirID + "," + (Convert.ToInt32(simdikiHarfID) + 1).ToString();
                                string oncekiSatirID = (Convert.ToInt32(simdikiSatirID) - 1).ToString();
                                string oncekiHarfID = simdikiSatirID + "," + (Convert.ToInt32(simdikiHarfID) - 1).ToString();

                                string _satirID = simdikiSatirID;

                                LinearLayout csL3;
                                EditText haT3;

                                if (klavyeKomutu == "geriGit")
                                {
                                    klavyeKomutu = "";
                                    sonKutu.EditableText?.Clear();



                                    csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                    haT3 = csL3.FindViewWithTag(oncekiHarfID) as EditText;

                                    int satirdakiSonKutuID = 0;

                                    if (haT3 == null)
                                    {
                                        if (Convert.ToInt32(oncekiSatirID) < 0) _satirID = (toplamSatirSayisi - 1).ToString();
                                        else _satirID = oncekiSatirID;
                                        for (int i = 0; i < kutuIDsiHangiSatiraAit.Count; i++)
                                        {
                                            if (kutuIDsiHangiSatiraAit[i].ToString() == _satirID) satirdakiSonKutuID++;
                                        }
                                        satirdakiSonKutuID -= 1;

                                        oncekiHarfID = _satirID + "," + satirdakiSonKutuID;

                                        csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                        haT3 = csL3.FindViewWithTag(oncekiHarfID) as EditText;
                                    }


                                    if (haT3.Enabled == false)
                                    {
                                        int birOncekiID = Convert.ToInt32(simdikiHarfID) - 2;
                                        oncekiHarfID = _satirID + "," + birOncekiID.ToString();

                                        //Eğer tüm kutular doldurulmadıysa
                                        while (suAnkiGirilenCevapSayisi != toplamHarfSayisi)
                                        {
                                            if (birOncekiID < 0)
                                            {
                                                if (Convert.ToInt32(birOncekiID) != -2) _satirID = (Convert.ToInt32(_satirID) - 1).ToString();
                                                if (Convert.ToInt32(_satirID) < 0) _satirID = (toplamSatirSayisi - 1).ToString();

                                                satirdakiSonKutuID = 0;
                                                for (int i = 0; i < kutuIDsiHangiSatiraAit.Count; i++)
                                                {
                                                    if (kutuIDsiHangiSatiraAit[i].ToString() == _satirID) satirdakiSonKutuID++;
                                                }
                                                satirdakiSonKutuID -= 1;
                                                birOncekiID = satirdakiSonKutuID;
                                                oncekiHarfID = _satirID + "," + birOncekiID.ToString();
                                            }

                                            csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                            haT3 = csL3.FindViewWithTag(oncekiHarfID) as EditText;

                                            if (haT3.Enabled == false)
                                            {
                                                birOncekiID = Convert.ToInt32(birOncekiID) - 1;
                                                oncekiHarfID = _satirID + "," + birOncekiID.ToString();
                                            }
                                            else break;
                                        }

                                    }

                                    klavyeKomutu = "";
                                }
                                else
                                {

                                    csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                    haT3 = csL3.FindViewWithTag(sonrakiHarfID) as EditText;

                                    if (haT3 == null)
                                    {
                                        if (Convert.ToInt32(sonrakiSatirID) > (toplamSatirSayisi - 1)) _satirID = "0";
                                        else _satirID = sonrakiSatirID;

                                        sonrakiHarfID = _satirID + ",0";

                                        csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                        haT3 = csL3.FindViewWithTag(sonrakiHarfID) as EditText;
                                    }


                                    if (haT3.Enabled == false)
                                    {
                                        int birSonrakiD = Convert.ToInt32(simdikiHarfID) + 2;


                                        int simdikiSatirdaKacKutuVar = 0;
                                        for (int i = 0; i < kutuIDsiHangiSatiraAit.Count; i++)
                                        {
                                            if (kutuIDsiHangiSatiraAit[i].ToString() == simdikiSatirID) simdikiSatirdaKacKutuVar++;
                                        }
                                        simdikiSatirdaKacKutuVar -= 1;

                                        if (birSonrakiD > simdikiSatirdaKacKutuVar)
                                        {
                                            birSonrakiD = 0;

                                            if (Convert.ToInt32(sonrakiSatirID) <= (toplamSatirSayisi - 1))
                                                _satirID = (Convert.ToInt32(simdikiSatirID) + 1).ToString();
                                            else _satirID = "0";
                                        }


                                        sonrakiHarfID = _satirID + "," + birSonrakiD.ToString();

                                        int donguSayaci = 0;

                                        while (true)
                                        {
                                            int satirdakiSonKutuID = 0;

                                            for (int i = 0; i < kutuIDsiHangiSatiraAit.Count; i++)
                                            {
                                                if (kutuIDsiHangiSatiraAit[i].ToString() == _satirID) satirdakiSonKutuID++;
                                            }
                                            satirdakiSonKutuID -= 1;

                                            if (birSonrakiD > satirdakiSonKutuID)
                                            {
                                                _satirID = (Convert.ToInt32(_satirID) + 1).ToString();
                                                if (Convert.ToInt32(_satirID) > (toplamSatirSayisi - 1)) _satirID = "0";

                                                birSonrakiD = 0;
                                                sonrakiHarfID = _satirID + "," + birSonrakiD.ToString();
                                            }

                                            csL3 = cevapKutulari.FindViewWithTag(_satirID) as LinearLayout;
                                            haT3 = csL3.FindViewWithTag(sonrakiHarfID) as EditText;

                                            if (haT3.Enabled == false)
                                            {
                                                birSonrakiD = Convert.ToInt32(birSonrakiD) + 1;
                                                sonrakiHarfID = _satirID + "," + birSonrakiD.ToString();
                                                donguSayaci++;
                                            }
                                            else break;


                                            if (f.oAyarlariTBL_.yanlisKelimeSil &&
                                                suAnkiGirilenCevapSayisi == toplamHarfSayisi) break;


                                            if (donguSayaci > 30) break;
                                        }

                                    }

                                }



                                ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                                soruScroll.Enabled = false;

                                //focus ile ilgili problemler oluyor: focus yapmadan önce tüm focusları temizliyoruz
                                for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                                {
                                    string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                                    string kutuID = satirdakiKutuIDleri[i].ToString();

                                    LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                                    EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                                    haT?.ClearFocus();
                                }


                                //önceki seçili kutunun boyasını eski haline getiriyoruz
                                if (sonSeciliKutuID != null)
                                {
                                    string[] bolx = sonSeciliKutuID.Split(',');
                                    string satirIDx = bolx[0];

                                    LinearLayout csLl = cevapKutulari.FindViewWithTag(satirIDx) as LinearLayout;
                                    EditText haTl = csLl.FindViewWithTag(sonSeciliKutuID) as EditText;

                                    if (haTl.Enabled)
                                    {
                                        haTl.SetTextColor(r.kutuYaziR);
                                        haTl.SetBackgroundColor(r.kutuArkaplanR);
                                    }
                                }

                                if (suAnkiGirilenCevapSayisi != toplamHarfSayisi)
                                {
                                    if (haT3.Enabled)
                                    {
                                        haT3.SetTextColor(r.kutuSeciliYaziR);
                                        haT3.SetBackgroundColor(r.kutuSeciliArkaplanR);
                                    }
                                }

                                sonSeciliKutuID = haT3.Tag.ToString();


                                soruScroll.Enabled = true;



                                if (suAnkiGirilenCevapSayisi == toplamHarfSayisi && sonKutu.Text != "")
                                {
                                    //Tüm harfler dolduruldu
                                    if (suAnkiDogruCevapSayisi == toplamHarfSayisi)
                                    {
                                        //Tüm harfler doğru
                                        await gecilmeyiKaydet();
                                        await bolumGecmeAnimasyonu();
                                    }
                                    else
                                    {
                                        //Harflerde yanlışlık var
                                        await yanlisCevapKaydet();
                                        await harflerYanlisAnimasyonu();
                                    }
                                }
                            }

                        }
                        else
                        {
                            sonKutu.EditableText?.Clear();
                        }
                    }
                    isleniyor = false;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public void klavyeDurumuDegistir(bool durum)
        {
            try
            {
                suAnkiKlavyeDurumu = durum;
                Color renk;
                if (durum) renk = r.klavyeAktifYaziR;
                else renk = r.klavyeDeaktifYaziR;

                LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye);
                klavye.Enabled = durum;

                LinearLayout ksatir0 = FindViewById<LinearLayout>(Resource.Id.ksatir0);
                ksatir0.Enabled = durum;

                LinearLayout ksatir1 = FindViewById<LinearLayout>(Resource.Id.ksatir1);
                ksatir1.Enabled = durum;

                LinearLayout ksatir2 = FindViewById<LinearLayout>(Resource.Id.ksatir2);
                ksatir2.Enabled = durum;


                char[] _tuslar;
                int suAnkiSatir = 0;

                foreach (string satir in tuslar)
                {
                    //Satırdaki harf sayısını belirliyoruz
                    int tusSayisi = satir.Length;

                    //Tuşları harflerine ayırıyoruz
                    _tuslar = satir.ToCharArray();

                    //Hangi satira eklenecekse o satiri belirliyoruz
                    LinearLayout klavyeSatiriLayout = klavye.FindViewWithTag(suAnkiSatir.ToString()) as LinearLayout;

                    //Cevaptaki harf sayısı kadar textBox cloneluyoruz
                    for (int i = 0; i < tusSayisi; i++)
                    {
                        TextView tus = klavyeSatiriLayout.FindViewWithTag(_tuslar[i]) as TextView;
                        tus.SetTextColor(renk);
                        tus.Enabled = durum;
                    }

                    suAnkiSatir++;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public void klavyeRenkDegis(bool durum)
        {
            try
            {
                Color renk;
                if (durum) renk = r.klavyeAktifYaziR;
                else renk = r.klavyeDeaktifYaziR;

                LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye);

                char[] _tuslar;
                int suAnkiSatir = 0;

                foreach (string satir in tuslar)
                {
                    //Satırdaki harf sayısını belirliyoruz
                    int tusSayisi = satir.Length;

                    //Tuşları harflerine ayırıyoruz
                    _tuslar = satir.ToCharArray();

                    //Hangi satira eklenecekse o satiri belirliyoruz
                    LinearLayout klavyeSatiriLayout = klavye.FindViewWithTag(suAnkiSatir.ToString()) as LinearLayout;

                    //Cevaptaki harf sayısı kadar textBox cloneluyoruz
                    for (int i = 0; i < tusSayisi; i++)
                    {
                        TextView tus = klavyeSatiriLayout.FindViewWithTag(_tuslar[i]) as TextView;
                        tus.SetTextColor(renk);
                    }

                    suAnkiSatir++;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public void butonlarinDurumunuDegistir(bool durum)
        {
            try
            {
                Color renk;
                if (durum) renk = r.butonAktifYaziR;
                else renk = r.butonDeaktifYaziR;

                LinearLayout butonlar = FindViewById<LinearLayout>(Resource.Id.butonlar);
                butonlar.Enabled = durum;

                Button harfAlBtn = FindViewById<Button>(Resource.Id.harfAlBtn);
                harfAlBtn.Enabled = durum;
                harfAlBtn.SetTextColor(renk);

                if (durum == false)
                {
                    Button uzaklastirBtn = FindViewById<Button>(Resource.Id.uzaklastirBtn);
                    uzaklastirBtn.Enabled = durum;
                    uzaklastirBtn.SetTextColor(renk);
                }

                Button kelimeyiAcBtn = FindViewById<Button>(Resource.Id.kelimeyiAcBtn);
                kelimeyiAcBtn.Enabled = durum;
                kelimeyiAcBtn.SetTextColor(renk);
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public async Task harfAl()
        {
            try
            {
                if (f.c is bulmacaEkrani)
                {
                    if (bilinenHarfler.Count != toplamHarfSayisi)
                    {
                        Random rastgeleHarfBul = new Random();
                        int rastgeleHarfID;
                        string satirID = "";
                        string kutuID = "";

                        do
                        {
                            rastgeleHarfID = rastgeleHarfBul.Next(0, toplamHarfSayisi);
                        }
                        while (acilanHarfler.IndexOf(rastgeleHarfID) >= 0);

                        acilanHarfler.Add(rastgeleHarfID);


                        int kacinciNesne = 0;
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            if (kacinciNesne == rastgeleHarfID)
                            {
                                satirID = kutuIDsiHangiSatiraAit[i].ToString();
                                kutuID = satirdakiKutuIDleri[i].ToString();
                                break;
                            }
                            kacinciNesne++;
                        }

                        if (bilinenHarfler.IndexOf(satirID + "," + kutuID) >= 0)
                        {
                            //fonks.mesajGoster("Bir sorun oluştu. Bu harf zaten açık.\n\nLütfen harfi açmayı tekrar deneyiniz.", this);
                        }
                        else
                        {
                            harfAlmaSayisi++;
                            LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);
                            LinearLayout cevapSatiriLayout = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText harfAlTxt = cevapSatiriLayout.FindViewWithTag(satirID + "," + kutuID) as EditText;


                            ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                            soruScroll.Enabled = false;


                            //harf kutularını deaktif yapıyoruz ve focusları temizliyoruz
                            for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                            {
                                string satirIDx = kutuIDsiHangiSatiraAit[i].ToString();
                                string kutuIDx = satirdakiKutuIDleri[i].ToString();

                                LinearLayout csL = cevapKutulari.FindViewWithTag(satirIDx) as LinearLayout;
                                EditText haT = csL.FindViewWithTag(satirIDx + "," + kutuIDx) as EditText;


                                haT?.ClearFocus();
                            }


                            harfAlTxt.Enabled = false;
                            bilinenHarfler.Add(satirID + "," + kutuID);


                            harfAlTxt.Text = tumHarfler[rastgeleHarfID].ToString();

                            harfAlTxt.SetTextColor(r.harfAlYaziR);
                            harfAlTxt.SetBackgroundColor(r.harfAlArkaplanR);


                            await tusAyarlamalari(cevapKutulari, harfAlTxt);


                            soruScroll.Enabled = true;

                            await soruIstatistikGuncelle("harfAlindi");
                        }
                    }
                    else
                    {
                        //mesajGoster("Zaten tüm harfler alınmış.");
                    }
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        async Task puanlaVeGuncelle()
        {
            try
            {
                //maksimum bir puan belirliyoruz
                int maxPuan = 1000;

                double sahneGecmeOrani = 10, yanlisDenemeOrani = 1;

                sahneGecmeOrani = sahneGecmeOrani - (toplamHarfSayisi * 0.4);
                yanlisDenemeOrani = yanlisDenemeOrani - (toplamHarfSayisi * 0.03);

                //yanlış cevap verildiğinde veya diğer seçeneklerde düşürülecek puan miktarı belirliyoruz
                double sahneGecmePuani = 25, yanlisDenemePuani = 2.5, harfAlmaPuani = 5;

                //harf sayısı arttıkça düşürülecek puan miktarında belirli ölçüde yükseltme yapıyoruz
                sahneGecmePuani = sahneGecmePuani + (toplamHarfSayisi * sahneGecmeOrani);
                yanlisDenemePuani = yanlisDenemePuani + (toplamHarfSayisi * yanlisDenemeOrani);
                harfAlmaPuani = harfAlmaPuani + (maxPuan / toplamHarfSayisi);

                //toplam puanları belirliyoruz
                double toplamSahneGecmePuani = (gorselSahneSayisi - 1) * sahneGecmePuani;
                double toplamYanlisDenemePuani = yanlisDenemeSayisi * yanlisDenemePuani;
                double toplamHarfAlmaPuani = harfAlmaSayisi * harfAlmaPuani;

                //maks. puandan düşürülecek puanları düşürüyoruz
                double toplam = maxPuan - toplamSahneGecmePuani;
                toplam -= toplamYanlisDenemePuani;
                toplam -= toplamHarfAlmaPuani;

                double sureSkoru = toplamSure - (toplamHarfSayisi * 3);
                if (sureSkoru < 0) sureSkoru = 0;

                toplam -= sureSkoru;


                //150. sorudan öncekiler için kalan görsel sahne süresini de ekstra puan olarak oyuncuya veriyoruz (ilk oyuncular için bonus puan)
                if (soruID <= 150)
                {
                    double kalanGorselSahneSuresi = 0;
                    if (gorselSahneSayisi == 1) kalanGorselSahneSuresi = gorselSahneSuresi * 2;
                    else if (gorselSahneSayisi == 2) kalanGorselSahneSuresi = gorselSahneSuresi;
                    else if (gorselSahneSayisi == 3) kalanGorselSahneSuresi = 0;

                    kalanGorselSahneSuresi = suAnkiGorselSahneSuresi + kalanGorselSahneSuresi;

                    toplam += Convert.ToInt32(Math.Round(kalanGorselSahneSuresi));
                    if (toplam > maxPuan) toplam = maxPuan;
                }

                //kelime açıldıysa toplam puanın 3te 1ini veriyoruz
                if (kelimeAcildi) toplam = toplam / (harfAlmaSayisi == 0 ? toplamHarfSayisi : (toplamHarfSayisi / harfAlmaSayisi));

                //en düşük puanı 100 yapıyoruz
                if (toplam < 100) toplam = 100;


                toplamPuan = Convert.ToInt32(Math.Round(toplam));


                await soruIstatistikGuncelle("hepsi");

                //veritabanına başarı ile kaydedildiyse play skor ve başarım kontrolleri yapılıyor
                f.PlayGames__.Yukle();
                bool basarimAcildi = f.PlayGames__.basarimlaraAdimAtlat(soruID);

                //bölüm ilk defa geçiliyorsa
                if (f.bolumGecilmemisse(soruID))
                {
                    f.PlayGames__.skorGonder(toplamPuan, soruID, bolumAdi);
                    //toplam geçilen bölüm sayısını 1 arttırıyoruz (başarım kontrolleri için)
                    f.toplamGecilenBolumSayisi++;

                    //başarımları kontrol ediyoruz
                    bool basarimKontrolR = await f.basarimKontrol();
                    f.basarimKontrolUI(basarimKontrolR);

                    //eğer playde başarım açıldıysa bölüm sonu bekleme süresini uzatıyoruz
                    if (basarimAcildi) bolumSonuBeklemeSuresi = 5000;

                    //bölümün geçildiğini uygulama içine de ekliyoruz
                    string hangiHarflerAcildi = hangiHarflerAcildiBul();
                    string acilan_harfler = acilan_harflerBul();

                    soruIstatistikTBL sit = new soruIstatistikTBL();
                    sit.soruID = soruID;
                    sit.bolumGecildi = bolumGecildi;
                    sit.hangiHarflerAcildi = hangiHarflerAcildi;
                    sit.toplamSure = toplamSure;
                    sit.yanlisDenemeSayisi = yanlisDenemeSayisi;
                    sit.gorselSahneSayisi = gorselSahneSayisi;
                    sit.suAnkiGorselSahneSuresi = suAnkiGorselSahneSuresi;
                    sit.uzaklastirmaSayisi = uzaklastirmaSayisi;
                    sit.harfAlmaSayisi = harfAlmaSayisi;
                    sit.kelimeAcildi = kelimeAcildi;
                    sit.toplamPuan = toplamPuan;
                    sit.acilanHarfler = acilan_harfler;

                    f.soruIstatistikTBL_ = sit;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }


        public async Task bolumGecmeAnimasyonu()
        {
            try
            {
                //await kısmında pencere değiştirilirse hata vermemesi için sadece bulmaca ekranında aç
                if (f.c is bulmacaEkrani)
                {
                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);

                    if (!kelimeAcildi)
                    {
                        f.sesEfektiCal("kelimeAciliyor");

                        //Harf kutularını deaktif yapıyoruz ve renklerini düzenliyoruz
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                            Color rnk = (haT.Background as ColorDrawable).Color;
                            if (rnk == r.kutuArkaplanR) haT.SetTextColor(r.kutuYaziR);

                            haT.Enabled = false;

                        }
                    }


                    int animasyonSuresi = 400 / toplamHarfSayisi;

                    await Task.Delay(animasyonSuresi);

                    //harflerin yeşil yanıp sönerek yanarak baştan sona doğru gitmesi animasyonu
                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;


                        Color oncekiRenk = new Color(haT.CurrentTextColor);
                        Color oncekiRenk2 = (haT.Background as ColorDrawable).Color;

                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            if (i2 % 2 == 0)
                            {
                                haT.SetTextColor(r.bolumGecYaziR);
                                haT.SetBackgroundColor(r.bolumGecArkaplanR);
                            }
                            else if (i2 % 2 == 1)
                            {
                                haT.SetTextColor(oncekiRenk);
                                haT.SetBackgroundColor(oncekiRenk2);
                            }


                            await Task.Delay(animasyonSuresi);
                        }


                    }


                    //harflerin yeşil yanıp sönerek yanarak sondan başa doğru geri gitmesi animasyonu
                    for (int i = satirdakiKutuIDleri.Count - 1; i >= 0; i--)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        Color oncekiRenk = new Color(haT.CurrentTextColor);
                        Color oncekiRenk2 = (haT.Background as ColorDrawable).Color;

                        for (int i2 = 0; i2 < 2; i2++)
                        {
                            if (i2 % 2 == 0)
                            {
                                haT.SetTextColor(r.bolumGecYaziR);
                                haT.SetBackgroundColor(r.bolumGecArkaplanR);
                            }
                            else if (i2 % 2 == 1)
                            {
                                haT.SetTextColor(oncekiRenk);
                                haT.SetBackgroundColor(oncekiRenk2);
                            }

                            await Task.Delay(animasyonSuresi);
                        }


                    }

                    await Task.Delay(animasyonSuresi);

                    //tüm harflerin aynı anda yanıp sönerek yeşile dönmesi
                    for (int i2 = 0; i2 < 2; i2++)
                    {
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                            haT.SetTextColor(r.kutuYaziR);
                            haT.SetBackgroundColor(r.kutuArkaplanR);

                        }

                        await Task.Delay(200);

                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                            haT.SetTextColor(r.bolumGecYaziR);
                            haT.SetBackgroundColor(r.bolumGecArkaplanR);

                        }

                        await Task.Delay(200);
                    }


                    f.gecisReklaminiHazirEt();

                    await Task.Delay(bolumSonuBeklemeSuresi);

                    await bolumSonuPenceresiniAc();
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        public async Task bolumSonuPenceresiniAc()
        {
            try
            {
                //await kısmında pencere değiştirilirse hata vermemesi için sadece bulmaca ekranında aç
                if (f.c is bulmacaEkrani)
                {
                    Window.AddFlags(WindowManagerFlags.NotTouchable);

                    if (RequestedOrientation != Android.Content.PM.ScreenOrientation.Portrait)
                        RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;

                    f.sesEfektiCal("bolumGecildi");

                    f.acikPencereleriKapat();

                    ViewGroup anaView = Window.DecorView.RootView as ViewGroup;

                    Drawable varsayilanArkaplan = anaView.Foreground; anaView.Foreground = new ColorDrawable(r.pencereSeffafArkaplanR);

                    //loading çıkarıyoruz
                    ProgressBar loadingBar = new ProgressBar(this);
                    loadingBar.IndeterminateDrawable = f.LD(loadingBar.IndeterminateDrawable);
                    loadingBar.Indeterminate = true;
                    RelativeLayout rl = new RelativeLayout(this);
                    rl.SetGravity(GravityFlags.Center);
                    rl.AddView(loadingBar);
                    var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                    anaView.AddView(rl, param);

                    await Task.Delay(300);

                    LayoutInflater inflater = (LayoutInflater)GetSystemService(LayoutInflaterService);
                    View bulmacaEkrani_gecildi = inflater.Inflate(Resource.Layout.bulmacaEkrani_gecildi, null, false);

                    PopupWindow popupWindow = new PopupWindow(bulmacaEkrani_gecildi, LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

                    popupWindow.Focusable = false;
                    popupWindow.OutsideTouchable = false;

                    View pcw = popupWindow.ContentView;

                    RelativeLayout gecildiRl = pcw.FindViewById<RelativeLayout>(Resource.Id.gecildiRl);
                    gecildiRl.SetBackgroundResource(Resource.Drawable.p_arkaplan);


                    TextView gecildiTxt = pcw.FindViewById<TextView>(Resource.Id.gecildiTxt);
                    gecildiTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(50));
                    var p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    p.Gravity = GravityFlags.Center;
                    int _ = f.px(20);
                    p.SetMargins(_, _, _, _);
                    gecildiTxt.LayoutParameters = p;
                    gecildiTxt.RequestLayout();

                    ImageView yildizImg = pcw.FindViewById<ImageView>(Resource.Id.yildizImg);
                    yildizImg.SetScaleType(ImageView.ScaleType.FitXy);
                    p = new LinearLayout.LayoutParams(f.px(600), f.px(190));
                    p.Gravity = GravityFlags.Top | GravityFlags.CenterHorizontal;
                    _ = f.px(30);
                    p.SetMargins(_, _, _, _);
                    yildizImg.LayoutParameters = p;
                    yildizImg.RequestLayout();


                    TextView puanTxt = pcw.FindViewById<TextView>(Resource.Id.puanTxt);
                    puanTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(50));
                    p = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    p.Gravity = GravityFlags.Center;
                    _ = f.px(20);
                    p.SetMargins(_, _, _, _);
                    puanTxt.LayoutParameters = p;
                    puanTxt.RequestLayout();


                    gecildiTxt.SetTypeface(gecildiTxt.Typeface, TypefaceStyle.Bold);
                    gecildiTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(80));
                    gecildiTxt.Text = y.bolumTamamlandi;

                    puanTxt.SetTypeface(puanTxt.Typeface, TypefaceStyle.Bold);
                    puanTxt.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(60));
                    puanTxt.Text = y.p(y.skor, toplamPuan);


                    int yildiz;
                    if (toplamPuan <= 400) yildiz = Resource.Drawable.yildiz1;
                    else if (toplamPuan <= 700) yildiz = Resource.Drawable.yildiz2;
                    else yildiz = Resource.Drawable.yildiz3;

                    yildizImg.SetImageResource(yildiz);


                    Button bolumlereDonBtn = pcw.FindViewById<Button>(Resource.Id.bolumlereDonBtn);
                    bolumlereDonBtn.SetBackgroundResource(Resource.Drawable.p_ana);//Bölümlere Dön
                    bolumlereDonBtn.Click += async delegate
                    {
                        f.sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();

                        await geriCik();
                    };

                    Button tekrarBtn = pcw.FindViewById<Button>(Resource.Id.tekrarBtn);
                    tekrarBtn.SetBackgroundResource(Resource.Drawable.p_tekrar);//Tekrar Başlat
                    tekrarBtn.Click += delegate
                    {
                        f.sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();

                        Button tekrarBaslatBtn = FindViewById<Button>(Resource.Id.tekrarBaslatBtn);
                        tekrarBaslatBtn.CallOnClick();
                    };

                    Button sonrakiBolumBtn = pcw.FindViewById<Button>(Resource.Id.sonrakiBolumBtn);
                    sonrakiBolumBtn.SetBackgroundResource(Resource.Drawable.p_sonraki);//Sonraki Bölüm
                    sonrakiBolumBtn.Click += delegate
                    {
                        f.sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();

                        Window.AddFlags(WindowManagerFlags.NotTouchable);

                        f.gecisReklamiGosterVeGec(soruID + 1);
                    };

                    Button kapatBtn = pcw.FindViewById<Button>(Resource.Id.kapatBtn);
                    kapatBtn.SetBackgroundResource(Resource.Drawable.p_x);//X
                    kapatBtn.Click += delegate
                    {
                        f.sesEfektiCal("butonTiklandi");

                        popupWindow.Dismiss();
                    };

                    popupWindow.DismissEvent += delegate
                    {
                        RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;

                        //klavyeyi gizliyoruz
                        LinearLayout klavye = FindViewById<LinearLayout>(Resource.Id.klavye);
                        klavye.Visibility = ViewStates.Gone;

                        //tekrar başlatma seçeneklerini yüklüyoruz
                        LinearLayout tekrarLayout = FindViewById<LinearLayout>(Resource.Id.tekrarLayout);
                        tekrarLayout.Visibility = ViewStates.Visible;

                        anaView.Foreground = varsayilanArkaplan;
                        anaView.RemoveView(rl);
                        f.UnbindDrawables(pcw);
                        Window.ClearFlags(WindowManagerFlags.NotTouchable);
                    };


                    popupWindow.ShowAtLocation(anaView, GravityFlags.Center, 0, 0);
                    f.BuPencereAcildi(popupWindow);

                    isleniyor = false;
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }
        public async Task harflerYanlisAnimasyonu()
        {
            try
            {
                //await kısmında pencere değiştirilirse hata vermemesi için sadece bulmaca ekranında aç
                if (f.c is bulmacaEkrani)
                {
                    f.sesEfektiCal("yanlisCevap");


                    LinearLayout cevapKutulari = FindViewById<LinearLayout>(Resource.Id.cevapKutulari);


                    ScrollView soruScroll = FindViewById<ScrollView>(Resource.Id.soruScroll);
                    soruScroll.Enabled = false;


                    //harf kutularını deaktif yapıyoruz ve focusları temizliyoruz
                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;


                        haT.Enabled = false;
                    }


                    //tüm harflerin aynı anda kırmızı yanıp sönmesi
                    for (int i2 = 0; i2 < 2; i2++)
                    {
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            //Alınan harflere dokunmuyoruz aynen kalıyor
                            if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                            {
                                LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                                EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                                haT.SetTextColor(r.kutuYaziR);
                                haT.SetBackgroundColor(r.kutuArkaplanR);
                            }

                        }

                        await Task.Delay(200);

                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                            {
                                LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                                EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                                haT.SetTextColor(r.harflerYanlisYaziR);
                                haT.SetBackgroundColor(r.harflerYanlisArkaplanR);
                            }

                        }

                        await Task.Delay(200);
                    }

                    //tüm harf kutularını temizliyoruz ve default renklerine döndürüp aktif hale getiriyoruz
                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                        {
                            LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                            EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                            haT.Enabled = true;
                            haT.SetTextColor(r.kutuYaziR);
                            haT.SetBackgroundColor(r.kutuArkaplanR);

                            //eğer yanlış kelimeyi sil işaretliyse siliyoruz
                            if (f.oAyarlariTBL_.yanlisKelimeSil)
                                haT.EditableText?.Clear();
                        }
                    }


                    //bazı alınan harfler renklerine dönmüyor: onlara ek kontrol yapıyoruz enabled false olanları tekrar bilinen harf rengine döndürüyoruz
                    for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                    {
                        string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                        string kutuID = satirdakiKutuIDleri[i].ToString();

                        LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                        EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                        if (haT.Enabled == false)
                        {
                            haT.SetTextColor(r.harfAlYaziR);
                            haT.SetBackgroundColor(r.harfAlArkaplanR);
                        }
                    }

                    //eğer yanlış kelimeyi sil işaretli ise ilk kutuya focuslan, işaretli değilse focus son kutuda kalsın
                    if (f.oAyarlariTBL_.yanlisKelimeSil)
                    {
                        //Bilinen harfler listesinde olanlara focuslamıyoruz (bilinen harfler dışındaki ilk aktif kutuyu bul ve focuslan)
                        for (int i = 0; i < satirdakiKutuIDleri.Count; i++)
                        {
                            string satirID = kutuIDsiHangiSatiraAit[i].ToString();
                            string kutuID = satirdakiKutuIDleri[i].ToString();

                            //Alınan harflere dokunmuyoruz aynen kalıyor
                            if (bilinenHarfler.IndexOf(satirID + "," + kutuID) == -1)
                            {
                                //yanlış kelimeyi sil işaretli değil ise son harfi sil, bir sonrakine focuslan
                                /*if (!fonks.oAyarlariTBL_.yanlisKelimeSil)
                                {
                                    LinearLayout csL = cevapKutulari.FindViewWithTag(satirID) as LinearLayout;
                                    EditText haT = csL.FindViewWithTag(satirID + "," + kutuID) as EditText;

                                    haT.EditableText?.Clear();
                                }*/

                                sonSeciliKutuID = satirID + "," + kutuID;

                                break;
                            }

                        }
                    }
                    else
                    {
                        //yanlış kelime sil kapalı ise son kutuya focus at
                        string[] bolx = sonSeciliKutuID.Split(',');
                        string satirIDx = bolx[0];

                        LinearLayout csLl = cevapKutulari.FindViewWithTag(satirIDx) as LinearLayout;
                        EditText haTl = csLl.FindViewWithTag(sonSeciliKutuID) as EditText;

                        kutuSec(cevapKutulari, haTl);
                    }


                    butonlarinDurumunuDegistir(true);
                    klavyeDurumuDegistir(true);

                    soruScroll.Enabled = true;

                    yanlisCevapVerildi = false;

                    f.c.Window.ClearFlags(WindowManagerFlags.NotTouchable);
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        static string hangiHarflerAcildiBul()
        {
            try
            {
                string hangiHarflerAcildi = "";
                foreach (string bilinenHarf in bilinenHarfler)
                {
                    if (bilinenHarf != "") hangiHarflerAcildi += bilinenHarf + "|";
                }
                //son | harfini sil
                if (hangiHarflerAcildi != "") hangiHarflerAcildi = hangiHarflerAcildi.Substring(0, hangiHarflerAcildi.Length - 1);

                return hangiHarflerAcildi;
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return "";
            }
        }

        static string acilan_harflerBul()
        {
            try
            {
                string acilan_harfler = "";

                foreach (int acilanharf in acilanHarfler)
                {
                    if (acilanharf.ToString() != "") acilan_harfler += acilanharf.ToString() + "|";
                }
                //son | harfini sil
                if (acilan_harfler != "") acilan_harfler = acilan_harfler.Substring(0, acilan_harfler.Length - 1);

                return acilan_harfler;
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return "";
            }
        }

        public static async Task<bool> soruIstatistikEkle()
        {
            try
            {
                bool sonuc = false;

                string hangiHarflerAcildi = hangiHarflerAcildiBul();
                string acilan_harfler = acilan_harflerBul();

                Dictionary<string, string> veriler = new Dictionary<string, string>{
                    { "_d", "sie"},
                    { "_p0", soruID.ToString()},
                    { "_p1", bolumGecildi.ToString()},
                    { "_p2", hangiHarflerAcildi},
                    { "_p3", toplamSure.ToString()},
                    { "_p4", yanlisDenemeSayisi.ToString()},
                    { "_p6", gorselSahneSayisi.ToString()},
                    { "_p7", suAnkiGorselSahneSuresi.ToString()},
                    { "_p8", uzaklastirmaSayisi.ToString()},
                    { "_p9", harfAlmaSayisi.ToString()},
                    { "_p10", kelimeAcildi.ToString()},
                    { "_p11", toplamPuan.ToString()},
                    { "_p12", acilan_harfler},
                    { "_p13", f.googleProfile_.Id}
                };

                var t = Task.Run(() => f.SendPostToURI(f.server_main_link, veriler));
                await t.ConfigureAwait(false);
                if (t.Result == "ok")
                {
                    sonuc = true;
                }
                else
                {
                    //veriler kaydedilemediyse
                    //işlemi tekrarla -- muhtemel kilitlenme?
                    await soruIstatistikEkle();
                }


                if (sonuc == true && f.bakimAktif)
                {
                    f.anaEkranaDonVeMesajGoster(y.sunucuBakimda);
                    return true;
                }

                return sonuc;
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return false;
            }
        }
        public static async Task<bool> soruIstatistikGuncelle(string ne)
        {
            try
            {
                bool sonuc = true;
                if (f.bolumGecilmemisse(soruID))
                {
                    sonuc = false;

                    //soru eklenmemişse önce soruyu ekliyoruz
                    if (f.soruIstatistikTBL_ == null && !soruIstatistikEklendi)
                    {
                        var tx = Task.Run(() => soruIstatistikEkle());
                        await tx.ConfigureAwait(false);
                        if (tx.Result == true)
                        {
                            soruIstatistikEklendi = true;
                            return true;
                        }
                        else return false;
                    }

                    string hangiHarflerAcildi = hangiHarflerAcildiBul();
                    string acilan_harfler = acilan_harflerBul();

                    //burada değişen olaylara göre güncelleme yapıyoruz ki, sunucu yükü azalsın
                    Dictionary<string, string> veriler = null;

                    if (ne == "hepsi")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig"},
                        { "_p0", soruID.ToString()},
                        { "_p1", bolumGecildi.ToString()},
                        { "_p2", hangiHarflerAcildi},
                        { "_p3", toplamSure.ToString()},
                        { "_p4", yanlisDenemeSayisi.ToString()},
                        { "_p6", gorselSahneSayisi.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p8", uzaklastirmaSayisi.ToString());
                        veriler.Add("_p9", harfAlmaSayisi.ToString());
                        veriler.Add("_p10", kelimeAcildi.ToString());
                        veriler.Add("_p11", toplamPuan.ToString());
                        veriler.Add("_p12", acilan_harfler);
                        veriler.Add("_p13", f.googleProfile_.Id);
                    }
                    else if (ne == "geriCik")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig2"},
                        { "_p0", soruID.ToString()},
                        { "_p3", toplamSure.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p13", f.googleProfile_.Id);
                    }
                    else if (ne == "sahneBitti")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig3"},
                        { "_p0", soruID.ToString()},
                        { "_p3", toplamSure.ToString()},
                        { "_p6", gorselSahneSayisi.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p13", f.googleProfile_.Id);
                    }
                    else if (ne == "gorselUzaklastirildi")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig4"},
                        { "_p0", soruID.ToString()},
                        { "_p3", toplamSure.ToString()},
                        { "_p6", gorselSahneSayisi.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p8", uzaklastirmaSayisi.ToString());
                        veriler.Add("_p13", f.googleProfile_.Id);
                    }
                    else if (ne == "harfAlindi")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig5"},
                        { "_p0", soruID.ToString()},
                        { "_p2", hangiHarflerAcildi},
                        { "_p3", toplamSure.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p9", harfAlmaSayisi.ToString());
                        veriler.Add("_p12", acilan_harfler);
                        veriler.Add("_p13", f.googleProfile_.Id);
                    }
                    else if (ne == "yanlisCevapVerildi")
                    {
                        veriler = new Dictionary<string, string>{
                        { "_d", "sig6"},
                        { "_p0", soruID.ToString()},
                        { "_p3", toplamSure.ToString()},
                        { "_p4", yanlisDenemeSayisi.ToString()}
                    };

                        if (soruID <= 150)
                            veriler.Add("_p7", suAnkiGorselSahneSuresi.ToString());

                        veriler.Add("_p13", f.googleProfile_.Id);
                    }

                    var t = Task.Run(() => f.SendPostToURI(f.server_main_link, veriler));
                    await t.ConfigureAwait(false);
                    if (t.Result == "ok")
                    {
                        sonuc = true;
                    }
                    else
                    {
                        //veriler kaydedilemediyse
                        //işlemi tekrarla -- muhtemel kilitlenme?
                        await soruIstatistikGuncelle(ne);
                    }

                }

                if (sonuc == true && f.bakimAktif)
                {
                    f.anaEkranaDonVeMesajGoster(y.sunucuBakimda);
                }

                return sonuc;
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return false;
            }
        }

    }


}