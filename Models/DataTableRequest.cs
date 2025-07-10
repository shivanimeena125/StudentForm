namespace Formio.Models
{
    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? Title { get; set; }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }

}
