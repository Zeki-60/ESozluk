using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface ILikeRepository
    {
        

        Like GetLike(int userId, int productId);

        void AddLike(Like like);
        void RemoveLike(Like like);

        // Kullanıcının beğendiği ürünlerin listesini getir
        List<Entry> GetLikedEntriesByUser(int userId);


    }
}
