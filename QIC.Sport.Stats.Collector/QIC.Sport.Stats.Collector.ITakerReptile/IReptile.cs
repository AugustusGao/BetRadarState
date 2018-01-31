using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public interface IReptile
    {
        /// <summary>
        /// 启动所有工作
        /// </summary>
        void Start();
        /// <summary>
        /// 停止所有工作
        /// </summary>
        void Stop();
    }
}
