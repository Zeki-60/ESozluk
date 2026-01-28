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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public User? GetUserByEmailWithRoles(string email)
        {
            return _context.Users
                .Include(u => u.UserRoles)      // Ara tabloyu dahil et
                .ThenInclude(ur => ur.Role)     // Ara tablodan Role tablosuna git
                .FirstOrDefault(x => x.Email == email);
        }
        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }
        public User GetByResetToken(string token)
        {
            return _context.Users.FirstOrDefault(u => u.PasswordResetToken == token);
        }

    }
}
