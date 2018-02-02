using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.BetRadar;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class SeasonParam : BRBaseParam
    {
        private string topRankUrl = "gismo.php?&html=1&id=2099&language=zh&clientid=4&state=2_{0}%2C3_{2}%2C22_{1}%2C5_{3}%2C9_overview%2C25_1&child=3";
        public string SeasonId;
        public string ContinentId;
        public string OrganizerId;
        public string SportId = "1";

        public SeasonParam()
        {
            HandleType = (int)RBHandleType.Season;
        }

        public override string GetUrl()
        {
            var url = string.Format(topRankUrl, SportId, ContinentId, OrganizerId, SeasonId);
            var sha1 = EncryptToSHA1(url);
            return BaseUrl + url + "&callback=" + sha1;
        }

        public override string GetKey()
        {
            return SeasonId + "_" + (RBHandleType)HandleType;
        }
    }
}
