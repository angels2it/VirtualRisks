using System;
using System.Collections.Generic;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using CastleGo.WebApi.Workers;

namespace CastleGo.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class GameConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static HttpConfiguration UsingGameConfig(this HttpConfiguration configuration)
        {
            var container = (configuration.DependencyResolver as AutofacWebApiDependencyResolver)?.Container;
            if (container == null)
                throw new ArgumentException();
            var workers = container.Resolve<IEnumerable<IWorker>>();
            foreach (var worker in workers)
            {
                worker.Run();
            }
            return configuration;
        }
    }
}