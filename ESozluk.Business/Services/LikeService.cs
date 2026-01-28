using AutoMapper;
using Core.Extensions;
using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using ESozluk.Domain.Exceptions;
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
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthService _authService;

        public LikeService(ILikeRepository repository, IMapper mapper, IUserRepository userRepository, IEntryRepository entryRepository, IHttpContextAccessor httpContextAccessor, IAuthService authService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _entryRepository = entryRepository;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }

        public string ToggleLike(int entryId,int currentUserId)
        {

            var entry = _entryRepository.GetById(entryId);
            (entry == null)
                .IfTrueThrow(() => new NotFoundException(Resources.SharedResource.ErrorEntryNotFound));


            var existingLike = _repository.GetLike(currentUserId, entryId);

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
                    UserId = currentUserId,
                    EntryId = entryId
                };
                _repository.AddLike(newLike);
                entry.LikeCount++;
                return "Beğeni eklendi.";
            }
        }

        public List<EntryResponse> GetMyLikedEntries(int currentUserId)
        {
            
            var entries = _repository.GetLikedEntriesByUser(currentUserId);
            return _mapper.Map<List<EntryResponse>>(entries);
        }
    }
}