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

        public PaginationDTO<SeatStatusView> GetSeatStatuses(int page, int pageSize)
        {
            PaginationDTO<SeatStatusView> pagination = new PaginationDTO<SeatStatusView>();
            var statuses = _context.SeatStatuses.OrderBy(c => c.Id).ToList();
            var status = statuses.Select(item => new SeatStatusView
            {
                Id = item.Id,
                Status = item.Status,
                Code = item.Code,
            }).ToList();
            var result = PaginatedList<SeatStatusView>.Create(status.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = status.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public SeatStatus GetStatusToCheck(int id)
        {
            var status = _context.SeatStatuses.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<SeatStatus>(status);
        }

        public SeatStatusView GetStatus(int id)
        {
            var status = _context.SeatStatuses.Where(item => item.Id == id).FirstOrDefault();
            return new SeatStatusView
            {
                Id = status.Id,
                Status = status.Status,
                Code = status.Code,
            };
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

        public ICollection<SeatStatusDto> GetSeatStatusesToCheck()
        {
            var status = _context.SeatStatuses.ToList();
            return _mapper.Map<List<SeatStatusDto>>(status);
        }
    }
}
