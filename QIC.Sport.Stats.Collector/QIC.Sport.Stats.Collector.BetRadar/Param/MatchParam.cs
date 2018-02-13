using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class MatchParam : BRBaseParam
    {
        public MatchParam()
        {
            HandleType = (int)RBHandleType.Match;
            CustomUrl = "gismo.php?&html=1&id=1827&gatracker=UA-79266371-40&language=zh&clientid=4&state=2_{0}%2C3_{1}%2C22_{2}%2C5_{3}%2C9_fixtures%2C242_21%2C231_full%2C23_1";

        }

        public override string GetUrl()
        {
            var url = string.Format(CustomUrl, SportId, OrganizerId, ContinentId, SeasonId);
            var sha1 = EncryptToSHA1(url);
            return BaseUrl + url + "&callback=" + sha1;
        }

        public override string GetKey()
        {
            return SeasonId + "_" + (RBHandleType)HandleType;
        }
    }
}
