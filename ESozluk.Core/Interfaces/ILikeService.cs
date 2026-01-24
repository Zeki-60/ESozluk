using ESozluk.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface ILikeService
    {
        string ToggleLike(int productId);
        List<EntryResponse> GetMyLikedEntries();
    }
}
