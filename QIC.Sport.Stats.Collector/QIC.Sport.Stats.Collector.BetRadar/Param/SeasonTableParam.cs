using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class SeasonTableParam : BRBaseParam
    {
        public string SeasonTypeId;
        public string TableId;
        public SeasonTableParam()
        {
            HandleType = (int)RBHandleType.SeasonTable;
            CustomUrl = "gismo.php?&html=1&id=1424&language=zh&clientid=4&state=2_{0}%2C3_{2}%2C22_{1}%2C5_{3}%2C9_overview%2C239_t{4}%2C242_{5}&child=0";
        }

        public override string GetUrl()
        {
            var url = string.Format(CustomUrl, SportId, ContinentId, OrganizerId, SeasonId, TableId, SeasonTypeId);
            var sha1 = EncryptToSHA1(url);
            return BaseUrl + url + "&callback=" + sha1;
        }

        public override string GetKey()
        {
            return SeasonId + "_" + (RBHandleType)HandleType + "_" + SeasonTypeId + "_" + TableId;
        }
    }
}
