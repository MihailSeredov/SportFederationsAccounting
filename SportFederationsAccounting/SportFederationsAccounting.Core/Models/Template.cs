using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFederationsAccounting.Core.Models
{
    /// <summary>
    /// Шаблон документа Word
    /// </summary>
    public class Template
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;           // Название шаблона ("Заявка на аккредитацию")

        [Required]
        public byte[] FileContent { get; set; } = Array.Empty<byte>(); // Сам .docx файл в байтах

        [MaxLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Навигационное свойство
        public List<TemplateMapping> Mappings { get; set; } = new();
    }

    /// <summary>
    /// Сопоставление плейсхолдера с полем федерации
    /// </summary>
    public class TemplateMapping
    {
        public int Id { get; set; }

        public int TemplateId { get; set; }
        public Template? Template { get; set; }

        [Required]
        [MaxLength(100)]
        public string Placeholder { get; set; } = string.Empty;     // {FullName}, {Code} и т.д.

        [MaxLength(100)]
        public string? MappedTo { get; set; }                        // "FullName", "SportType.Name", "Phone"...

        public bool IsMissing { get; set; } = false;                // Красный крестик
    }
}
