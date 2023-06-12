using System.Collections.Generic;
using UnityEngine;

namespace Assets._SDK.Toolkit
{
    public class WaitForSecondsCached
    {
        public WaitForSeconds[] GenerateWaitForSeconds(int from, int to)
        {
            WaitForSeconds[] waitForSecondsArray = new WaitForSeconds[to - from + 1];
            for (int i = from; i <= to; i++)
            {
                waitForSecondsArray[i] = new WaitForSeconds(i);
            }

            return waitForSecondsArray;
        }
    }
}