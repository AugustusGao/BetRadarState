using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Common;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class TeamHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            SeasonParam param = bd.Param as SeasonParam;

            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  解析队伍基本信息

            //  解析进球数获得队员的点球个数，更新到队员点球信息缓存中

            //  解析全部名单，并添加球员任务
            //  15
            var playersData = cdata[16];
            var root = GetHtmlRoot(playersData);
            var body = root.SelectSingleNode("//tbody");
            var trs = body.SelectNodes("/tr[@onclick=*]");
            List<string> list=new List<string>();
            foreach (var node in body.ChildNodes)
            {
                var playerId = RegGetStr(node.InnerHtml, "playerid', '", "',");
                    list.Add(playerId);
            }

            //  如果有添加获取伤停的任务




            //  ---以下不用再请求，可以根据队员的参赛记录而得到  return

            //  最佳名单，根据进球按进球，点球排序获得

            //  如果有添加获取牌的任务

            //  如果有添加获取助攻的任务
        }
    }
}
