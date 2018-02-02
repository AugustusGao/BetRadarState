using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CacheData;

namespace QIC.Sport.Stats.Collector.Cache.CacheDataManager
{
    public class OrganizerEntityManager : BaseCacheManager, ICacheManager
    {
        public BaseCacheEntity AddOrGetCacheEntity(string key)
        {
            OrganizerEntity ret = new OrganizerEntity();
            DicCacheData.AddOrUpdate(key, ret, (k, v) =>
            {
                ret = (OrganizerEntity)v;
                return v;
            });
            return ret;
        }

        public void Remove(BaseCacheEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
