using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeliveryLogic.Models
{
    /// <summary>
    /// Delivery window details
    /// </summary>
    public class AccessWindow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Start time of delivery window
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// End time of delivery window
        /// </summary>
        public DateTime EndTime { get; set; }

    }
}
