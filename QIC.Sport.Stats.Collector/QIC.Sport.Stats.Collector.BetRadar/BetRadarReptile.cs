using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Handle;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar
{
    public class BetRadarReptile : BaseReptile
    {
        public BetRadarReptile()
        {
            InitWorkManager();
            InitCacheManager();
        }

        public override void InitWorkManager()
        {
            // todo IOC生成所有Manager并加入到管理集合中

            var organizerManager = new OrganizerManager();
            IocUnity.RegisterInstance<IWorkManager>("LeagueManager", organizerManager);
            DicWorkManagers.Add(typeof(OrganizerManager).Name, (IWorkManager)organizerManager);

            var leagueManager = new LeagueManager();
            IocUnity.RegisterInstance<IWorkManager>("LeagueManager", leagueManager);
            DicWorkManagers.Add(typeof(LeagueManager).Name, (IWorkManager)leagueManager);

            var teamManager = new TeamManager();
            IocUnity.RegisterInstance<IWorkManager>("TeamManager", teamManager);
            DicWorkManagers.Add(typeof(TeamManager).Name, (IWorkManager)teamManager);

            //  起始任务添加到OrganizerManager中
            var indexUrl = "gismo.php?&html=1&id=1828&language=zh&clientid=4&child=1&ismenu=1&childnodeid=1819";
            var param = ParamFactory.CreateParam((int)RBHandleType.Organizer);
            ((OrganizerParam)param).IndexUrl = indexUrl;
            organizerManager.AddOrUpdateParam(param);
        }

        public override void InitCacheManager()
        {
            var organizerEntityManager = new OrganizerEntityManager();
            IocUnity.RegisterInstance<ICacheManager>("OrganizerEntityManager", organizerEntityManager);
            DicCacheManagers.Add(typeof(OrganizerEntityManager).Name, (ICacheManager)organizerEntityManager);

            var leagueEntityManager = new LeagueEntityManager();
            IocUnity.RegisterInstance<ICacheManager>("LeagueEntityManager", leagueEntityManager);
            DicCacheManagers.Add(typeof(LeagueEntityManager).Name, (ICacheManager)leagueEntityManager);

        }
    }
}
