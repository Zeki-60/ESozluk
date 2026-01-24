using ESozluk.Core.Entities;
using ESozluk.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ESozluk.DataAccess.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;
        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }
        public Like GetLike(int userId, int entryId)
        {
            return _context.Likes
                .FirstOrDefault(x => x.UserId == userId && x.EntryId == entryId);
        }

        public void AddLike(Like like)
        {
            _context.Likes.Add(like);
            _context.SaveChanges();
        }

        public void RemoveLike(Like like)
        {
            _context.Likes.Remove(like);
            _context.SaveChanges();
        }

        public List<Entry> GetLikedEntriesByUser(int userId)
        {
            return _context.Likes
            .Where(x => x.UserId == userId)
            .Include(x => x.Entry)          // Entry'yi getir
            .ThenInclude(e => e.User)   // Entry'nin yazarını da getir (UserName için)
            .Include(x => x.Entry)
            .ThenInclude(e => e.Topic)  // Entry'nin topiğini de getir (TopicName için)
            .Include(x => x.Entry)
            .ThenInclude(e => e.Likes)
            .Select(x => x.Entry)           // Sadece Entry nesnelerini listele
            .ToList();
        }






    }
}
