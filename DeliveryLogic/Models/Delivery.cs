using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DeliveryLogic.Models.Enums;

namespace DeliveryLogic.Models
{
    public class Delivery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int DeliveryId { get; set; }

        /// <summary>
        /// Status of Delivery
        /// </summary>
        [EnumDataType(typeof(DeliveryStatus))]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeliveryStatus State { get; set; }

        public AccessWindow AccessWindow { get; set; }

        public Recipient Recipient { get; set; }

        public Order Order { get; set; }
    }
}