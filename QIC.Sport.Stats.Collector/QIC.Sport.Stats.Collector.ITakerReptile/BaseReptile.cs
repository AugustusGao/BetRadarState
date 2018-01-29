using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseReptile : IReptile
    {
        protected Dictionary<string, BaseWorkManager> DicWorkManagers = new Dictionary<string, BaseWorkManager>();
        public void InitWorkManager(List<BaseWorkManager> baseWorkManager) { }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
