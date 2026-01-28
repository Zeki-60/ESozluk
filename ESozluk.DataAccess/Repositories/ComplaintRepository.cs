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
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly AppDbContext _context;

        public ComplaintRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(EntryComplaint complaint)
        {
            _context.EntryComplaints.Add(complaint);
            _context.SaveChanges();
        }

        public List<EntryComplaint> GetAllWithDetails()
        {
            return _context.EntryComplaints
                .Include(c => c.User)
                .Include(c => c.Entry)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public List<EntryComplaint> GetByUserId(int userId)
        {
            return _context.EntryComplaints
                .Where(c => c.UserId == userId)
                .Include(c => c.Entry)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }
    }
}
