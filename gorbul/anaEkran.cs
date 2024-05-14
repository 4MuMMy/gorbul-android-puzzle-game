using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Android.Gms.Common;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Newtonsoft.Json;

namespace gorbul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    class anaEkran : AppCompatActivity
    {
        Button basla;

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
        public override void OnBackPressed() { }//disable back
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.anaEkran);

                f.c = this;

                f.boyutlariOlustur("anaEkran");

                f.ortakAyarlar(Window);

                f.PlayGames__.Baslat();


                basla = FindViewById<Button>(Resource.Id.basla);
                basla.SetBackgroundResource(Resource.Drawable.btn_anaEkran);
                basla.Text = y.basla_btn;
                basla.Click += async delegate
                {
                    basla.Text = y.baslatiliyor_btn;
                    basla.Enabled = false;

                    await Task.Delay(1);

                    await baslatButonuIsle();
                };


                ImageView anaEkranCikis = FindViewById<ImageView>(Resource.Id.anaEkranCikis);
                anaEkranCikis.SetImageResource(Resource.Drawable.p_cikis);

                anaEkranCikis.Click += delegate
                {
                    f.mesajGoster(y.cikmayaEminMisiniz, () =>
                    {
                        f.uygulamayiKapat();
                    }, true, () =>
                    { }, y.cik_btn, y.cikma_btn);

                };


                //bu ekranda müziği ve sesleri varsayılan olarak durduruyoruz
                if (f.playerBG != null)
                    f.playerBG.Stop();

                f.sesEfektiDurdur();

                f.acilisTamamlandi = true;
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }

        }

        protected override void OnStart()
        {//oncreate çalıştırılıp her şey yüklendikten sonra
            base.OnStart();

            //mesaj gösterme aktifse mesaj göster
            if (Intent.GetStringExtra("mesaj") != null)
            {
                string e = JsonConvert.DeserializeObject<string>(Intent.GetStringExtra("mesaj"));

                if (e == y.guncellemeVar || e == y.yeniSurumVar)
                {
                    guncellemeMesaji(e);
                }
                else
                {
                    f.mesajGoster(e);

                    if (f.main_log != "")
                    {
                        f.logKaydet("\n\n_____________________\n\n" + f.main_log + "\n\n_____________________\n\n");
                        f.main_log = "";
                    }
                }

                Intent.RemoveExtra("mesaj");
            }
        }

        void guncellemeMesaji(string e)
        {
            f.mesajGoster(e, () =>
            {
                //güncellemesi için google play sayfasını açıyoruz
                Intent browserIntent = new Intent(Intent.ActionView,
                    Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=com.mummy.gorbul"));
                StartActivity(browserIntent);
            }, true, () =>
            {
                butonAktif();
            }, y.guncelle_btn, y.kapat_btn);
        }

        async Task baslatButonuIsle()
        {
            //server time çekilememiş ise ise şimdi çekiyoruz
            if (!f.hasServerTime)
            {
                f.getServerTime();
                if (f.hasServerTime) f.temelVerileriCek_ = true;
            }

            //server time çekilmiş ise devam et
            if (f.hasServerTime)
            {
                //ayarlar çek uygulama aktif mi? ve duyuru kontrolü
                var t = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=a"));
                await t.ConfigureAwait(false);

                await Task.Delay(10);

                if (t.Result != null)
                {
                    try
                    {
                        f.ayarlarTBL_ = JsonConvert.DeserializeObject<ayarlarTBL>(t.Result);

                        if (f.ayarlarTBL_ == null)
                        {
                            f.mesajGoster(y.ayarlarAlinamadi, butonAktif);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        f.hata(new string[1] { y.ayarlarAlinamadi + " :: " + ex }, butonAktif);
                        return;
                    }
                }
                else
                {
                    f.mesajGoster(y.ayarlarAlinamadi, butonAktif);
                    return;
                }
                t.Dispose();

                if (f.ayarlarTBL_ != null)
                {
                    if (f.ayarlarTBL_.duyuru != null && f.ayarlarTBL_.duyuru != "")
                    {
                        f.mesajGoster(f.ayarlarTBL_.duyuru, async () =>
                        {
                            await ayarlarAktifMi();
                        });
                    }
                    else
                    {
                        await ayarlarAktifMi();
                    }
                }
            }
            //çekilememişse devam etmeye gerek yok
            else f.mesajGoster(y.baglantiBasarisiz, butonAktif);
        }

        void butonAktif()
        {
            basla.Text = y.basla_btn;
            basla.Enabled = true;
        }

        async Task ayarlarAktifMi()
        {
            if (f.ayarlarTBL_.aktif == 1)
            {
                //uygulama bakımda değilse versionu da kontrol ediyoruz
                if (f.ayarlarTBL_.version == f._version)
                {
                    //version da aynı ise giriş yapıyoruz - google servislerine giriş yap

                    //daha önce giriş yapılmamışsa giriş yapıyoruz
                    if (_PlayGames.mGoogleSignInAccount == null)
                    {
                        f.PlayGames__.PlayGirisPenceresiniAc();
                    }
                    else
                    {
                        //Zaten daha önce giriş yapılmışsa direk oyunu başlatıyoruz
                        await oyunuBaslat();
                    }
                }
                else
                {
                    guncellemeMesaji(y.guncellemeVar);
                    return;
                }
            }
            else
            {
                f.bakimAktif = true;
                f.mesajGoster(y.sunucuBakimda, butonAktif);
                return;
            }
        }

        async Task oyunuBaslat()
        {
            try
            {
                var a = _PlayGames.mGoogleSignInAccount;
                f.googleProfile_ = new GoogleProfile()
                {
                    Id = a.Id,
                    DisplayName = a.DisplayName,
                    GivenName = a.GivenName,
                    FamilyName = a.FamilyName,
                    Email = a.Email,
                    PhotoUrl = a.PhotoUrl
                };

                //ban kontrolü yapılıyor
                var t = Task.Run(() => f.GetResponseFromURI(f.server_main_link + "?_d=b&_g=" + f.googleProfile_.Id));
                await t.ConfigureAwait(false);
                if (t.Result != null)
                {
                    string oA = t.Result;
                    t.Dispose();

                    //banlıysa gg
                    if (oA == "e")
                    {
                        f.mesajGoster(y.hesapYasakli, butonAktif);
                        return;
                    }
                    else
                    {
                        try
                        {
                            f.oAyarlariTBL_ = JsonConvert.DeserializeObject<oAyarlariTBL>(oA);
                        }
                        catch
                        {
                            f.mesajGoster(y.ayarlarAlinamadi);
                            return;
                        }

                        if (f.oAyarlariTBL_ == null)
                        {
                            //varsayılan oyuncu ayarları
                            f.oAyarlariTBL_ = new oAyarlariTBL();
                            f.oAyarlariTBL_.sesler = true;
                            f.oAyarlariTBL_.muzik = true;
                            f.oAyarlariTBL_.titresim = true;
                            f.oAyarlariTBL_.yanlisKelimeSil = true;
                        }

                        //Eğer bakım aktif olduysa ve bakım bittiyse temel verileri tekrar çekiyoruz ve bakımdan çıkartıyoruz
                        //eğer server time'ı yeni çekildiyse de temel verileri çekiyoruz
                        if (f.bakimAktif || f.temelVerileriCek_)
                        {
                            f.bakimAktif = false;
                            await f.temelVerileriCek();
                        }

                        //Profil görselini çekiyoruz
                        var t2e = Task.Run(() => f.GetImageBitmapFromUrl(f.googleProfile_.PhotoUrl.ToString()));
                        await t2e.ConfigureAwait(false);
                        if (t2e.Result != null)
                        {
                            try
                            {
                                f.profilGorseli_ = f.yenidenBoyutlandir(t2e.Result, 180, 180);
                            }
                            catch{}//kontrollü catch
                        }
                        t2e.Dispose();

                        //müzik açıksa başlat
                        f.muzikBaslatDurdur();

                        //reklamları yüklüyoruz
                        Android.Gms.Ads.MobileAds.Initialize(f.c.ApplicationContext);

                        f.ekranGecisi(typeof(bolumler));
                    }
                }
                else
                {
                    f.mesajGoster(y.girisYapilamadi, butonAktif);
                    return;
                }
            }
            catch(Exception ex)
            {
                f.hata(ex, butonAktif);
                return;
            }
        }

        protected override async void OnActivityResult(int requestCode, [Android.Runtime.GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            if (requestCode == f.PlayGames__.RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);

                if (result.IsSuccess)
                {
                    //oyuna ilk defa giriş yapılıyor
                    _PlayGames.mGoogleSignInAccount = result.SignInAccount;
                    await oyunuBaslat();
                }
                else
                {
                    f.mesajGoster(y.p(y.googleaGirisYapilamadi, result.Status), butonAktif);
                    return;
                    //giriş başarısız
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [Android.Runtime.GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            f.mesajGoster(y.p(y.googleaGirisHatasi, result), butonAktif);
            return;
        }
    }


}
