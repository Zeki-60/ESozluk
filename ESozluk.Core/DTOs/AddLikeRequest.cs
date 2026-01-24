using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.DTOs
{
    public class AddLikeRequest
    {
        public int UserId { get; set; }
        public int EntryId { get; set; }
    }
}
