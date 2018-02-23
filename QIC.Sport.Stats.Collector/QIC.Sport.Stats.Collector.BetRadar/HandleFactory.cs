using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.BetRadar.Handle;
using QIC.Sport.Stats.Collector.Common;

namespace QIC.Sport.Stats.Collector.BetRadar
{
    public static class HandleFactory
    {
        public static IHandle CreateHandle(int handleType)
        {
            IHandle handle;
            switch ((RBHandleType)handleType)
            {
                case RBHandleType.Organizer:
                    handle = new OrganizerHandle();
                    break;
                case RBHandleType.Season:
                    handle = new SeasonHandle();
                    break;
                case RBHandleType.Team:
                    handle = new TeamHandle();
                    break;
                case RBHandleType.Match:
                    handle = new MatchHandle();
                    break;
                case RBHandleType.Player:
                    handle = new PlayerHandle();
                    break;
                case RBHandleType.Injury:
                    handle = new InjuryHandle();
                    break;
                case RBHandleType.PlayerTransfer:
                    handle = new PlayerTransferHandle();
                    break;
                default:
                    handle = null;
                    break;
            }
            return handle;
        }
    }
}
