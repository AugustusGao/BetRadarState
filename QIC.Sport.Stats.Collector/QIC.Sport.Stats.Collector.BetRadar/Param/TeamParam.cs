using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class TeamParam : BRBaseParam
    {
        public TeamParam()
        {
            HandleType = (int)RBHandleType.Team;
            CustomUrl = "gismo.php?&html=1&id=1831&language=zh&clientid=4&state=2_{0}%2C3_{1}%2C22_{2}%2C5_{3}%2C9_teampage%2C6_{4}&child=0";
        }

        public override string GetKey()
        {
            return TeamId + "_" + (RBHandleType)HandleType;
        }

        public override string GetUrl()
        {
            var url = string.Format(CustomUrl, SportId, OrganizerId, ContinentId, SeasonId, TeamId);
            var sha1 = EncryptToSHA1(url);
            return BaseUrl + url + "&callback=" + sha1;
        }
    }
}
