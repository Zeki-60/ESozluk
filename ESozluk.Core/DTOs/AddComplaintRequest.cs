using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class AddComplaintRequest
    {
        public int EntryId { get; set; }
        public string Entry { get; set; }
        public string Description { get; set; }
    }
}
