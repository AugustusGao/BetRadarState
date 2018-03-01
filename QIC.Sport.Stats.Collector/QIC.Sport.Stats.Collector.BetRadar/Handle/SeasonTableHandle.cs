using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Cache.CombinedData;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class SeasonTableHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            SeasonTableParam param = bd.Param as SeasonTableParam;
            CheckSetHistoryParam(param);

            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            var xml = new XmlHelper(txt);
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            var root = GetHtmlRoot(cdata[10]);
            var wrap = root.SelectSingleNode("//div[@class='multiview_wrap']");
            if (wrap == null || wrap.ChildNodes == null) return;

            var tableId = RegexGetStr(wrap.ChildNodes[0].OuterHtml, "tableid','", "',");
            var tableName = wrap.ChildNodes[0].InnerText;

            var ti = SeasonTableInfoManager.AddOrGetCacheEntity<SeasonTableInfo>(tableId);
            ti.CompareSet(tableId, tableName);

            //  解析小组积分
            var tbody = wrap.SelectSingleNode("table/tbody");
            List<SeasonTableRank> list = new List<SeasonTableRank>();
            foreach (var tr in tbody.ChildNodes)
            {
                var count = tr.ChildNodes.Count;
                var points = tr.ChildNodes[count - 2].InnerText;
                var description = tr.ChildNodes[count - 1].InnerText;
                var teamId = RegexGetStr(tr.ChildNodes[2].InnerHtml, "teamid','", "',");
                var rank = new SeasonTableRank()
                {
                    TeamId = teamId,
                    TableId = tableId,
                    TeamPoints = points,
                    Description = description
                };
            }
            ti.CompareTableRank(list);
        }
    }
}
