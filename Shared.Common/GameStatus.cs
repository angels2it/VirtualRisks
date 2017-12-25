using System;

namespace CastleGo.Shared.Common
{
    public enum GameStatus
    {
        Requesting,
        Playing,
        Rejected,
        Ended,
    }

    /// <summary>
    /// 1 unit = 0.01 speed value
    /// </summary>
    public enum GameSpeed
    {
        UltraFast = 1,
        Fast = 10,
        Speedy = 50,
        Normal = 100,
        Slow = 200,
        UltraSlow = 500
    }

    public enum GameDifficulfy
    {
        Easy,
        Normal,
        Hard
    }
    public static class GameSpeedHelper
    {
        public static double GetSpeedValue(GameSpeed speed)
        {
            int speedValue = (int)speed;
            if (speedValue == 0) // no settings - used default value
                return 1;
            return speedValue * 0.01;
        }
    }
}
