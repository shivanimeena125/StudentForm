using Formio.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Formio.Models
{
    public class FormSubmission
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string SubmittedBy { get; set; }

        public DateTime SubmittedUtc { get; set; } = DateTime.UtcNow;

        public string SubmissionData { get; set; }

        [ForeignKey("Forms")]
        public Guid FormGroupId { get; set; }

        public Forms Forms { get; set; } 

        public ApplicationUser ApplicationUser { get; set; }


    }
}
