using OrderTicketFilm.Models;

namespace OrderTicketFilm.Dto
{
    public class FilmDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime OpeningDay { get; set; }
        public string Director { get; set; }
        public string Time { get; set; }
        public string Image { get; set; }
        public int TypeId { get; set; }
    }

    public class FilmView : FilmDto
    {
        public string TypeName { get; set; }
    }
}
