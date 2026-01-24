using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        User? GetById(int Id);
        User? GetUserByEmailWithRoles(string email);
        User? GetByEmail(string email);
        User? GetByResetToken(string token);
    }
}
