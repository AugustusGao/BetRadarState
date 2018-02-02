using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.Cache
{
    public class BaseCacheEntity
    {
        public virtual bool Equal(BaseCacheEntity entity) { return false; }
        public virtual string GetKey() { return null; }
    }
}
