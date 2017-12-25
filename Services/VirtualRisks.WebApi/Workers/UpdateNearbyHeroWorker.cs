using System.Configuration;
using CastleGo.Application.Games;
using Hangfire;

namespace CastleGo.WebApi.Workers
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateNearbyHeroWorker : IWorker
    {
        /// <inheritdoc />
        public void Run()
        {
            var minute = 5;
            int.TryParse(ConfigurationManager.AppSettings["Game:HeroStayInCastleTime"], out minute);
            RecurringJob.AddOrUpdate<IGameService>(g => g.UpdateNearbyHero(), Cron.MinuteInterval(minute));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RunArtifactWorker : IWorker
    {
        /// <inheritdoc />
        public void Run()
        {
        }
    }
}
