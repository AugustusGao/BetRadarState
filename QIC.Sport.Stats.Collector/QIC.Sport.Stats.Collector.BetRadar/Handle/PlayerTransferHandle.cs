using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class PlayerTransferHandle : BaseHandle, IHandle
    {
        //  解析转会记录，缓存到PlayerEntity中
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            PlayerTransferParam param = bd.Param as PlayerTransferParam;
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
            var player = PlayerEntityManager.AddOrGetCacheEntity<PlayerEntity>(param.PlayerId);
            foreach (var node in tbody.ChildNodes)
            {
                TransferHistory th = new TransferHistory();
                th.TeamId = RegexGetStr(node.ChildNodes[0].InnerHtml, "teamid','", "',");
                th.DuringTime = node.ChildNodes[1].InnerText;
                th.Description = node.ChildNodes[2].InnerText;
                player.CompareSetTransferHistory(th);
            }
        }
    }
}
