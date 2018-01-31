using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseReptile : IReptile
    {
        protected Dictionary<string, IWorkManager> DicWorkManagers = new Dictionary<string, IWorkManager>();

        public void Start()
        {
            // todo 循环Start各个BaseWorkManager
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
    }
}
