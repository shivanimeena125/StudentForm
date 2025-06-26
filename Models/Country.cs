using System.ComponentModel.DataAnnotations;

namespace StudentForm.Models
{
    public class Country
    {
        [Key]

        public int Id { get; set; }
        public string CountryName { get; set; }

        public ICollection<State> States { get; set; }
    }
}
