using OrderTicketFilm.Models;
using System.Globalization;

namespace OrderTicketFilm.Dto
{
    public class Enums
    {
        public SeatStatus seatOrderInfo = new SeatStatus { Code = "Booked"};
        public SeatStatus seatEmptyInfo = new SeatStatus { Code = "Empty" };
    }
}
