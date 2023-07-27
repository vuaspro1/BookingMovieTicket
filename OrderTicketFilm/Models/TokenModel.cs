using OrderTicketFilm.Dto;

namespace OrderTicketFilm.Models
{
    public class TokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public ICollection<Role>? Roles { get; set; }
    }
}
