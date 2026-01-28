using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Entities
{
    public class EntryComplaint
    {
        public string Reason { get; set; } // Şikayet Nedeni 
        public string Description { get; set; } // Detaylı Açıklama
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsResolved { get; set; } = false; // admin inceledi mi?


        public int UserId { get; set; }
        public User User { get; set; }

        public int EntryId { get; set; }
        public Entry Entry { get; set; }
    }
}
