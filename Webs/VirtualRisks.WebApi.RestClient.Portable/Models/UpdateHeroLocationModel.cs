// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class UpdateHeroLocationModel
    {
        /// <summary>
        /// Initializes a new instance of the UpdateHeroLocationModel class.
        /// </summary>
        public UpdateHeroLocationModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UpdateHeroLocationModel class.
        /// </summary>
        public UpdateHeroLocationModel(PositionModel position = default(PositionModel), string heroId = default(string))
        {
            Position = position;
            HeroId = heroId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "position")]
        public PositionModel Position { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "heroId")]
        public string HeroId { get; set; }

    }
}