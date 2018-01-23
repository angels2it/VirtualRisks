// Helpers/Settings.cs

using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace VirtualRisks.Mobiles.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string CurrentGameIdKey = "current_game_id_key";
		private const string TokenKey = "token_key";
		private const string UserIdKey = "user_id_key";

		#endregion


		public static string CurrentGameId
        {
			get
			{
				return AppSettings.GetValueOrDefault(CurrentGameIdKey, string.Empty);
			}
			set
			{
				AppSettings.AddOrUpdateValue(CurrentGameIdKey, value);
			}
		}

        internal static void Clear()
        {
            Token = string.Empty;
            UserId = string.Empty;
        }

        internal static bool RequireLogin()
        {
            return string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(UserId);
        }

        public static bool IsAuth
        {
            get
            {
                return !string.IsNullOrEmpty(AppSettings.GetValueOrDefault(TokenKey, string.Empty));
            }
        }
        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(TokenKey, string.Empty);
            }
            set
            {
                AppSettings.AddOrUpdateValue(TokenKey, value);
            }
        }

	    public static string UserId
	    {
	        get
	        {
	            return AppSettings.GetValueOrDefault(UserIdKey, string.Empty);
	        }
	        set
	        {
	            AppSettings.AddOrUpdateValue(UserIdKey, value);
	        }
	    }
    }
}