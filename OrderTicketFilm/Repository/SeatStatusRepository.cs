using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;

namespace OrderTicketFilm.Repository
{
    public class SeatStatusRepository : ISeatStatusRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public SeatStatusRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public bool CreateSeatStatus(SeatStatus seatStatus)
        {
            _context.Add(seatStatus);
            return Save();
        }

        public bool DeleteSeatStatus(int id)
        {
            var deleteStatus = _context.SeatStatuses.SingleOrDefault(item => item.Id == id);
            if (deleteStatus != null)
            {
                _context.SeatStatuses.Remove(deleteStatus);
            }
            return Save();
        }

        public ICollection<SeatStatusDto> GetSeatStatuses()
        {
            var status = _context.SeatStatuses.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<SeatStatusDto >>(status);
        }

        public SeatStatus GetStatus(int id)
        {
            var status = _context.SeatStatuses.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<SeatStatus>(status);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SeatStatusExists(int id)
        {
            return _context.SeatStatuses.Any(c => c.Id == id);
        }

        public bool UpdateSeatStatus(SeatStatus seatStatus)
        {
            var statusMap = _context.SeatStatuses.FirstOrDefault(rs => rs.Id == seatStatus.Id);

            if (statusMap == null)
            {
                return false;
            }

            // Update only the "Status" property
            statusMap.Status = seatStatus.Status;
            _context.Update(seatStatus);
            return Save();
        }
    }
}
