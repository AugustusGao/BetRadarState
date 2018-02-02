using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class LeagueHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            LeagueParam param = bd.Param as LeagueParam;

            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  获取联赛名称
            var title = xml.GetAttributeValue("//page", "title");
            var seasonName = title.Split('>').Last();

            //  获取积分数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);
            var html = "";
            if (cdata.Count > 5)
            {
                html = cdata[4];
            }

            //  解析积分数据
            var root = GetHtmlRoot(html);




        }
    }
}
