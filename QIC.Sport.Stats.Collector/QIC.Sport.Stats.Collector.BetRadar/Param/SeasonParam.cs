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
        public string CurrentUrlKey = "season";
        private string allTableIdsUrl = "gismo.php?&html=1&id=1424&language=zh&clientid=4&state=2_{0}%2C3_{2}%2C22_{1}%2C5_{3}%2C%2C9_overview&child=0";
        private string allTeamIdsUrl = "gismo.php?&html=1&id=1831&language=zh&clientid=4&state=2_{0}%2C3_{2}%2C22_{1}%2C5_{3}%2C%2C9_teampage&child=0";
        public SeasonParam()
        {
            HandleType = (int)RBHandleType.Season;
            CustomUrl = "gismo.php?&html=1&id=2099&language=zh&clientid=4&state=2_{0}%2C3_{2}%2C22_{1}%2C5_{3}%2C9_overview%2C25_1&child=3";
        }

        /// <summary>
        /// 设置参数拿取Url
        /// </summary>
        /// <param name="key">tableIds 拿取所有组的,teamIds拿取所有队伍的</param>
        public void SetCurrentUrlKey(string key)
        {
            switch (key)
            {
                case "tableIds":
                    CustomUrl = allTableIdsUrl;
                    CurrentUrlKey = "tableIds";
                    break;
                case "teamIds":
                    CustomUrl = allTeamIdsUrl;
                    CurrentUrlKey = "teamIds";
                    break;

            }
        }
        public override string GetUrl()
        {
            var url = string.Format(CustomUrl, SportId, ContinentId, OrganizerId, SeasonId);
            var sha1 = EncryptToSHA1(url);
            return BaseUrl + url + "&callback=" + sha1;
        }

        public override string GetKey()
        {
            return SeasonId + "_" + (RBHandleType)HandleType + "_" + CurrentUrlKey;
        }
    }
}
