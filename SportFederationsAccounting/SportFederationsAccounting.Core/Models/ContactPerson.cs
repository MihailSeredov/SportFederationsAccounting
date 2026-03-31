using System.ComponentModel.DataAnnotations;

namespace SportFederationsAccounting.Core.Models
{
    public class ContactPerson
    {
        public int Id { get; set; }

        public int FederationId { get; set; }
        public Federation Federation { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Position { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}