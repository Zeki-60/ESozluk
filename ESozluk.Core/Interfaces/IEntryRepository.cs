using ESozluk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Core.Interfaces
{
    public interface IEntryRepository
    {
        List<Entry> GetAll();
        void Add(Entry entry);
        void UpdateEntry(Entry entry);
        void DeleteEntry(Entry entry);
        Entry? GetById(int id);
        IQueryable<Entry> GetAllQueryable();

    }
}
