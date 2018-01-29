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
        /// <summary>
        /// 初始化各个工作管理到爬虫上
        /// </summary>
        /// <param name="baseWorkManager"></param>
        void InitWorkManager(List<BaseWorkManager> baseWorkManager);

        //  todo 还需初始化缓存管理
    }
}
