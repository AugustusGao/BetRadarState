using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class TeamHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            //  解析队伍基本信息

            //  解析进球数获得队员的点球个数，附值到队员信息中

            //  解析全部名单，并添加球员任务

            //  如果有添加获取伤停的任务




            //  ---以下不用再请求，可以根据队员的参赛记录而得到  return

            //  最佳名单，根据进球按进球，点球排序获得

            //  如果有添加获取牌的任务

            //  如果有添加获取助攻的任务
        }
    }
}
