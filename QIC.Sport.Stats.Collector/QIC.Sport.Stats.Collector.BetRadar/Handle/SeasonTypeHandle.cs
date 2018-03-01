using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class SeasonTypeHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            SeasonTypeParam param = bd.Param as SeasonTypeParam;
            CheckSetHistoryParam(param);

            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            var xml = new XmlHelper(txt);
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

        }
    }
}
