using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
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
            CheckSetHistoryParam(param);

            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValue(cdataFlag);
            if (string.IsNullOrEmpty(cdata)) return;

            var root = GetHtmlRoot(cdata);
            var tbody = root.SelectSingleNode("//tbody");
            List<InjurePlayer> list = new List<InjurePlayer>();
            foreach (var node in tbody.ChildNodes)
            {
                var player = node.SelectSingleNode("td[@class='player']");
                if (player == null) continue;

                InjurePlayer injure = new InjurePlayer();
                injure.PlayerId = RegexGetStr(player.InnerHtml, "playerid', '", "',");
                injure.Missing = node.SelectSingleNode("td[@class='missing']").InnerText;
                injure.Status = node.SelectSingleNode("td[@class='reason ']").InnerText;
                list.Add(injure);
            }
            var tp = TeamPlayersManager.AddOrGetCacheEntity<TeamPlayers>(param.TeamId + "_" + param.SeasonId);
            tp.CompareSetInjurePlayerList(list);
        }
    }
}
