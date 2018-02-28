using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache
{
    public class BaseCacheManager : ICacheManager
    {
        protected ConcurrentDictionary<string, BaseCacheEntity> DicCacheData = new ConcurrentDictionary<string, BaseCacheEntity>();

        public int Count { get { return DicCacheData.Count; } }
        public T AddOrGetCacheEntity<T>(string key) where T : BaseCacheEntity, new()
        {
            T ret = new T();
            DicCacheData.AddOrUpdate(key, ret, (k, v) =>
            {
                ret = (T)v;
                return v;
            });
            return ret;
        }


        public void Remove(string key)
        {
            BaseCacheEntity bce;
            DicCacheData.TryRemove(key, out bce);
        }
    }
}
