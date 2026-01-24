using ESozluk.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface IEntryService
    {
        void AddEntry(AddEntryRequest request);
        void UpdateEntry(UpdateEntryRequest request);
        void DeleteEntry(DeleteEntryRequest request);
        EntryListResponse GetAllEntries(string? sort, int page, int pageSize);


    }
}
