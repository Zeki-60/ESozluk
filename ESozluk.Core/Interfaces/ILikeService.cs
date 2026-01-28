using ESozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface ILikeService
    {
        string ToggleLike(int productId,int currentUserId);
        List<EntryResponse> GetMyLikedEntries(int currentUserId);
    }
}
