using System.ComponentModel.DataAnnotations;

namespace SportFederationsAccounting.Core.Models
{
    public class SportType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;   // "Лыжный спорт", "Футбол" и т.д.

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;
    }
}