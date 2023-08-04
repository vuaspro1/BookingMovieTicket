using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Film, FilmDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<User, UserDto>();
            CreateMap<Room, RoomDto>();
            CreateMap<Seat, SeatDto>();
            CreateMap<ShowTime, ShowTimeDto>();
            CreateMap<TypeOfFilm, TypeOfFilmDto>();
            CreateMap<FilmDto, Film>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<RoleDto, Role>();
            CreateMap<UserDto, User>();
            CreateMap<RoomDto, Room>();
            CreateMap<SeatDto, Seat>();
            CreateMap<ShowTimeDto, ShowTime>();
            CreateMap<TicketDto, Ticket>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<TypeOfFilmDto, TypeOfFilm>();
            CreateMap<Bill, BillDto>();
            CreateMap<BillDto, Bill>();
            CreateMap<RoomStatusDto, RoomStatus>();
            CreateMap<RoomStatus, RoomStatusDto>();
            CreateMap<SeatStatus, SeatStatusDto>();
            CreateMap<SeatStatusDto, SeatStatus>();
        }
    }
}
