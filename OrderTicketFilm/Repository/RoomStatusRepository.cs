using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class RoomStatusRepository : IRoomStatusRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public RoomStatusRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateRoomStatus(RoomStatus roomStatus)
        {
            _context.Add(roomStatus);
            return Save();
        }

        public bool DeleteRoomStatus(int id)
        {
            var deleteStatus = _context.RoomStatuses.SingleOrDefault(item => item.Id == id);
            if (deleteStatus != null)
            {
                _context.RoomStatuses.Remove(deleteStatus);
            }
            return Save();
        }

        public ICollection<RoomStatusDto> GetRoomStatuses()
        {
            var status = _context.RoomStatuses.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<RoomStatusDto>>(status);
        }

        public RoomStatus GetStatus(int id)
        {
            var status = _context.RoomStatuses.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<RoomStatus>(status);
        }

        public bool RoomStatusExists(int id)
        {
            return _context.RoomStatuses.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRoomStatus(RoomStatus roomStatus)
        {
            var statusMap = _context.RoomStatuses.FirstOrDefault(rs => rs.Id == roomStatus.Id);

            if (statusMap == null)
            {
                return false;
            }

            // Update only the "Status" property
            statusMap.Status = roomStatus.Status;
            _context.Update(roomStatus);
            return Save();
        }
    }
}
