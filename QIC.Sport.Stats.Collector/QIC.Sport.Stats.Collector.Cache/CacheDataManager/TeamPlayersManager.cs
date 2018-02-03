using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache.CacheData;

namespace QIC.Sport.Stats.Collector.Cache.CacheDataManager
{
    public class TeamPlayersManager : BaseCacheManager, ICacheManager
    {
        public BaseCacheEntity AddOrGetCacheEntity(string key)
        {
            TeamPlayers ret = new TeamPlayers();
            DicCacheData.AddOrUpdate(key, ret, (k, v) =>
            {
                ret = (TeamPlayers)v;
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
