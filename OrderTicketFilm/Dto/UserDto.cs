namespace OrderTicketFilm.Dto
{
    public class UserDto : UserUpdate
    {
        //public int Id { get; set; }
        
        public string UserName { get; set; }
    }

    public class UserView : UserDto
    {
        public int Id { get; set; }
        public List<RoleView>? Roles { get; set; }
    }

    public class UserUpdate
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
