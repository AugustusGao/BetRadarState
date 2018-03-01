using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseReptile : IReptile
    {
        protected Dictionary<string, IWorkManager> DicWorkManagers = new Dictionary<string, IWorkManager>();
        protected Dictionary<string, ICacheManager> DicCacheManagers = new Dictionary<string, ICacheManager>();

        public void Start()
        {
            foreach (var manager in DicWorkManagers.Values)
            {
                manager.Start();
            }
        }

        public void Stop()
        {
            foreach (var manager in DicWorkManagers.Values)
            {
                manager.Stop();
            }
        }

        public virtual void InitWorkManager()
        {

        }

        public virtual void InitCacheManager()
        {

        }
    }
}
