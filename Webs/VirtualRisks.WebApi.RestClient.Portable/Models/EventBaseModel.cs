// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class EventBaseModel
    {
        /// <summary>
        /// Initializes a new instance of the EventBaseModel class.
        /// </summary>
        public EventBaseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the EventBaseModel class.
        /// </summary>
        public EventBaseModel(System.Guid? id = default(System.Guid?), System.DateTime? runningAt = default(System.DateTime?), System.DateTime? executeAt = default(System.DateTime?), string type = default(string))
        {
            Id = id;
            RunningAt = runningAt;
            ExecuteAt = executeAt;
            Type = type;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "runningAt")]
        public System.DateTime? RunningAt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "executeAt")]
        public System.DateTime? ExecuteAt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

    }
}
