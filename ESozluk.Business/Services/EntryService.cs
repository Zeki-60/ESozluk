using AutoMapper;
using Core.Extensions;
using ESozluk.Business.Utilities;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Exceptions;
using ESozluk.Domain.Interfaces;
using ESozluk.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;


namespace ESozluk.Business.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITopicRepository _topicRepository;
        private readonly IAuthService _authService;

        public EntryService(IEntryRepository repository, IMapper mapper,IUserRepository userRepository,ITopicRepository topicRepository,IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _topicRepository = topicRepository;
            _httpContextAccessor = httpContextAccessor;
            _authService=authService;
        }
        

        public void AddEntry(AddEntryRequest request, int currentUserId)
        {

            
            int user = currentUserId;
            var topic = _topicRepository.GetById(request.TopicId);
            
            
            (topic==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorTopicNotFound));

            

            var entryEntity = _mapper.Map<Entry>(request);
            entryEntity.UserId = user; // <--- Token'dan gelen ID'yi buraya atadık.


            // Tarih alanlarını oto dolduruyoru<ttttt
            entryEntity.CreateDate = DateTime.Now;
            entryEntity.UpdateDate = DateTime.Now;
            _repository.Add(entryEntity);

            
        }

        public EntryListResponse GetAllEntries(string? sort, int page, int pageSize)
        {
            
            var query = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "date":
                        query = query.OrderByDescending(x => x.CreateDate);
                        break;
                    case "popular":
                        query = query.OrderByDescending(x => x.Likes.Count);
                        break;   
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Id); 
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var pagedEntries = query.ToPage(page, pageSize).ToList();

            return new EntryListResponse
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                Entries = _mapper.Map<List<EntryResponse>>(pagedEntries)
            };
        }

        public void UpdateEntry(UpdateEntryRequest request)
        {

            var entry = _repository.GetById(request.Id);

            (entry==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorEntryNotFound));
            
            (entry.UserId != _authService.GetCurrentUserId())
                .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.ErrorUnauthorizedAccess));

            

            _mapper.Map(request, entry);
            entry.UpdateDate = DateTime.Now; // Güncelleme tarihini oto alıyouz

            _repository.UpdateEntry(entry);

            
        }


        public void DeleteEntry(DeleteEntryRequest request,int currentUserId, bool isCurrentUserModerator)
        {
            var entry = _repository.GetById(request.Id);
            (entry==null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorEntryNotFound));


            //var currentUser = _httpContextAccessor.HttpContext?.User;//düzelt
            //bool isModerator = currentUser.IsInRole("Moderator");
            (entry.UserId != currentUserId && !isCurrentUserModerator)
                .IfTrueThrow(() => new AuthorizedAccessException(Resources.SharedResource.ErrorUnauthorizedAccess));
            
            _repository.DeleteEntry(entry);
        }
    }
}