using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface IComplaintRepository
    {
        void Add(EntryComplaint complaint);
        List<EntryComplaint> GetAllWithDetails(); // Admin için detaylı liste
        List<EntryComplaint> GetByUserId(int userId); // Kullanıcı kendi şikayetlerini görsün
    }
}
