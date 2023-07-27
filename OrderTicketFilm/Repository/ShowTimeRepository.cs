using AutoMapper;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System.Net.Sockets;

namespace OrderTicketFilm.Repository
{
    public class ShowTimeRepository : IShowTimeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;

        public ShowTimeRepository(MyDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CreateShowTime(ShowTime showTime)
        {
            _context.Add(showTime);
            return Save();
        }

        public bool DeleteShowTime(int id)
        {
            var deleteShowTime = _context.ShowTimes.SingleOrDefault(item => item.Id == id);
            if (deleteShowTime != null)
            {
                _context.ShowTimes.Remove(deleteShowTime);
            }
            return Save();
        }

        public ShowTimeDto GetShowTime(int id)
        {
            var showTime = _context.ShowTimes.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<ShowTimeDto>(showTime);
        }

        public ICollection<ShowTimeDto> GetShowTimes()
        {
            var showTimes = _context.ShowTimes.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<ShowTimeDto>>(showTimes);
        }

        public ICollection<ShowTime> GetShowTimesToCheck()
        {
            var showTimes = _context.ShowTimes.OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<ShowTime>>(showTimes);
        }

        public ShowTime GetShowTimeToCheck(int id)
        {
            var showTime = _context.ShowTimes.Where(item => item.Id == id).FirstOrDefault();
            return _mapper.Map<ShowTime>(showTime);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ShowTimeExists(int id)
        {
            return _context.ShowTimes.Any(f => f.Id == id);
        }

        public bool UpdateShowTime(ShowTime showTime)
        {
            _context.Update(showTime);
            return Save();
        }
    }
}
