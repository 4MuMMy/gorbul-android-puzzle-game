using Android.Content;
using Android.Gms.Games;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Tasks;
using Android.Gms.Common;
using Android.Gms.Games.LeaderBoard;
using System;

namespace gorbul
{
	class _PlayGames : Java.Lang.Object, IOnSuccessListener, IOnCompleteListener
	{
		public static GoogleSignInClient mGoogleSignInClient;
		public static GoogleSignInAccount mGoogleSignInAccount;

		public static IGamesClient mIGamesClient;
		public static IAchievementsClient mIAchievementsClient;
		public static ILeaderboardsClient mILeaderboardsClient;

		public int RC_SIGN_IN = 9001;
		public int RC_ACHIEVEMENT_UI = 9003;


		public void Baslat()
		{
			GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultGamesSignIn)
						.RequestIdToken(f.google_firebase_account_link)
						.RequestScopes(
							new Scope(Scopes.Email),
							new Scope(Scopes.OpenId),
							new Scope(Scopes.Games),
							new Scope(Scopes.GamesLite),
							//new Scope(Scopes.CloudSave),
							new Scope(Scopes.Profile))
						.RequestServerAuthCode(f.google_firebase_account_link)
						.RequestEmail()
						.Build();
			mGoogleSignInAccount = GoogleSignIn.GetLastSignedInAccount(f.c);

			if (!GoogleSignIn.HasPermissions(mGoogleSignInAccount, gso.GetScopeArray()))
			{
				mGoogleSignInClient = GoogleSignIn.GetClient(f.c, gso);
				mGoogleSignInClient
					.SilentSignIn()
					.AddOnCompleteListener(this);
			}
		}

		public void Yukle()
        {
			mIGamesClient = GamesClass.GetGamesClient(f.c, mGoogleSignInAccount);
			mIAchievementsClient = GamesClass.GetAchievementsClient(f.c, mGoogleSignInAccount);
			mILeaderboardsClient = GamesClass.GetLeaderboardsClient(f.c, mGoogleSignInAccount);

			//oyun ekranını play games'e tanımlama (kullanıcıya gösterilecek şeyler için)
			mIGamesClient.SetViewForPopups(f.c.Window.DecorView.RootView);
		}

		public void PlayGirisPenceresiniAc()
        {
			Intent signInIntent = mGoogleSignInClient.SignInIntent;
			f.c.StartActivityForResult(signInIntent, RC_SIGN_IN);
		}

		public void OnComplete(Task t)
		{
			if (t.IsSuccessful)
			{
				if (t.Result is GoogleSignInAccount)
					mGoogleSignInAccount = t.Result as GoogleSignInAccount;
			}
		}

		public void ShowAchievements()
		{
			Yukle();
			mIAchievementsClient.GetAchievementsIntent().AddOnSuccessListener(this);
		}

		public void ShowLeaderboards()
		{
			Yukle();
			mILeaderboardsClient.GetAllLeaderboardsIntent().AddOnSuccessListener(this);
		}

		public void OnSuccess(Java.Lang.Object o)
		{
			if (o is Intent)
			{
				var i = o as Intent;
				f.c.StartActivityForResult(o as Intent, RC_ACHIEVEMENT_UI);
			}
		}

		public void skorGonder(int skor, int soruID, string bolumAdi)
		{
            foreach (var s in f.skorTablolari_)
			{
				//bu soru ortalama bulunacak soru mu kontrol ediyoruz. eğer son soru yani ortalama bulunacak soru ise
				//soruID eşitse skor tablosunun son id'sine (örneğin 1-40In "40" kısmı)
				if (s.son == soruID)
				{
					//istenilen aralıktaki tüm bölümlerin ortalama puanını bul (örneğin 1 ile 40 arasındakiler)

					int adet = 0;
					int bolumlerinPuanToplami = 0;
					int beklenenToplamSkorAdeti = s.son - s.ilk;
					//toplam puanı bul
					foreach (var si in f.soruIstatistikIDleriTBL_)
					{
						for (int i = s.ilk; i <= s.son; i++)
						{
							if (si.soruID == i)
                            {
								bolumlerinPuanToplami += si.toplamPuan;
								adet++;

								if (adet == beklenenToplamSkorAdeti) break;
							}
						}

						if (adet == beklenenToplamSkorAdeti) break;
					}

					//son geçilen bölümün skoru henüz eklenmediği için manuel ekliyoruz
					beklenenToplamSkorAdeti++;
					bolumlerinPuanToplami += skor;
					adet++;
					
					//ortalama puanı buluyoruz
					int bolumlerinOrtamaPuani = 0;
					if (adet != 1)
						bolumlerinOrtamaPuani = bolumlerinPuanToplami / adet;

					if (bolumlerinPuanToplami != 0 &&
						adet != 1 &&
						beklenenToplamSkorAdeti == adet)
					{
						mILeaderboardsClient.SubmitScoreImmediate(s.lead_id, bolumlerinOrtamaPuani, bolumAdi + soruID);
						mILeaderboardsClient.SubmitScore(s.lead_id, bolumlerinOrtamaPuani, bolumAdi + soruID);
					}
					break;
                }
            }
		}

		public void basarimAc(string achi_id)
		{
			if (achi_id != "")
			{
				mIAchievementsClient.UnlockImmediate(achi_id);
				mIAchievementsClient.Unlock(achi_id);
			}
		}
		public bool basarimlaraAdimAtlat(int soruID)
		{
			//bu soruidsinin üstündeki (açılmamış) tüm başarımları 1 adım arttırıyoruz
			//diziyi reverse yapmamızın sebebi play gameste en son gözüken başarımı bir sonraki olarak ayarlamak
			//foreach (var b in f.basarimlarTBL_.Reverse())
			foreach (var b in f.basarimlarTBL_)
			{
				//arttırımlı başarımlar "ilerlemeyi sıfırla" yapıldıktan sonra kontrol edilemez olduğu için devre dışı bırakıldı
				/*if (b.hedefDeger >= soruID)
				{
					mIAchievementsClient.IncrementImmediate(b.achi_id, 1);
				}*/

				//eğer bu başarımsa açıyoruz
				if (b.hedefDeger == soruID)
				{
					mIAchievementsClient.SetStepsImmediate(b.achi_id, b.hedefDeger);
					mIAchievementsClient.SetSteps(b.achi_id, b.hedefDeger);
					f.PlayGames__.basarimAc(b.achi_id);
					return true;
				}
			}

			return false;
		}


	}
}