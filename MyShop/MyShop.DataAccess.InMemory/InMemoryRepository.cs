using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;

        string ClassName;

        public InMemoryRepository()
        {
            //get a class name
            ClassName = typeof(T).Name;
            items = cache[ClassName] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            //store.commit items to cache memory
            cache[ClassName] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);

            if (tToUpdate == null)
            {
                throw new Exception($"{ClassName} Not found");
            }

            tToUpdate = t;
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);

            if (t == null)
            {
                throw new Exception($"{ClassName} Not found");
            }

            return t;
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(i => i.Id == Id);

            if (tToDelete == null)
            {
                throw new Exception($"{ClassName} Not found");
            }

            items.Remove(tToDelete);
        }
    }
}
