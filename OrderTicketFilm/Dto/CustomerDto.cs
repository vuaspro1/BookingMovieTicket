namespace OrderTicketFilm.Dto
{
    public class CustomerDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class CustomerView : CustomerDto
    {
        public int Id { get; set; }
    }
}
