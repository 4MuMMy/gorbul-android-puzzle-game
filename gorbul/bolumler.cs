using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace gorbul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class bolumler : AppCompatActivity
    {
        public const int bolumBasinaSoruSayisi = 40;
        private int
            sayfaBaslangicNo = 1,
            sayfaBitisNo,
            suAnkiSayfa = -1,
            toplamSayfaSayisi,
            sonGecilenSoruID = -1;
        private string bolumAdi;
        private Bitmap gorsel;
        private bool basarimKontrolR;

        System.Timers.Timer sandikSayaci;

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
        public override void OnBackPressed() { }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.bolumler);

                f.c = this;

                f.yukleniyorOlustur();

                f.boyutlariOlustur("bolumler");
                f.ortakAyarlar(Window);

                bolumlerYukle();

                //başarım ayarları yükle
                f.PlayGames__.Yukle();
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }

        }

        async Task<Bitmap> bolumGorseliCek()
        {
            try
            {
                Bitmap gorsel = null;

                //link şu şekilde olmalı: "/sorular/1/1.jpg" , "/sorular/1/2.jpg" , "/sorular/1/3.jpg"
                //yani klasör uygulamadaki sorunun idsini, sonraki gorsellerde gorsel sahne idsini belirliyor
                string dosyaAdi = suAnkiSayfa + ".jpg";

                Stream localFile = null;
                /*try
                {
                    localFile = Assets.Open("bolumGorselleri/" + dosyaAdi);
                }
                catch
                {
                    localFile = null;
                }*/

                if (localFile == null)
                {
                    string url = "";
                    if (f.hasServerTime)
                        url = f.server_bolum_gorseli_link + f.tokenOlustur() + "/" + dosyaAdi;

                    //localde görseller bulunmuyorsa yani yeni bir soru veritabanından eklenmişse, görselleri sunucudan çekiyoruz
                    var t = Task.Run(() => f.GetImageBitmapFromUrl(url));
                    await t.ConfigureAwait(false);
                    if (t.Result != null)
                    {
                        gorsel = t.Result;
                    }
                    else
                    {
                        return null;
                    }
                    t.Dispose();
                }
                else gorsel = BitmapFactory.DecodeStream(localFile);

                if (localFile != null) localFile.Dispose();

                if (gorsel == null)
                {
                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                    return null;
                }

                return gorsel;
            }
            catch (Exception ex)
            {
                f.hata(new string[1] { y.gorselAlinamadi + " :: " + ex });
                return null;
            }
        }

        async void bolumlerYukle()
        {
            try
            {
                if (!await arkaplandaYap()) return;


                f.ustMenuOlustur("bolumler", suAnkiSayfa, bolumAdi);

                ImageView bolumGorseli = FindViewById<ImageView>(Resource.Id.bolumGorseli);

                if (suAnkiSayfa != 1)
                {
                    var p3 = new RelativeLayout.LayoutParams(f.px(1080), f.px(7760));//height bolumler.cs:bolumlerYukle():141de değiştiriliyor (sayfa 1den sonrası için)
                    p3.AddRule(LayoutRules.AlignParentTop);
                    p3.AddRule(LayoutRules.CenterHorizontal);
                    bolumGorseli.LayoutParameters = p3;
                    bolumGorseli.RequestLayout();
                }

                bolumGorseli.SetImageBitmap(gorsel);

                try
                {
                    bool hasDrawable = (bolumGorseli.Drawable != null);
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

                f.basarimKontrolUI(basarimKontrolR);

                Button oncekiBolumlerBtn = FindViewById<Button>(Resource.Id.oncekiBolumlerBtn);
                oncekiBolumlerBtn.Text = y.oncekiBolumler_btn;

                //0,40 arasındaki sorular yani ilk bölümse önceki butonunu gizle
                if (suAnkiSayfa == 1)
                {
                    oncekiBolumlerBtn.Visibility = ViewStates.Gone;
                }

                int oncekiSayfa = suAnkiSayfa - 1;
                if (oncekiSayfa <= 0) oncekiSayfa = 1;

                oncekiBolumlerBtn.Click += (sender, args) =>
                {
                    f.sesEfektiCal("butonTiklandi");

                    oncekiBolumlerBtn.Enabled = false;

                    veriGonder vg3 = new veriGonder();
                    vg3.gonderilecekVeriAdi = "suAnkiSayfa";
                    vg3.gonderilecekVeri = oncekiSayfa;

                    f.ekranGecisi(typeof(bolumler), vg3);
                    Finish();
                };


                Button sonrakiBolumlerBtn = FindViewById<Button>(Resource.Id.sonrakiBolumlerBtn);
                sonrakiBolumlerBtn.Text = y.sonrakiBolumler_btn;

                int sonrakiSayfa = suAnkiSayfa + 1;
                if (sonrakiSayfa > toplamSayfaSayisi) sonrakiSayfa = toplamSayfaSayisi;

                //son sayfa ise butonu deaktif yap
                if (toplamSayfaSayisi == suAnkiSayfa)
                {
                    sonrakiBolumlerBtn.Enabled = false;
                    sonrakiBolumlerBtn.LayoutParameters.Width = f.px(700);
                    sonrakiBolumlerBtn.Text = y.yeniBolumlerYakinda;
                    sonrakiBolumlerBtn.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(45));
                }

                sonrakiBolumlerBtn.Click += (sender, args) =>
                {
                    f.sesEfektiCal("butonTiklandi");

                    sonrakiBolumlerBtn.Enabled = false;

                    veriGonder vg3 = new veriGonder();
                    vg3.gonderilecekVeriAdi = "suAnkiSayfa";
                    vg3.gonderilecekVeri = sonrakiSayfa;

                    f.ekranGecisi(typeof(bolumler), vg3);
                    Finish();
                };


                GridLayout bolumlerGrid = FindViewById<GridLayout>(Resource.Id.bolumlerGrid);

                int toplamKolonSayisi = 4;
                int toplamSatirSayisi = (int)Math.Ceiling((double)sayfaBitisNo / (double)toplamKolonSayisi);

                bolumlerGrid.AlignmentMode = GridAlign.Bounds;
                bolumlerGrid.ColumnCount = toplamKolonSayisi;
                bolumlerGrid.RowCount = 100;
                bolumlerGrid.Orientation = GridOrientation.Horizontal;


                int kolonSayaci = 0, satirSayaci = 0, gridIndex = 0, scrollSayaci = 0;
                bool ilkSatir = true, scrollSon = false;

                for (int i = (sayfaBaslangicNo - 1); i < sayfaBitisNo; i++)
                {
                    int soruID = f.sorularTBL_[i].id;

                    TextView cvp = new TextView(this);
                    cvp.Tag = soruID;
                    cvp.Text = soruID.ToString();
                    cvp.SetWidth(f.px(150));
                    cvp.SetHeight(f.px(150));
                    cvp.SetPadding(f.px(20), f.px(10), f.px(20), f.px(10));
                    cvp.SetTextColor(r.bolumKutusuDeaktifYaziR);
                    cvp.SetBackgroundColor(r.bolumKutusuDeaktifArkaplanR);
                    cvp.Enabled = false;

                    if (soruID == 1)
                    {
                        cvp.SetTextColor(r.bolumKutusuYaziR);
                        cvp.SetBackgroundColor(r.bolumKutusuArkaplanR);
                        cvp.Enabled = true;
                    }

                    //bir önceki bölüm geçildiyse şimdiki bölümü açılabilir yap.
                    int birOncekiBolumID = soruID - 1;
                    if (birOncekiBolumID == 0) birOncekiBolumID = 1;

                    soruIstatistikIDleriTBL birOncekiBolum = f.soruIstIDleriBul(birOncekiBolumID);
                    if (birOncekiBolum != null)
                    {
                        if (birOncekiBolum.bolumGecildi)
                        {
                            cvp.SetTextColor(r.bolumKutusuYaziR);
                            cvp.SetBackgroundColor(r.bolumKutusuArkaplanR);
                            cvp.Enabled = true;
                        }
                    }

                    //şimdiki bölüm geçildi ise geçildi olarak işaretle
                    soruIstatistikIDleriTBL sr = f.soruIstIDleriBul(soruID);
                    if (sr != null)
                    {
                        if (sr.bolumGecildi)
                        {
                            cvp.SetTextColor(r.bolumKutusuBolumGecildiYaziR);
                            cvp.SetBackgroundColor(r.bolumKutusuBolumGecildiArkaplanR);
                            cvp.Enabled = true;
                        }
                    }


                    cvp.SetTextSize(global::Android.Util.ComplexUnitType.Px, f.px(60));
                    cvp.SetTypeface(cvp.Typeface, TypefaceStyle.Bold);
                    cvp.LongClickable = false;
                    cvp.TextAlignment = TextAlignment.Gravity;
                    cvp.Gravity = GravityFlags.Center;


                    GridLayout.LayoutParams param = new GridLayout.LayoutParams();
                    param.Height = GridLayout.LayoutParams.WrapContent;
                    param.Width = GridLayout.LayoutParams.WrapContent;
                    param.RightMargin = f.px(15);
                    param.TopMargin = f.px(15);
                    param.SetGravity(GravityFlags.Center);


                    cvp.LayoutParameters = param;

                    if (ilkSatir)
                    {
                        int boslukAdedi = 0;

                        if (kolonSayaci == 0) boslukAdedi = 0;
                        else if (kolonSayaci == 1) boslukAdedi = 1;
                        else if (kolonSayaci == 2) boslukAdedi = 2;
                        else if (kolonSayaci == 3) boslukAdedi = 3;


                        for (int i2 = 0; i2 < boslukAdedi; i2++)
                        {
                            TextView bosluk = new TextView(this);
                            bosluk.SetWidth(f.px(165));
                            bosluk.SetHeight(f.px(165));
                            bolumlerGrid.AddView(bosluk, gridIndex);
                            gridIndex++;
                        }
                    }
                    else
                    {
                        int boslukAdedi = 0;

                        if (kolonSayaci == 0) boslukAdedi = 2;
                        else if (kolonSayaci == 1) boslukAdedi = 1;
                        else if (kolonSayaci == 2) boslukAdedi = 0;
                        else if (kolonSayaci == 3) boslukAdedi = 1;


                        for (int i2 = 0; i2 < boslukAdedi; i2++)
                        {
                            TextView bosluk = new TextView(this);
                            bosluk.SetWidth(f.px(165));
                            bosluk.SetHeight(f.px(165));
                            bolumlerGrid.AddView(bosluk, gridIndex);
                            gridIndex++;
                        }
                    }


                    bolumlerGrid.AddView(cvp, gridIndex);
                    gridIndex++;


                    kolonSayaci++;


                    if (ilkSatir)
                    {
                        int boslukAdedi = 0;

                        if (kolonSayaci == 1)
                        {
                            if (satirSayaci == 0) boslukAdedi = 3;
                            else boslukAdedi = 1;
                        }
                        else if (kolonSayaci == 2) boslukAdedi = 2;
                        else if (kolonSayaci == 3) boslukAdedi = 1;

                        if (kolonSayaci != 4)
                        {
                            for (int i2 = 0; i2 < boslukAdedi; i2++)
                            {
                                TextView bosluk = new TextView(this);
                                bosluk.SetWidth(f.px(165));
                                bosluk.SetHeight(f.px(165));
                                bolumlerGrid.AddView(bosluk, gridIndex);
                                gridIndex++;
                            }
                        }
                    }
                    else
                    {
                        int boslukAdedi = 0;

                        if (kolonSayaci == 1) boslukAdedi = 1;
                        else if (kolonSayaci == 2) boslukAdedi = 2;
                        else if (kolonSayaci == 3) boslukAdedi = 3;
                        else if (kolonSayaci == 4) boslukAdedi = 2;

                        for (int i2 = 0; i2 < boslukAdedi; i2++)
                        {
                            TextView bosluk = new TextView(this);
                            bosluk.SetWidth(f.px(165));
                            bosluk.SetHeight(f.px(165));
                            bolumlerGrid.AddView(bosluk, gridIndex);
                            gridIndex++;
                        }
                    }


                    if (kolonSayaci == toplamKolonSayisi)
                    {
                        if (ilkSatir) ilkSatir = false;
                        else ilkSatir = true;

                        satirSayaci++;
                        kolonSayaci = 0;
                    }


                    if (satirSayaci == 1 && kolonSayaci == 2)
                    {
                        ilkSatir = true;
                        satirSayaci = 0;
                        kolonSayaci = 0;
                    }


                    cvp.Touch += async (sender, args) =>
                    {
                            //sadece basma durumunda çalışmalı, diğer türlü tüm durumlarda çalışıyor up, move gibi
                            if ((args as TextView.TouchEventArgs).Event.Action == MotionEventActions.Down)
                        {
                            Window.AddFlags(WindowManagerFlags.NotTouchable);
                            cvp.Enabled = false;

                            f.tumNesneleriDeaktifEt();

                            f.sesEfektiCal("butonTiklandi");

                            Color oncekiRenk = new Color(cvp.CurrentTextColor);
                            Color oncekiRenk2 = (cvp.Background as ColorDrawable).Color;

                            cvp.SetTextColor(r.bolumKutusuBasildigindakiArkaplanR);
                            cvp.SetBackgroundColor(r.bolumKutusuBasildigindakiYaziR);

                            await Task.Delay(100);

                            cvp.SetTextColor(oncekiRenk);
                            cvp.SetBackgroundColor(oncekiRenk2);


                            f.bolumuBaslat(soruID, true);
                        }
                    };

                    if (!scrollSon) scrollSayaci++;
                    if (soruID == sonGecilenSoruID) scrollSon = true;
                }

                f.yukleniyorKaldir();

               await sandikKontroluYukle();

                //scrollview'i son geçilen bölüme indiriyoruz
                if (sonGecilenSoruID != -1 && scrollSon)
                {
                    await Task.Delay(200);

                    ScrollView bolumlerScroll = FindViewById<ScrollView>(Resource.Id.bolumlerScroll);
                    int y = (scrollSayaci * f.px(162));
                    bolumlerScroll.SmoothScrollTo(0, y);
                }

            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        async Task<bool> arkaplandaYap()
        {
            try
            {
                sayfaBitisNo = bolumBasinaSoruSayisi;

                if (Intent.GetStringExtra("suAnkiSayfa") != null)
                {
                    suAnkiSayfa = Convert.ToInt32(JsonConvert.DeserializeObject<string>(Intent.GetStringExtra("suAnkiSayfa")));
                }

                int toplamSoruSayisi = f.sorularTBL_.Length;

                toplamSayfaSayisi = (int)Math.Ceiling((double)toplamSoruSayisi / (double)bolumBasinaSoruSayisi);

                //Soru istatistik id ve bolumgecildi değerlerini çekiyoruz
                var t2x = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=ssc&_g=" + f.googleProfile_.Id));
                await t2x.ConfigureAwait(false);
                if (t2x.Result != null) f.soruIstatistikIDleriTBL_ = JsonConvert.DeserializeObject<soruIstatistikIDleriTBL[]>(t2x.Result);
                else
                {
                    f.anaEkranaDonVeMesajGoster(y.bolumlerYuklenemedi);
                    return false;
                }
                t2x.Dispose();

                await Task.Delay(1);

                //en son eklenen son soru değilse 1 çünkü bölüm geçmeden bir sonraki bölüme geçemez

                if (f.soruIstatistikIDleriTBL_ != null)
                {
                    var sonSoru = f.soruIstatistikIDleriTBL_[f.soruIstatistikIDleriTBL_.Length - 1];

                    if (sonSoru.bolumGecildi)
                    {
                        sonGecilenSoruID = sonSoru.soruID;
                    }
                    else
                    {
                        int ix = f.soruIstatistikIDleriTBL_.Length - 2;
                        if (ix == -1) ix = 0;
                        sonGecilenSoruID = f.soruIstatistikIDleriTBL_[ix].soruID;
                    }
                }

                if (suAnkiSayfa == -1)
                {
                    if (sonGecilenSoruID != -1)
                        suAnkiSayfa = (int)Math.Ceiling(((double)sonGecilenSoruID / (double)bolumBasinaSoruSayisi));
                    else suAnkiSayfa = 1;

                    //bu bölümleri bitirdiyse diğer bölümler sayfasına geç
                    if (sonGecilenSoruID % bolumBasinaSoruSayisi == 0)
                    {
                        //eğer kategorinin son sorusu ise ve başka hiç soru yoksa diğer sayfaya atlama
                        double sonMu = ((double)toplamSoruSayisi) / ((double)suAnkiSayfa);
                        if (sonMu != bolumBasinaSoruSayisi)
                        {
                            suAnkiSayfa++;
                        }
                    }
                }

                sayfaBitisNo = suAnkiSayfa * bolumBasinaSoruSayisi;
                if (sayfaBitisNo > toplamSoruSayisi)
                {
                    sayfaBitisNo = sayfaBitisNo - toplamSoruSayisi;
                    sayfaBaslangicNo = toplamSoruSayisi - ((sayfaBitisNo - bolumBasinaSoruSayisi) * -1) + 1;
                    sayfaBitisNo = toplamSoruSayisi;
                }
                else
                {
                    sayfaBaslangicNo = (sayfaBitisNo - bolumBasinaSoruSayisi) + 1;
                }

                if (sayfaBaslangicNo <= 0) sayfaBaslangicNo = 1;

                f.toplamGecilenBolumSayisi = 0;
                //toplam geçilen bölüm sayısını buluyoruz
                if (f.soruIstatistikIDleriTBL_ != null)
                {
                    foreach (var bolum in f.soruIstatistikIDleriTBL_)
                    {
                        if (bolum.bolumGecildi) f.toplamGecilenBolumSayisi++;
                    }
                }

                foreach (var bolum in f.bolumlerTBL_)
                {
                    if (bolum.id == suAnkiSayfa) bolumAdi = bolum.bolumAdi;
                }

                await Task.Delay(1);

                //Jeton bilgileri çekiliyor
                var t4 = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=jt&_g=" + f.googleProfile_.Id));
                await t4.ConfigureAwait(false);
                if (t4.Result != null) f.jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t4.Result);
                else
                {
                    f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                    return false;
                }
                t4.Dispose();

                await Task.Delay(1);

                //İlk giriş ise yeni oyuncunun jeton bilgilerini ekliyoruz
                if (f.jetonlarTBL_ == null)
                {
                    Dictionary<string, string> veriler = new Dictionary<string, string>{
                    { "_d", "jto"},
                    { "_p1", f.googleProfile_.Id}
                    };

                    var t5 = Task.Run(() => f.SendPostToURI(f.server_main_link, veriler));
                    await t5.ConfigureAwait(false);
                    if (t5.Result == "ok")
                    {
                        t5.Dispose();
                        //yeni eklenen jeton verilerini tekrar çekiyoruz
                        var t4_ = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=jt&_g=" + f.googleProfile_.Id));
                        await t4_.ConfigureAwait(false);
                        if (t4_.Result != null) f.jetonlarTBL_ = JsonConvert.DeserializeObject<jetonlarTBL>(t4_.Result);
                        else
                        {
                            f.anaEkranaDonVeMesajGoster(y.baziVerilerYuklenemedi);
                            return false;
                        }
                        t4_.Dispose();
                    }
                    else
                    {
                        //veriler kaydedilemediyse
                        f.anaEkranaDonVeMesajGoster(y.verilerSunucuyaKaydedilemiyor);
                        return false;
                    }
                }

                //jetonlar hala boşsa hata var
                if (f.jetonlarTBL_ == null)
                {
                    f.anaEkranaDonVeMesajGoster(y.verilerSunucuyaKaydedilemiyor);
                    return false;
                }

                gorsel = await bolumGorseliCek();

                if (gorsel == null)
                {
                    f.anaEkranaDonVeMesajGoster(y.gorselAlinamadi);
                    return false;
                }

                //başarımları kontrol ediyoruz
                basarimKontrolR = await f.basarimKontrol();

                return true;
            }
            catch (Exception ex)
            {
                f.hata(ex);
                return false;
            }
        }


        async Task sandikKontroluYukle()
        {
            ViewGroup anaView = Window.DecorView.RootView as ViewGroup;
            Drawable varsayilanArkaplan = anaView.Foreground;

            if (f.sandikKontrolEdilmedi)
            {
                //Günlük sandık kontrolünü yapıyoruz - eğer sandık zamanı geldiyse işlemler için sandık sayacını başlatıyoruz

                if (await f.gunlukSandikKontrolu())
                {
                    //eğer günlük sandık açma zamanı gelmişse önce işlemler için timerı tanımlıyoruz ve googleadsi yüklüyoruz

                    sandikSayaci = new System.Timers.Timer();
                    sandikSayaci.Interval = 1000;
                    sandikSayaci.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                    {
                        RunOnUiThread(async () =>
                        {
                            if (f.sandikJetonR == "kapatildi")
                            {
                                sandikSayaci.Stop();
                                anaView.RemoveView(anaView.FindViewWithTag("loading"));
                                anaView.Foreground = varsayilanArkaplan;
                                Window.ClearFlags(WindowManagerFlags.NotTouchable);
                                Window.SetSoftInputMode(SoftInput.StateHidden);
                                f.ortakAyarlar(Window);

                                f.mesajGoster(y.reklamKapatildi);

                                f.sandikJetonR = null;
                                f.sandikIslem = null;
                                f.sandikKontrolEdilmedi = false;
                                sandikSayaci.Dispose();
                            }
                            else if (f.sandikJetonR == "yuklenemedi")
                            {
                                sandikSayaci.Stop();
                                anaView.RemoveView(anaView.FindViewWithTag("loading"));
                                anaView.Foreground = varsayilanArkaplan;
                                Window.ClearFlags(WindowManagerFlags.NotTouchable);
                                Window.SetSoftInputMode(SoftInput.StateHidden);
                                f.ortakAyarlar(Window);

                                f.mesajGoster(y.reklamYuklenemedi);

                                f.sandikJetonR = null;
                                f.sandikIslem = null;
                                f.sandikKontrolEdilmedi = false;
                                sandikSayaci.Dispose();
                            }
                            else if (f.sandikIslem == y.bitti_btn)
                            {
                                sandikSayaci.Stop();
                                Window.ClearFlags(WindowManagerFlags.NotTouchable);
                                Window.SetSoftInputMode(SoftInput.StateHidden);
                                f.ortakAyarlar(Window);

                                f.sandikKontrolEdilmedi = false;
                                sandikSayaci.Dispose();
                            }

                            //sandık işlemleri varsa bittikten sonra mağaza ayarlarını yüklüyoruz
                            if (f.sandikJetonR == "kapatildi" || f.sandikJetonR == "yuklenemedi" || f.sandikIslem == y.bitti_btn)
                            {
                                //mağazayı yüklüyoruz
                                await f.magazaAyarlariYukle();
                            }
                        });
                    };

                    await f.sandikGoster();

                    sandikSayaci.Start();
                }
                else
                {
                    //sandık işlemleri yoksa direk mağaza ayarlarını yüklüyoruz

                    //mağazayı yüklüyoruz
                    await f.magazaAyarlariYukle();
                }
            }
        }

    }
}