using System.ComponentModel.DataAnnotations;

namespace SportFederationsAccounting.Core.Models
{
    public class Federation
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public int Code { get; set; }

        [MaxLength(500)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(300)]
        public string ShortName { get; set; } = string.Empty;

        public int? SportTypeId { get; set; }
        public SportType? SportType { get; set; }

        public int? AccreditationStatusId { get; set; }
        public AccreditationStatus? AccreditationStatus { get; set; } = null!;

        public int FederationStateId { get; set; }
        public FederationState FederationState { get; set; } = null!;

        public DateOnly? AccreditationEndDate { get; set; }

        [MaxLength(500)]
        public string? LegalAddress { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PostalAddress { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Phone { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Email { get; set; } = string.Empty;

        [MaxLength(12)]
        public string? INN { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? OGRN { get; set; } = string.Empty;

        public List<ContactPerson> ContactPersons { get; set; } = new();
    }
}