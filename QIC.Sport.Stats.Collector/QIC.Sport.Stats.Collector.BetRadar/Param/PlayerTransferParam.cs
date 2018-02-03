using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QIC.Sport.Stats.Collector.BetRadar.Param
{
    public class PlayerTransferParam : BRBaseParam
    {
        public PlayerTransferParam()
        {
            HandleType = (int)RBHandleType.PlayerTransfer;
            CustomUrl =
                "gismo.php?&html=1&id=2052&language=zh&clientid=4&state=2_{0}%2C3_{1}%2C22_{2}%2C5_{3}%2C9_players%2C6_{4}%2C174_{5}%2C256_2&child=0";
        }
        public override string GetKey()
        {
            return PlayerId + "_" + (RBHandleType)HandleType;
        }
    }
}
