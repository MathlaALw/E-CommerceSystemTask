using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_CommerceSystem.Models
{
    //add RefreshToken entity with EF annotations, audit fields and computed state
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsRevoked => Revoked != null;

        public bool IsActive => !IsRevoked && !IsExpired;

        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

    }
}
