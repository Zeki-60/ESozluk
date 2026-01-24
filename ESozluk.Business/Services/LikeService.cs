using AutoMapper;
using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using ESozluk.Core.Exceptions;
using ESozluk.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Business.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public LikeService(ILikeRepository repository, IMapper mapper, IUserRepository userRepository, IEntryRepository entryRepository, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _entryRepository = entryRepository;
            _httpContextAccessor= httpContextAccessor;
            _userService= userService;
        }
        
        public string ToggleLike(int entryId)
        {
            int userId = _userService.GetCurrentUserId();

            var entry=_entryRepository.GetById(entryId);
            if (entry == null)
            {
                throw new NotFoundException("Entry bulunamadı");
            }
            var existingLike = _repository.GetLike(userId, entryId);

            if (existingLike != null)
            {
                _repository.RemoveLike(existingLike);
                entry.LikeCount--;
                return "Beğeni kaldırıldı.";
            }
            else
            {
                var newLike = new Like
                {
                    UserId = userId,
                    EntryId = entryId
                };
                _repository.AddLike(newLike);
                entry.LikeCount++;
                return "Beğeni eklendi.";
            }
        }

        public List<EntryResponse> GetMyLikedEntries()
        {
            int userId = _userService.GetCurrentUserId();
            var entries = _repository.GetLikedEntriesByUser(userId);
            return _mapper.Map<List<EntryResponse>>(entries);
        }
    }
}