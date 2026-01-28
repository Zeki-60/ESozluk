using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface ITopicRepository
    {
        List<Topic> GetAll();
        void AddTopic(Topic topic);
        void UpdateTopic(Topic topic);
        void DeleteTopic(Topic topic);
        Topic? GetById(int Id);
        Topic GetTopicWithEntries(int id);

    }
}
