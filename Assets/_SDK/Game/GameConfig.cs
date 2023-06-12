using System.Collections;
using UnityEngine;

namespace Assets._SDK.Game
{
    public static class SceneName
    {
        public const string PLAY_SCENE = "PLAY_SCENE";
    }

    public enum GameMode
    {
        LEVEL,
        ENDLESS,
    }

    public class GameplayConfig
    {
        public static GameMode gameMode;
    }
}