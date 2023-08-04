namespace OrderTicketFilm.Dto
{
    public class ShowTimeDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int RoomId { get; set; }
        public int FilmId { get; set; }
    }

    public class ShowTimeView : ShowTimeDto
    {

        public string RoomName { get; set; }
        public string FilmName { get; set; }
        public string Duration { get; set; }
        public string Image { get; set; }
    }
}
 