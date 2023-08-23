namespace OrderTicketFilm.Dto
{
    public class TypeOfFilmDto
    {

        public string Name { get; set; }
    }

    public class TypeOfFilmView : TypeOfFilmDto
    {
        public int Id { get; set; }
    }
}
