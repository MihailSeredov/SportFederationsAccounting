using System.ComponentModel.DataAnnotations;

namespace SportFederationsAccounting.Core.Models
{
    public class FederationState
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;   // "Действующая", "В статусе ликвидации", "Ликвидирована"

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
    }
}