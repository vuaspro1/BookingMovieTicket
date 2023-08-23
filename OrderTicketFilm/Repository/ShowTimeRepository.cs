using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderTicketFilm.Dto;
using OrderTicketFilm.Interface;
using OrderTicketFilm.Models;
using System;
using System.Drawing.Printing;
using System.Net.Sockets;

namespace OrderTicketFilm.Repository
{
    public class ShowTimeRepository : IShowTimeRepository
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public static DateTime Now { get; }

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
            var showTime = _context.ShowTimes.Include(s => s.Film).ThenInclude(s => s.TypeOfFilm).Include(s => s.Room)
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
                TypeName = showTime.Film.TypeOfFilm.Name
            };
        }

        public PaginationDTO<ShowTimeView> GetShowTimes(int page, int pageSize)
        {
            PaginationDTO<ShowTimeView> pagination = new PaginationDTO<ShowTimeView>();
            var showtimes = _context.ShowTimes.Include(s => s.Film).ThenInclude(s => s.TypeOfFilm)
                .Include(s => s.Room).OrderBy(s => s.Time.Date).ToList();
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
                TypeName = item.Film.TypeOfFilm.Name
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = showTime.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }

        public ICollection<ShowTime> GetShowTimesToCheck()
        {
            var showTimes = _context.ShowTimes.Include(item => item.Film).Include(item => item.Room).OrderBy(c => c.Id).ToList();
            return _mapper.Map<List<ShowTime>>(showTimes);
        }

        public ShowTime GetShowTimeToCheck(int? id)
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

        public PaginationDTO<ShowTimeView> GetShowTimesByDay(DateTime startDate, DateTime endDate, int page, int pageSize)
        {
            PaginationDTO<ShowTimeView> pagination = new PaginationDTO<ShowTimeView>();
            List<ShowTime> showTimes;

            if (startDate.Date == endDate.Date)
            {
                // Lấy tất cả suất chiếu trong startDate nếu startDate bằng endDate
                showTimes = _context.ShowTimes.Include(item => item.Film).ThenInclude(item => item.TypeOfFilm)
                    .Include(item => item.Room).Where(item => item.Time.Date == startDate.Date).ToList();
            }
            else
            {
                // Lấy tất cả suất chiếu trong khoảng từ startDate đến endDate
                showTimes = _context.ShowTimes.Include(item => item.Film).ThenInclude(item => item.TypeOfFilm)
                    .Include(item => item.Room).Where(item => item.Time.Date >= startDate.Date && item.Time.Date <= endDate.Date)
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
                TypeName = item.Film.TypeOfFilm.Name
            }).ToList();
            var result = PaginatedList<ShowTimeView>.Create(showTime.AsQueryable(), page, pageSize);
            pagination.data = result;
            pagination.page = page;
            pagination.totalItem = showTime.Count();
            pagination.pageSize = pageSize;
            return pagination;
        }
    }
}
