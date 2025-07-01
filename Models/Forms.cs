using System.ComponentModel.DataAnnotations;

namespace Formio.Models
{
    public class Forms
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "CreatedBy is required.")]
        [StringLength(100, ErrorMessage = "Max 100 characters.")]
        public string CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        [Required]
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedUtc { get; set; }

        [Required]
        public Guid VersionId { get; set; }

        [Required]
        public bool Latest { get; set; }

        public string? FormFields { get; set; }
    }
}
