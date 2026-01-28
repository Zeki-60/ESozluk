using AutoMapper;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _repo;
        private readonly IEntryRepository _entryRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComplaintService(
            IComplaintRepository repo,
            IEntryRepository productRepo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
           )
        {
            _repo = repo;
            _entryRepo = productRepo;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) throw new UnauthorizedAccessException(Resources.SharedResource.UserNotFound);
            return int.Parse(userIdClaim.Value);
        }

        public void AddComplaint(AddComplaintRequest request)
        {
            var entry = _entryRepo.GetById(request.EntryId);
            if (entry == null)
            {
                //throw new Exception(Resources.SharedResource.);
            }

            int userId = GetCurrentUserId();

            var complaint = _mapper.Map<EntryComplaint>(request);
            complaint.UserId = userId;
            complaint.CreatedAt = DateTime.Now;
            complaint.IsResolved = false;

            _repo.Add(complaint);
        }

        public List<ComplaintResponse> GetAllComplaints()
        {
            var list = _repo.GetAllWithDetails();
            return _mapper.Map<List<ComplaintResponse>>(list);
        }

        public List<ComplaintResponse> GetMyComplaints()
        {
            int userId = GetCurrentUserId();
            var list = _repo.GetByUserId(userId);
            return _mapper.Map<List<ComplaintResponse>>(list);
        }
    }
}
