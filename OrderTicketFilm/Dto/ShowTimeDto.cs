namespace OrderTicketFilm.Dto
{
    public class ShowTimeDto
    {
        public DateTime Time { get; set; }
        public int RoomId { get; set; }
        public int FilmId { get; set; }
    }

    public class ShowTimeView : ShowTimeDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string FilmName { get; set; }
        public string Duration { get; set; }
        public string Image { get; set; }
        public string TypeName { get; set; }
    }
}
 