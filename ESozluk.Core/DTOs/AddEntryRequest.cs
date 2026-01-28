using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class AddEntryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }

    }
}
