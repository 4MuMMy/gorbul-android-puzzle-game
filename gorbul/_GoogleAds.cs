using System;
using Android.Gms.Ads;

namespace gorbul
{
    public static class _GoogleAds
    {
        public static Android.Gms.Ads.Interstitial.InterstitialAd _interstitialAd;
        public static bool InterstitialAdIsLoaded = false;
        public static bool InterstitialAdIsFailed = false;

        public class _InterstitialAdLoadCallback : Android.Gms.Ads.Hack.InterstitialAdLoadCallback
        {
            public EventHandler _OnInterstitialAdLoaded;
            public EventHandler _OnAdFailedToLoad;

            public override void OnInterstitialAdLoaded(Android.Gms.Ads.Interstitial.InterstitialAd p0)
            {
                InterstitialAdIsLoaded = true;
                _interstitialAd = p0;
                _OnInterstitialAdLoaded?.Invoke(this, EventArgs.Empty);

                base.OnInterstitialAdLoaded(p0);
            }

            public override void OnAdFailedToLoad(LoadAdError p0)
            {
                InterstitialAdIsFailed = true;

                _OnAdFailedToLoad?.Invoke(this, EventArgs.Empty);

                base.OnAdFailedToLoad(p0);
            }
        }


        public static Android.Gms.Ads.Rewarded.RewardedAd _rewardedAd;
        public static bool RewardedAdIsLoaded = false;
        public static bool RewardedAdIsFailed = false;

        public class _RewardedAdLoadCallback : global::Android.Gms.Ads.Hack.RewardedAdLoadCallback
        {
            public EventHandler _OnRewardedAdLoaded;
            public EventHandler _OnAdFailedToLoad;
            public override void OnRewardedAdLoaded(global::Android.Gms.Ads.Rewarded.RewardedAd p0)
            {
                RewardedAdIsLoaded = true;
                _rewardedAd = p0;
                _OnRewardedAdLoaded?.Invoke(this, EventArgs.Empty);

                base.OnRewardedAdLoaded(p0);
            }

            public override void OnAdFailedToLoad(LoadAdError p0)
            {
                RewardedAdIsFailed = true;

                _OnAdFailedToLoad?.Invoke(this, EventArgs.Empty);

                base.OnAdFailedToLoad(p0);
            }
        }


        public class _IOnUserEarnedRewardListener : Java.Lang.Object, IOnUserEarnedRewardListener
        {
            public EventHandler OnRewarded;
            public void OnUserEarnedReward(Android.Gms.Ads.Rewarded.IRewardItem rewardItem)
            {
                OnRewarded?.Invoke(this, EventArgs.Empty);
            }
        }


        public class _FullScreenContentCallback : FullScreenContentCallback
        {
            public EventHandler _OnAdDismissedFullScreenContent;
            public EventHandler _OnAdFailedToShowFullScreenContent;
            public EventHandler _OnAdShowedFullScreenContent;
            public override void OnAdDismissedFullScreenContent()
            {
                _OnAdDismissedFullScreenContent?.Invoke(this, EventArgs.Empty);

                base.OnAdDismissedFullScreenContent();
            }

            public override void OnAdFailedToShowFullScreenContent(AdError p0)
            {
                _OnAdFailedToShowFullScreenContent?.Invoke(this, EventArgs.Empty);

                base.OnAdFailedToShowFullScreenContent(p0);
            }
            public override void OnAdShowedFullScreenContent()
            {
                _OnAdShowedFullScreenContent?.Invoke(this, EventArgs.Empty);

                base.OnAdShowedFullScreenContent();
            }
        }


    }
}