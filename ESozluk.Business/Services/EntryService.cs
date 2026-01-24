using AutoMapper;
using ESozluk.Business.Utilities;
using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using ESozluk.Core.Exceptions;
using ESozluk.Core.Interfaces;
using ESozluk.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Services
{
    public class EntryService : IEntryService
    {
        private readonly IEntryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITopicRepository _topicRepository;
        private readonly IUserService _userService;

        public EntryService(IEntryRepository repository, IMapper mapper,IUserRepository userRepository,ITopicRepository topicRepository,IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _topicRepository = topicRepository;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        

        public void AddEntry(AddEntryRequest request)
        {
            
            int user = _userService.GetCurrentUserId();
            var topic = _topicRepository.GetById(request.TopicId);
            
            
            
            if (topic == null)
            {
                throw new NotFoundException("Topic Id bulunamadı.");
            }

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
            if (entry == null)
            {
                throw new NotFoundException("Entry bulunamadı.");
            }
            if (entry.UserId != _userService.GetCurrentUserId())
            {
                throw new UnauthorizedAccessException("Bu entry'i güncelleme yetkiniz yok. Sadece kendi entrylerinizi düzenleyebilirsiniz.");
            }

            _mapper.Map(request, entry);
            entry.UpdateDate = DateTime.Now; // Güncelleme tarihini oto alıyouz

            _repository.UpdateEntry(entry);

            
        }


        public void DeleteEntry(DeleteEntryRequest request)
        {
            var entry = _repository.GetById(request.Id);
            if (entry == null)
            {
                throw new NotFoundException("Entry bulunamadı.");
            }
            var currentUser = _userService;
            bool isModerator = currentUser.IsInRole("Moderator");
            if (entry.UserId != _userService.GetCurrentUserId() && !isModerator)
            {
                throw new AuthorizedAccessException("Bu entry'i silme yetkiniz yok.");
            }
            _repository.DeleteEntry(entry);
        }
    }
}