// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateTokenModel
    {
        /// <summary>
        /// Initializes a new instance of the UpdateTokenModel class.
        /// </summary>
        public UpdateTokenModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateTokenModel class.
        /// </summary>
        /// <param name="device">Possible values include: 'Android',
        /// 'iOS'</param>
        public UpdateTokenModel(string device = default(string), string token = default(string))
        {
            Device = device;
            Token = token;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets possible values include: 'Android', 'iOS'
        /// </summary>
        [JsonProperty(PropertyName = "device")]
        public string Device { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

    }
}
