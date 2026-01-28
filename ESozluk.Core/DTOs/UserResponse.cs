using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string PasswordHash { get; set; }
        //public string PasswordSalt { get; set; }
        public DateTime RegistrationDate { get; set; }
        
    }
}
