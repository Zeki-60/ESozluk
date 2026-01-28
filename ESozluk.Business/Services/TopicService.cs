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
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthService _authService;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public TopicService(ITopicRepository repository, IMapper mapper, IUserRepository userRepository, ICategoryRepository categoryRepository, IAuthService authService, IStringLocalizer<SharedResource> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _authService= authService;
            _localizer = localizer;
        }

        public TopicWithEntriesResponse GetTopicWithEntries(int topicId,string sort)
        {
            var topic = _repository.GetTopicWithEntries(topicId);

            (topic==null)
                .IfTrueThrow(() => new NotFoundException(_localizer["ErrorTopicNotFound"]));

            var response = new TopicWithEntriesResponse
            {
                Id = topic.Id,
                Name = topic.Name,
                Entries = _mapper.Map<List<EntryResponse>>(topic.Entries)
            };
            switch (sort.ToLower())
            {
                case "date": // En YENİ ürün en üstte, id ye göre sıraladık çünkü bu da bir nevi bize tarihi veriyor.
                    response.Entries = response.Entries.OrderByDescending(x => x.Id).ToList();
                    break;

                case "popular": // En ÇOK LIKE alan en üstte
                    response.Entries = response.Entries.OrderByDescending(x => x.LikeCount).ToList();
                    break;
            }


            return response;
        }


        public void AddTopic(AddTopicRequest request)
        {
            var user = _userRepository.GetById(request.UserId);
            var category= _categoryRepository.GetById(request.CategoryId);

            (user==null)
                .IfTrueThrow(() => new NotFoundException(_localizer["UserNotFound"]));

           
            (category==null)
                .IfTrueThrow(() => new NotFoundException(_localizer["ErrorCategoryNotFound"]));
            

            var topicEntity = _mapper.Map<Topic>(request);
            topicEntity.CreateDate = DateTime.Now;

            _repository.AddTopic(topicEntity);

            
        }

        public List<TopicResponse> GetAllTopics()
        {
            var topics = _repository.GetAll();
            return _mapper.Map<List<TopicResponse>>(topics);

        }

        public void UpdateTopic(UpdateTopicRequest request,int currentUserId)
        {
            var topic = _repository.GetById(request.Id);

            (topic == null)
                .IfTrueThrow(() => new NotFoundException(_localizer["ErrorTopicNotFound"]));

            (topic.UserId != currentUserId)
                .IfTrueThrow(() => new AuthorizedAccessException(_localizer["ErrorUnauthorizedAccess"]));

            

            _mapper.Map(request, topic);
            _repository.UpdateTopic(topic);

            
        }
        public void DeleteTopic(DeleteTopicRequest request,int currentUserId)
        {
            var topic = _repository.GetById(request.Id);

            (topic == null)
                .IfTrueThrow(() => new NotFoundException(_localizer["ErrorTopicNotFound"]));
            (topic.UserId != currentUserId)
                .IfTrueThrow(() => new AuthorizedAccessException(_localizer["ErrorUnauthorizedAccess"]));


            _repository.DeleteTopic(topic);
        }
    }
}