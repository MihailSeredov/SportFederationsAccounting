using System.ComponentModel.DataAnnotations;

namespace SportFederationsAccounting.Core.Models
{
    public class AccreditationStatus
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;   // "Аккредитована", "Отказ", "Завершена" и т.д.

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
    }
}