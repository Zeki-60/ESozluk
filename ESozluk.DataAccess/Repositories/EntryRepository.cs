using ESozluk.Domain.Entities;
using ESozluk.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.DataAccess.Repositories
{
    public class EntryRepository :IEntryRepository
    {
        private readonly AppDbContext _context;
        public EntryRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Entry entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }
        public List<Entry> GetAll()
        {
            return _context.Entries
                   .Include(x => x.User)
                   .Include(x => x.Topic)
                   .Include(x => x.Likes)
                   .ToList();
        }
        public IQueryable<Entry> GetAllQueryable()
        {
            return _context.Entries
                   .Include(x => x.User)
                   .Include(x => x.Topic)
                   .Include(x => x.Likes)
                   .AsQueryable();
        }

        public Entry? GetById(int id)
        {

            return _context.Entries
                .Include(x => x.User) 
                .Include(x => x.Topic)
                .Include(x => x.Likes)
                 .FirstOrDefault(x => x.Id == id);
        }

        public void UpdateEntry(Entry entry)
        {
            _context.Entries.Update(entry);
            _context.SaveChanges();
        }

        public void DeleteEntry(Entry entry)
        {
            _context.Entries.Remove(entry);
            _context.SaveChanges();
        }

    }

    
}
