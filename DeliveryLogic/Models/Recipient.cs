using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DeliveryLogic.Models
{
    /// <summary>
    /// Recipient details
    /// </summary>
    public class Recipient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int RecipientId { get; set; }

        /// <summary>
        /// Name of the Recipient
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Address of the Recipient
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Email Address of the Recipient
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number of the Recipient
        /// </summary>
        public string PhoneNumber { get; set; }

    }
}
