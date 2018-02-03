using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CacheData;

namespace QIC.Sport.Stats.Collector.Cache.CacheDataManager
{
    public class SeasonEntityManager : BaseCacheManager, ICacheManager
    {

        public BaseCacheEntity AddOrGetCacheEntity(string key)
        {
            SeasonEntity ret = new SeasonEntity();
            DicCacheData.AddOrUpdate(key, ret, (k, v) =>
            {
                ret = (SeasonEntity)v;
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
