using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Cache;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class PlayerHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            //  解析球员基本信息

            //  解析全部参赛记录
            //  记录比赛Id
            //  缓存出场信息PlayerTimeRecord
            //  缓存进球，牌，助攻信息PlayerStatisticsRecord


            //  如果有添加获取转会记录的任务


        }
    }
}
