using System.ComponentModel.DataAnnotations;

namespace OrderTicketFilm.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
