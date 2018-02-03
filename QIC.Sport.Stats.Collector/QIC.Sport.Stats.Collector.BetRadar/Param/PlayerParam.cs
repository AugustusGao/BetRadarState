using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class PlayerParam : BRBaseParam
    {
        public PlayerParam()
        {
            HandleType = (int)RBHandleType.Player;
            CustomUrl =
                "gismo.php?&html=1&id=1374&language=zh&clientid=4&state=2_{0}%2C3_{1}%2C22_{2}%2C5_{3}%2C9_players%2C6_{4}%2C174_{5}&child=1";
        }
        public override string GetKey()
        {
            return PlayerId + "_" + (RBHandleType)HandleType;
        }
    }
}
