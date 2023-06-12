using System;

namespace _GAME._Scripts.Game
{
    public class GameFormat
    {
        public static string Cash(float balance) => $"$ {balance:F0}";
        public static string Cash(int balance) => $"${balance}";

        public static string RewardCash(int balance) => balance switch
        {
            0                              => "",
            < 1000                         => $"{balance}$",
            > 999 when balance % 1000 == 0 => $"{balance / 1000}K$",
            > 1000                         => $"{(float)balance / 1000:F1}K$"
        };

        private const string AM = "AM";
        private const string PM = "PM";

        public static string Time(int date, float hour24, float minute)
        {
            int hour12 =(int)(hour24 > 12 ? hour24 - 12 : hour24);

            return $"{hour12:D2}:{(int)minute:D2}";
        }

        public static string TimePeriod(float hour24)
        {
            return hour24 > 11 ? PM : AM;
        }
    }
}