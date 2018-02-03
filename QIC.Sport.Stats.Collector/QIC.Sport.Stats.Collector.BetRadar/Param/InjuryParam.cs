using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class InjuryParam : BRBaseParam
    {
        public InjuryParam()
        {
            HandleType = (int)RBHandleType.Injury;
            CustomUrl = "gismo.php?&html=1&id=3013&language=zh&clientid=4&state=2_{0}%2C3_{1}%2C22_{2}%2C5_{3}%2C9_teampage%2C6_{4}%2C20_8&child=0";
        }
        public override string GetKey()
        {
            return SeasonId + "_" + TeamId + "_" + (RBHandleType)HandleType;
        }
    }
}
