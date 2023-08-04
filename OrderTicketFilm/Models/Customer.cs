using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Bill>? Bills { get; set; }
    }
}
