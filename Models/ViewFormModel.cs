namespace Formio.Models
{
    public class ViewFormModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedUtc { get; set; }

        public DateTime? ModifiedUtc { get; set; }
        public Guid VersionId { get; set; }
        public Guid FormGroupId { get; set; }
    }
}
