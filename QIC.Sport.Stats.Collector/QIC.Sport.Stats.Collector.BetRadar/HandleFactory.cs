using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.Infrastructure.IOC;
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
                    handle = IocUnity.GetService<IHandle>(typeof(OrganizerHandle).Name);
                    break;
                case RBHandleType.Season:
                    handle = IocUnity.GetService<IHandle>(typeof(SeasonHandle).Name);
                    break;
                case RBHandleType.Team:
                    handle = IocUnity.GetService<IHandle>(typeof(TeamHandle).Name);
                    break;
                case RBHandleType.Match:
                    handle = IocUnity.GetService<IHandle>(typeof(MatchHandle).Name);
                    break;
                case RBHandleType.Player:
                    handle = IocUnity.GetService<IHandle>(typeof(PlayerHandle).Name);
                    break;
                case RBHandleType.Injury:
                    handle = IocUnity.GetService<IHandle>(typeof(InjuryHandle).Name);
                    break;
                case RBHandleType.PlayerTransfer:
                    handle = IocUnity.GetService<IHandle>(typeof(PlayerTransferHandle).Name);
                    break;
                default:
                    handle = null;
                    break;
            }
            return handle;
        }
    }
}
