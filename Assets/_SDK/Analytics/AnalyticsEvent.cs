using System.Collections;
using UnityEngine;

namespace Assets._SDK.Analytics
{
    public class AnalyticsEvent
    {
        public static string LEVEL_START         = "level_start";
        public static string LEVEL_WIN           = "level_win";
        public static string LEVEL_LOSE          = "level_lose";
        public static string REWARDED_VIDEO_SHOW = "rewarded_video_show";
        public static string INTERSTITIAL_SHOW   = "interstitial_show";
        public static string APPOPENADS_SHOW     = "app_open_ads_show";
    }

    public static class UserProperties
    {
        public static string LEVEL_REACH  = "level_reach";
        public static string DAYS_PLAYING = "days_playing";
        public static string TOTAL_SPENT  = "total_spent";
        public static string TOTAL_EARN   = "total_earn";
    }

    public static class AnalyticParamKey
    {
        public static string TIME      = "time";
        public static string LEVEL     = "level";
        public static string PLACEMENT = "placement";

        #region REWARD

        public static string WEAPON_SHOP   = "weapon_shop";
        public static string WEAPON_INGAME = "weapon_ingame";
        public static string PLAYER_REVIVE = "player_revive";
        public static string MULTI_PRIZE   = "multi_prize";
        public static string REWARD_REFILL = "reward_refill";

        #endregion

        #region INTER

        public static string END_DAY = "end_day";

        #endregion
    }
}