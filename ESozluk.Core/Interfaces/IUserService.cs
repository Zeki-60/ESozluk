using ESozluk.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface IUserService
    {
        List<UserResponse> GetAllUsers();
        void AddUser(AddUserRequest request);
        void DeleteUser(DeleteUserRequest request);
        void UpdateUser(UpdateUserRequest request);
        int GetCurrentUserId();
    }
}
