using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentForm.Models
{
    public class City
    {
        [Key]

        public int Id { get; set; }
        public string CityName { get; set; }

        public int StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; } 

    }
}
