using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System;
using System.Net.Sockets;

namespace OrderTicketFilm.Repository
{
    public class ShowTimeRepository : IShowTimeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public static DateTime Now { get; }
        public static int PAGE_SIZE { get; set; } = 10;

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

        public ShowTimeView GetShowTime(int id)
        {
            var showTime = _context.ShowTimes.Include(s => s.Film).Include(s => s.Room)
                                          .FirstOrDefault(s => s.Id == id);
            return new ShowTimeView
            {
                Id = showTime.Id,
                RoomId = showTime.Room.Id,
                FilmId = showTime.Film.Id,
                Time = showTime.Time,
                FilmName = showTime.Film.Name,
                RoomName = showTime.Room.Name,
                Duration = showTime.Film.Time,
                Image = showTime.Film.Image,
            };
        }

        public ICollection<ShowTimeView> GetShowTimes(int page)
        {
            var showtimes = _context.ShowTimes.Include(s => s.Film).Include(s => s.Room).OrderBy(s => s.Time.Date).ToList();
            var showTime = showtimes.Select(item => new ShowTimeView
            {
                Id = item.Id,
                RoomId = item.Room.Id,
                FilmId = item.Film.Id,
                Time = item.Time,
                FilmName = item.Film.Name,
                RoomName = item.Room.Name,
                Duration = item.Film.Time,
                Image = item.Film.Image,
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, PAGE_SIZE);
            return result;
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

        public bool UpdateShowTime( ShowTime showTime)
        {
            _context.Update(showTime);
            return Save();
        }

        public ICollection<ShowTimeView> GetShowTimesOfNow(int page)
        {
            DateTime localDate = DateTime.Now;
            var showtimes = _context.ShowTimes.Include(s => s.Film).Include(s => s.Room).ToList();
            showtimes = showtimes.Where(item => item.Time > localDate).OrderBy(item => item.Time.Date).ToList();
            var showTimes = showtimes.Select(item => new ShowTimeView
            {
                Id = item.Id,
                RoomId = item.Room.Id,
                FilmId = item.Film.Id,
                Time = item.Time,
                FilmName = item.Film.Name,
                RoomName = item.Room.Name,
                Duration = item.Film.Time,
                Image = item.Film.Image,
            }).ToList();  
            var result = PaginatedList<ShowTimeView>.Create(showTimes.AsQueryable(), page, PAGE_SIZE);
            return result;
        }

        public ICollection<ShowTimeView> GetShowTimesByDay(DateTime startDate, DateTime endDate, int page)
        {
            List<ShowTime> showTimes;

            if (startDate.Date == endDate.Date)
            {
                // Lấy tất cả suất chiếu trong startDate nếu startDate bằng endDate
                showTimes = _context.ShowTimes.Where(item => item.Time.Date == startDate.Date).ToList();
            }
            else
            {
                // Lấy tất cả suất chiếu trong khoảng từ startDate đến endDate
                showTimes = _context.ShowTimes
                    .Where(item => item.Time.Date >= startDate.Date && item.Time.Date <= endDate.Date)
                    .OrderBy(item => item.Time.Date).ToList();
            }

            var showTime = showTimes.Select(item => new ShowTimeView
            {
                Id = item.Id,
                RoomId = item.Room.Id,
                FilmId = item.Film.Id,
                Time = item.Time,
                FilmName = item.Film.Name,
                RoomName = item.Room.Name,
                Duration = item.Film.Time,
                Image = item.Film.Image,
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, PAGE_SIZE);
            return result;
        }
    }
}
