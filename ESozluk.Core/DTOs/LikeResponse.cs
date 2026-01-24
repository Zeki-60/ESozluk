using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.DTOs
{
    public class LikeResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EntryName { get; set; }
        public DateTime LikeDate { get; set; }
    }
}
