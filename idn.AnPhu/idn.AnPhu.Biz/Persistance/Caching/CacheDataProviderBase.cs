using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Core.Data.DataAccess;
using Client.Core.Data.Entities;
using Client.Core.Data;
using Client.Core.Caching;

namespace idn.AnPhu.Biz.Persistance.Caching
{
    public abstract class CacheDataProviderBase<T>
       where T : EntityBase
    {
        private string cacheName = "contents";

        protected System.Runtime.Caching.ObjectCache Cache
        {
            get
            {
                return CacheManagerFactory.DefaultCacheManager.GetObjectCache(cacheName);

            }
        }

        protected IDataProvider<T> innerRepository;
        public CacheDataProviderBase(IDataProvider<T> inner)
        {
            this.innerRepository = inner;
        }
        public virtual T Get(T dummy)
        {
            var cacheKey = GetCacheKey(dummy);


            var o = Cache.Get(cacheKey);
            if (o == null)
            {
                o = innerRepository.Get(dummy);
                if (o == null)
                {
                    return (T)o;
                }
                Cache.Add(cacheKey, o, ObjectCacheExtensions.DefaultCacheItemPolicy);
            }
            return (T)o;
        }

        protected abstract string GetCacheKey(T o);

        public virtual void Add(T item)
        {
            innerRepository.Add(item);
        }

        public virtual void Update(T @new, T old)
        {
            innerRepository.Update(@new, old);
            var cacheKey = GetCacheKey(@new);
            Cache.Remove(cacheKey);
        }

        public virtual void Remove(T item)
        {
            innerRepository.Remove(item);
            var cacheKey = GetCacheKey(item);
            Cache.Remove(cacheKey);
        }

    }
}
