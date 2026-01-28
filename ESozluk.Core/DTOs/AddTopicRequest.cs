using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class AddTopicRequest
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
    }
}
