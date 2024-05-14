using System;
using System.Collections.Generic;
using Android.App;
using Android.BillingClient.Api;

namespace gorbul
{
    class _BillingClient : Java.Lang.Object, IPurchasesUpdatedListener, IBillingClientStateListener,
        ISkuDetailsResponseListener, IConsumeResponseListener, IAcknowledgePurchaseResponseListener
    {
        public IList<SkuDetails> SkuDetails;
        public BillingClient billingClient;
        private List<string> skuList = new List<string>() { "20jeton", "50jeton", "120jeton", "320jeton", "750jeton" };
        public IList<Purchase> querylist;

        public _BillingClient()
        {
            billingClient = BillingClient.NewBuilder(f.c).EnablePendingPurchases().SetListener(this).Build();
            billingClient.StartConnection(this);
        }

        public void LoadPurchases()
        {
            if (billingClient.IsReady)
            {
                var paramse = SkuDetailsParams.NewBuilder().SetSkusList(skuList).SetType(BillingClient.SkuType.Inapp).Build();
                billingClient.QuerySkuDetails(paramse, this);

                _IpurchasedIPurchasesResponseListener = async (s, e) =>
                {
                    var ss = s as object[];
                    var p0 = ss[0] as BillingResult;
                    var p1 = ss[1] as IList<Purchase>;

                    querylist = p1;
                };
            }
        }

        public void PurchaseNow(SkuDetails skuDetails)
        {
            var billingFlowParams = BillingFlowParams.NewBuilder().SetSkuDetails(skuDetails).Build();
            billingClient.LaunchBillingFlow(f.c as Activity, billingFlowParams);
        }

        public EventHandler _OnBillingServiceDisconnected;
        public void OnBillingServiceDisconnected()
        {
            _OnBillingServiceDisconnected?.Invoke(this, EventArgs.Empty);
        }

        public EventHandler _OnBillingSetupFinished;
        public void OnBillingSetupFinished(BillingResult p0)
        {
            _OnBillingSetupFinished?.Invoke(p0, EventArgs.Empty);
        }

        public EventHandler _OnPurchasesUpdated;
        public void OnPurchasesUpdated(BillingResult p0, IList<Purchase> p1)
        {
            object[] o = new object[2];
            o[0] = p0;
            o[1] = p1;
            _OnPurchasesUpdated?.Invoke(o, EventArgs.Empty);
        }

        public EventHandler _OnSkuDetailsResponse;
        public void OnSkuDetailsResponse(BillingResult p0, IList<SkuDetails> p1)
        {
            object[] o = new object[2];
            o[0] = p0;
            o[1] = p1;
            _OnSkuDetailsResponse?.Invoke(o, EventArgs.Empty);
        }

        public void InitProductAdapter(IList<SkuDetails> skuDetails)
        {
            this.SkuDetails = skuDetails;
        }

        public void ConsumePurchase(string PurchaseToken)
        {
            if (PurchaseToken != null)
            {
                var consumeParams = ConsumeParams.NewBuilder().SetPurchaseToken(PurchaseToken).Build();
                billingClient.Consume(consumeParams, this);
            }
        }   

        public EventHandler _OnConsumeResponse;
        public void OnConsumeResponse(BillingResult p0, string p1)
        {
            object[] o = new object[2];
            o[0] = p0;
            o[1] = p1;
            _OnConsumeResponse?.Invoke(o, EventArgs.Empty);
        }

        public EventHandler _OnAcknowledgePurchaseResponse;
        public void OnAcknowledgePurchaseResponse(BillingResult p0)
        {
            _OnAcknowledgePurchaseResponse?.Invoke(p0, EventArgs.Empty);
            _OnAcknowledgePurchaseResponse = null;
        }

        /*public void ClearOrConsumeAllPurchases()
        {
            querylist = billingClient.QueryPurchases(BillingClient.SkuType.Inapp).PurchasesList;
            if (querylist != null)
            {
                foreach (var query in querylist)
                {
                    var consumeParams = ConsumeParams.NewBuilder().SetPurchaseToken(query.PurchaseToken).Build();
                    billingClient.Consume(consumeParams, this);
                }
            }
        }*/

        public EventHandler _IpurchasedIPurchasesResponseListener;
        public void IpurchasedIPurchasesResponseListener(BillingResult p0, IList<Purchase> p1)
        {
            object[] o = new object[2];
            o[0] = p0;
            o[1] = p1;
            _IpurchasedIPurchasesResponseListener?.Invoke(o, EventArgs.Empty);
        }


        public EventHandler _IPurchasesResponseListener;
        public void IPurchasesResponseListener(BillingResult p0, IList<Purchase> p1)
        {
            object[] o = new object[2];
            o[0] = p0;
            o[1] = p1;
            _IPurchasesResponseListener?.Invoke(o, EventArgs.Empty);
        }

    }
}