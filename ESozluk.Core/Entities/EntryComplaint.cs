using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Entities
{
    public class EntryComplaint
    {
        public int Id { get; set; }
        public string Reason { get; set; } 
        public string Description { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false; 


        public int UserId { get; set; }
        public User User { get; set; }

        public int EntryId { get; set; }
        public Entry Entry { get; set; }
    }
}
