using System;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace gorbul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class acilisEkrani : AppCompatActivity
    {
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
                SetContentView(Resource.Layout.acilisEkrani);

                //Eğer uygulama Play Store vb. yerlerden 2. kez başlatılırsa,
                //problem çıkmaması için tüm geçmişi temizlemek çok zor olduğu için uygulamayı kapatıyoruz,
                //kullanıcı tekrar açtığında ise problem yaşanmıyor.
                if (f._version != "") f.uygulamayiKapat();

                f.c = this;

                f.cihazCozunurlukAyarlari();

                f.boyutlariOlustur("acilisEkrani");

                f.ortakAyarlar(Window);

                MuMMy();
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }

        void getVersion()
        {
            var p = PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0);
            string v = "";

            #pragma warning disable 0618
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                v = p.VersionName + "." + p.LongVersionCode;
            else
                v = p.VersionName + "." + p.VersionCode;
            #pragma warning restore 0618

            f._version = v;
        }

        public async void MuMMy()
        {
            getVersion();
            f.getServerTime();

            await Task.Delay(1000);

            ImageView TFpng = FindViewById<ImageView>(Resource.Id.TFpng);
            TFpng.SetImageResource(Resource.Drawable.mummy);
            TFpng.Visibility = ViewStates.Visible;

            await Task.Delay(500);

            TextView versionTxtAE = FindViewById<TextView>(Resource.Id.versionTxtAE);
            versionTxtAE.Text = y.p(y.version, f._version);
            versionTxtAE.Visibility = ViewStates.Visible;

            //oyuncu ile bağlantısı olmayan ana oyun verilerini başlangıç ekranında çekiyoruz ki başla butonunun yükü azalsın
            if (f.hasServerTime)
            {
                await f.temelVerileriCek();
            }

            //ses efektlerini çalmaya hazır hale getiriyoruz (gecikme olmaması için)
            f.sesEfektleriniYukle();

            await Task.Delay(2000);

            Intent intent = new Intent(this, typeof(anaEkran));
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);

            f.gorselleriTemizle();

            StartActivity(intent);
            OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            Finish();
        }

    }
}