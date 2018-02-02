using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.ITakerReptile.Dto
{
    public class BaseParam
    {
        public int HandleType { get; set; }
        public virtual string GetKey() { return null; }
    }
}
