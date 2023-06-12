using System;
public class AdsConfig
{
    // AppOpenAdd
#if UNITY_ANDROID
    public const string ID_TIER_1 = "ca-app-pub-6336405384015455/7927115923";
    public const string ID_TIER_2 = "ca-app-pub-6336405384015455/9095344473";
    public const string ID_TIER_3 = "ca-app-pub-6336405384015455/3520967054";

#elif UNITY_IOS
        public const string ID_TIER_1 = "";
        public const string ID_TIER_2 = "";
        public const string ID_TIER_3 = "";
#else
        public const string ID_TIER_1 = "ca-app-pub-6336405384015455/7927115923";
        public const string ID_TIER_2 = "ca-app-pub-6336405384015455/9095344473";
        public const string ID_TIER_3 = "ca-app-pub-6336405384015455/3520967054";
#endif

    //AppFlyer
    //public const string AppFlyerDevKey = "Mza5CYwx7pzKhdhcFcTHdm";
    //public const string AppFlyerHost = "appsflyersdk.com";

    // public const string AppFlyerAppId = ""; // Only for iOS

    //Max
    //public const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";
    //public const string InterstitialAdUnitId = "fa38f49f84fd9c7c";
    //public const string RewardedAdUnitId = "4b5d2212116eefd0";
    //public const string BannerAdUnitId = "c182066f13ddfb69";

    //public const string InterstitialAdUnitId = "d3ffc84f1fa3a03d";
    //public const string RewardedAdUnitId = "82926a5a0d827477";
    //public const string BannerAdUnitId = "efcca8e6f53d66d8";
    //public const string AppOpenAdUnitId = "";

    // Iron Source
    public const string ironSourceAppKey = "192a9647d";

    public const string CappingTimeRemoteConfigKey = "config_gta_nextbot_capping_time";
    public const float CappingTimeDefaultValue = 25;
    public const string InterBackupRemoteConfigKey = "config_gta_nextbot_show_inter_backup";
    public const bool InterBackupDefaultValue = false;

    public const float CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA = 5f;
    public const AdManagerEnum AdManagerType = AdManagerEnum.MockAds;

	//public static TypeAds EnabledTypeAds = TypeAds.AppOpen| TypeAds.Banner| TypeAds.RewardVideo| TypeAds.Interstitial;
}

public enum AdManagerEnum
{
    MockAds,
    IronSource
}

//[Flags]
//public enum TypeAds
//{
//	None = 0,
//	AppOpen = 1,
//	Banner = 2,
//	RewardVideo = 4,
//	Interstitial = 8,
//	Other = 16,
//}