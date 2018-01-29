using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    interface IWorkManager
    {
        void AddOrUpdateParam();
        void RemoveParam();
        void EnQueueData();
        void Start() { }
        void Stop() { }
    }
}
