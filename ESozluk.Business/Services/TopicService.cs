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
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService; 

        public TopicService(ITopicRepository repository, IMapper mapper, IUserRepository userRepository, ICategoryRepository categoryRepository, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _userService= userService;
        }

        public TopicWithEntriesResponse GetTopicWithEntries(int topicId,string sort)
        {
            var topic = _repository.GetTopicWithEntries(topicId);
            if (topic == null)
            {
                throw new NotFoundException("topik bulunamadı");
            }
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

            if (user == null)
            {
                throw new NotFoundException("Kullanıcı Id bulunamadı.");

            }
            if (category == null)
            {
                throw new NotFoundException("Kategori Id bulunamadı.");

            }

            var topicEntity = _mapper.Map<Topic>(request);
            topicEntity.CreateDate = DateTime.Now;

            _repository.AddTopic(topicEntity);

            
        }

        public List<TopicResponse> GetAllTopics()
        {
            var topics = _repository.GetAll();
            return _mapper.Map<List<TopicResponse>>(topics);

        }

        public void UpdateTopic(UpdateTopicRequest request)
        {
            var topic = _repository.GetById(request.Id);

            if (topic == null)
            {
                throw new NotFoundException("topic bulunamadı.");
            }
            if (topic.UserId != _userService.GetCurrentUserId())
            {
                throw new AuthorizedAccessException("Bu topiği güncelleme yetkiniz yok.");
            }

            _mapper.Map(request, topic);
            _repository.UpdateTopic(topic);

            
        }
        public void DeleteTopic(DeleteTopicRequest request)
        {
            var topic = _repository.GetById(request.Id);
            if (topic == null)
            {
                throw new NotFoundException("topic bulunamadı.");
            }
            if (topic.UserId != _userService.GetCurrentUserId())
            {
                throw new AuthorizedAccessException("Bu topiği silme yetkiniz yok.");
            }

            _repository.DeleteTopic(topic);
        }
    }
}