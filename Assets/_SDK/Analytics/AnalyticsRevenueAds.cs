using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using System;
using AppsFlyerSDK;

public class AnalyticsRevenueAds
{
    //public static string AppsflyerID;
    //public static string FirebaseID;
    public static void SendEventFB(IronSourceImpressionData data)
    {
        SendEventRealtime(data);
    }
    private static void SendEventRealtime(IronSourceImpressionData data)
    {
        //string revenue = data.revenue.Value.ToString("N12").TrimEnd('0');

        Firebase.Analytics.Parameter[] AdParameters = {
             new Firebase.Analytics.Parameter("ad_platform", "iron_source"),
             new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
             new Firebase.Analytics.Parameter("ad_unit_name",data.adUnit),
             new Firebase.Analytics.Parameter("currency","USD"),
             new Firebase.Analytics.Parameter("value",data.revenue.Value),
             new Firebase.Analytics.Parameter("placement",data.placement),
             new Firebase.Analytics.Parameter("country_code",data.country),
             new Firebase.Analytics.Parameter("ad_format",data.instanceName),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_Impression_ironsource", AdParameters);


    }
	#region AF
	internal static void SendEventAF(IronSourceImpressionData data)
	{
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add(AFAdRevenueEvent.COUNTRY, data.country);
        dic.Add(AFAdRevenueEvent.AD_UNIT, data.adUnit);
        dic.Add(AFAdRevenueEvent.AD_TYPE, data.instanceName);
        dic.Add(AFAdRevenueEvent.PLACEMENT, data.placement);
        dic.Add(AFAdRevenueEvent.ECPM_PAYLOAD, data.encryptedCPM);
        //dic.Add("custom", "foo");
        //dic.Add("custom_2", "bar");
        //dic.Add("af_quantity", "1");
        AppsFlyerAdRevenue.logAdRevenue(data.adNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeIronSource, data.revenue.Value, "USD", dic);

    }
	#endregion
}
