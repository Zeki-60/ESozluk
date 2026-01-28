using ESozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface IComplaintService
    {
        void AddComplaint(AddComplaintRequest request);
        List<ComplaintResponse> GetAllComplaints(); // Admin
        List<ComplaintResponse> GetMyComplaints();  // User
    }
}
