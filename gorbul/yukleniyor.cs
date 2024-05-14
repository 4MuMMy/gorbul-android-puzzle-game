using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Newtonsoft.Json;

namespace gorbul
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class yukleniyor : AppCompatActivity
    {
        protected override void OnDestroy()
        {
            base.OnDestroy();
            f.temizlikYap();
        }
        public override void OnBackPressed() { }//disable back
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                f.c = this;

                f.ortakAyarlar(Window);

                f.yukleniyorOlustur();

                ekranGecisiYap();

            }
            catch (Exception ex)
            {
                f.hata(ex);
                return;
            }
        }

        async void ekranGecisiYap()
        {
            try
            {
                await Task.Delay(100);

                if (Intent.Extras != null)
                {
                    Type gecilecekEkran = JsonConvert.DeserializeObject<Type>(Intent.GetStringExtra("gecilecekEkran"));
                    Intent intent = new Intent(ApplicationContext, gecilecekEkran);
                    intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                    var ekVeriler = Intent.GetStringExtra("ekVeriler");

                    if (ekVeriler != null)
                    {
                        veriGonder[] vg = JsonConvert.DeserializeObject<veriGonder[]>(ekVeriler);

                        foreach (var veri in vg)
                        {
                            intent.PutExtra(veri.gonderilecekVeriAdi, JsonConvert.SerializeObject(veri.gonderilecekVeri));
                        }

                    }

                    f.gorselleriTemizle();

                    if (this != null)
                        FinishAffinity();

                    StartActivity(intent);
                    OverridePendingTransition(0, 0);
                    FinishAfterTransition();
                }
            }
            catch (Exception ex)
            {
                f.hata(ex);
            }
        }

    }
}