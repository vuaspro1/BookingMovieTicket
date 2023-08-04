using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class TypeOfFilm
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Film>? Films { get; set; }
    }
}
