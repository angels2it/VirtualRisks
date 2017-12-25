using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace CastleGo.Providers
{
    /// <summary>Web api helper</summary>
    public class WebApiProvider : IWebApiProvider
    {
        private const int ApiTimeOut = 300;

        public async Task<TOut> GetAsync<TOut>(string baseUrl, string resource, List<Parameter> parameters = null, string token = "")
        {
            try
            {
                RestClient client = new RestClient(new Uri(baseUrl));
                RestRequest request = new RestRequest(resource, Method.GET);
                if (!string.IsNullOrEmpty(token))
                    request.AddHeader("Authorization", "Bearer " + token);
                if (parameters != null)
                {
                    foreach (Parameter parameter1 in parameters)
                    {
                        Parameter parameter = parameter1;
                        request.AddQueryParameter(parameter.Name, parameter.Value.ToString());
                        parameter = (Parameter)null;
                    }
                    List<Parameter>.Enumerator enumerator = new List<Parameter>.Enumerator();
                }
                IRestResponse<TOut> restResponse = await client.ExecuteGetTaskAsync<TOut>((IRestRequest)request);
                IRestResponse<TOut> rs = restResponse;
                restResponse = (IRestResponse<TOut>)null;
                return rs.Data;
            }
            catch (Exception ex)
            {
                return default(TOut);
            }
        }

        public RestClient GetClient(string baseUrl, string token = "")
        {
            return new RestClient(new Uri(baseUrl));
        }
    }
}
