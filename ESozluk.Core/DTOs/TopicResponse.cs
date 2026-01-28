using ESozluk.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class TopicResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        //entry sayısı
    }
}
