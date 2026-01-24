using ESozluk.Core.Entities;
using ESozluk.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.DataAccess.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;

        public TopicRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            _context.SaveChanges();
        }
        public List<Topic> GetAll() {
            return _context.Topics
                               .Include(x => x.User)
                               .Include(x => x.Category)
                               .ToList();
        }

        public Topic? GetById(int id)
        {
            return _context.Topics.Find(id);
        }

        public void UpdateTopic(Topic topic)
        {
            _context.Topics.Update(topic);
            _context.SaveChanges();
        }
        public Topic GetTopicWithEntries(int id)
        {
            return _context.Topics
                .Include(x => x.Entries)
                .ThenInclude(t => t.User)
                .Include(x => x.Entries)
                .ThenInclude(t => t.Likes)
                .FirstOrDefault(x => x.Id == id);
        }

        public void DeleteTopic(Topic topic)
        {
            _context.Topics.Remove(topic);
            _context.SaveChanges();
        }
    }
}
