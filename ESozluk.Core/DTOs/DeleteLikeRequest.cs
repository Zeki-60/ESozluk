using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ESozluk.Domain.DTOs
{
    public class DeleteLikeRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
    }
}
