using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class CategoryWithTopicsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<TopicResponse> Topics { get; set; }
    }
}
