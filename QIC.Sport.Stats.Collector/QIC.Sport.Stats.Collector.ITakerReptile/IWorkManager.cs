using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
   public interface IWorkManager
    {
        void AddOrUpdateParam(BaseParam baseParam);
        void RemoveParam(BaseParam baseParam);
        void EnQueueData(BaseData baseData);
        void Start();
        void Stop();
    }
}
