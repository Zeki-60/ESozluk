using ESozluk.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface IEntryService
    {
        void AddEntry(AddEntryRequest request, int currentUserId);
        void UpdateEntry(UpdateEntryRequest request);
        void DeleteEntry(DeleteEntryRequest request,int currentUserId, bool isCurrentUserModerator);
        EntryListResponse GetAllEntries(string? sort, int page, int pageSize);


    }
}
