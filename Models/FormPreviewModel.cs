namespace Formio.Models
{
    public class  FormPreviewModel
    {
        public Guid FormGroupId { get; set; }
        public string FormJson { get; set; }              
        public string SubmissionData { get; set; }
    }
}
