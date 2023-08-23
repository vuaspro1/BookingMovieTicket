namespace OrderTicketFilm.Dto
{
    public class RoomDto
    {
        public string Name { get; set; }
        //public int RoomStatusId { get; set; }
    }

    public class RoomView : RoomDto
    {
        public int Id { get; set; }
        //public string StatusName { get; set; }
    }
}
