using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class TopicWithEntriesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<EntryResponse> Entries { get; set; }
    }
}
