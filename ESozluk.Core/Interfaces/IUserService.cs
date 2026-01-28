using ESozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface IUserService
    {
        List<UserResponse> GetAllUsers();
        void AddUser(AddUserRequest request);
        void DeleteUser(DeleteUserRequest request, int currentUserId, bool isCurrentUserAdmin);
        void UpdateUser(UpdateUserRequest request, int currentUserId, bool isCurrentUserAdmin);
    }
}
