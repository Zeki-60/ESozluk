using ESozluk.Core.DTOs;
using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface ITopicService
    {
        List<TopicResponse> GetAllTopics();
        void AddTopic(AddTopicRequest request);
        void DeleteTopic(DeleteTopicRequest request);
        void UpdateTopic(UpdateTopicRequest request);
        TopicWithEntriesResponse GetTopicWithEntries(int categoryId,string sort);

    }
}
