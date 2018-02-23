using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class InjuryHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            //  解析队伍伤停人员缓存到TeamPlayers中
            BRData bd = data as BRData;
            InjuryParam param = bd.Param as InjuryParam;

            if (string.IsNullOrEmpty(bd.Html)) return;
            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValue(cdataFlag);

            var root = GetHtmlRoot(cdata);
            var tbody = root.SelectSingleNode("//tbody");
            List<InjurePlayer> list = new List<InjurePlayer>();
            foreach (var node in tbody.ChildNodes)
            {
                var player = node.SelectSingleNode("td[@class='player']");
                if (player == null) continue;

                InjurePlayer injure = new InjurePlayer();
                injure.PlayerId = RegGetStr(player.InnerHtml, "playerid', '", "',");
                injure.Missing = node.SelectSingleNode("td[@class='missing']").InnerText;
                injure.Status = node.SelectSingleNode("td[@class='reason ']").InnerText;
                list.Add(injure);
            }
            var m = IocUnity.GetService<ICacheManager>(typeof(TeamPlayersManager).Name);
            var tp = m.AddOrGetCacheEntity<TeamPlayers>(param.TeamId + "_" + param.SeasonId);
            tp.CompareInjurePlayerList(list);
        }
    }
}
