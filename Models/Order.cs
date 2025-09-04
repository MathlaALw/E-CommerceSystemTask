using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    public class Order
    {
        [Key] 
        public int OID { get; set; }

        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [ForeignKey("user")]
        public int UID { get; set; }
        public User user { get; set; }

        // Add order status
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [JsonIgnore]
        public virtual ICollection <OrderProducts> OrderProducts { get; set; }
    }
}
