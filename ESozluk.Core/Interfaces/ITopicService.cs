using ESozluk.Domain.DTOs;
using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface ITopicService
    {
        List<TopicResponse> GetAllTopics();
        void AddTopic(AddTopicRequest request);
        void DeleteTopic(DeleteTopicRequest request,int currentUserId);
        void UpdateTopic(UpdateTopicRequest request,int currentUserId);
        TopicWithEntriesResponse GetTopicWithEntries(int categoryId,string sort);

    }
}
