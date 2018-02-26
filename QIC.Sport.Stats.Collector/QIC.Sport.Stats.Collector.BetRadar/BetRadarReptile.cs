using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
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
            InitHandle();
        }
        public override void InitWorkManager()
        {
            // todo IOC生成所有Manager并加入到管理集合中

            var organizerManager = new OrganizerManager();
            IocUnity.RegisterInstance<IWorkManager>(typeof(OrganizerManager).Name, organizerManager);
            DicWorkManagers.Add(typeof(OrganizerManager).Name, (IWorkManager)organizerManager);

            var leagueManager = new LeagueManager();
            IocUnity.RegisterInstance<IWorkManager>(typeof(LeagueManager).Name, leagueManager);
            DicWorkManagers.Add(typeof(LeagueManager).Name, (IWorkManager)leagueManager);

            var teamManager = new TeamManager();
            IocUnity.RegisterInstance<IWorkManager>(typeof(TeamManager).Name, teamManager);
            DicWorkManagers.Add(typeof(TeamManager).Name, (IWorkManager)teamManager);

            var playerManager = new PlayerManager();
            IocUnity.RegisterInstance<IWorkManager>(typeof(PlayerManager).Name, playerManager);
            DicWorkManagers.Add(typeof(PlayerManager).Name, (IWorkManager)playerManager);

            //  起始任务添加到OrganizerManager中
            var indexUrl = "gismo.php?&html=1&id=1828&language=zh&clientid=4&child=1&ismenu=1&childnodeid=1819";
            var param = new OrganizerParam() { HandleType = (int)RBHandleType.Organizer, IndexUrl = indexUrl };
            organizerManager.AddOrUpdateParam(param);
        }

        public override void InitCacheManager()
        {
            var organizerEntityManager = new OrganizerEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(OrganizerEntityManager).Name, organizerEntityManager);
            DicCacheManagers.Add(typeof(OrganizerEntityManager).Name, (ICacheManager)organizerEntityManager);

            var leagueEntityManager = new LeagueEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(LeagueEntityManager).Name, leagueEntityManager);
            DicCacheManagers.Add(typeof(LeagueEntityManager).Name, (ICacheManager)leagueEntityManager);

            var matchEntityManager = new MatchEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(MatchEntityManager).Name, matchEntityManager);
            DicCacheManagers.Add(typeof(MatchEntityManager).Name, (ICacheManager)matchEntityManager);

            var playerEntityManager = new PlayerEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(PlayerEntityManager).Name, playerEntityManager);
            DicCacheManagers.Add(typeof(PlayerEntityManager).Name, (ICacheManager)playerEntityManager);

            var playerPenaltiesManager = new PlayerPenaltiesManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(PlayerPenaltiesManager).Name, playerPenaltiesManager);
            DicCacheManagers.Add(typeof(PlayerPenaltiesManager).Name, (ICacheManager)playerPenaltiesManager);

            var PlayerStatisticsRecordManager = new PlayerStatisticsRecordManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(PlayerStatisticsRecordManager).Name, PlayerStatisticsRecordManager);
            DicCacheManagers.Add(typeof(PlayerStatisticsRecordManager).Name, (ICacheManager)PlayerStatisticsRecordManager);

            var playerTimeRecordManager = new PlayerTimeRecordManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(PlayerTimeRecordManager).Name, playerTimeRecordManager);
            DicCacheManagers.Add(typeof(PlayerTimeRecordManager).Name, (ICacheManager)playerTimeRecordManager);

            var seasonEntityManager = new SeasonEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(SeasonEntityManager).Name, seasonEntityManager);
            DicCacheManagers.Add(typeof(SeasonEntityManager).Name, (ICacheManager)seasonEntityManager);

            var seasonTeamsManager = new SeasonTeamsManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(SeasonTeamsManager).Name, seasonTeamsManager);
            DicCacheManagers.Add(typeof(SeasonTeamsManager).Name, (ICacheManager)seasonTeamsManager);

            var teamEntityManager = new TeamEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(TeamEntityManager).Name, teamEntityManager);
            DicCacheManagers.Add(typeof(TeamEntityManager).Name, (ICacheManager)teamEntityManager);

            var teamPlayersManager = new TeamPlayersManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(TeamPlayersManager).Name, teamPlayersManager);
            DicCacheManagers.Add(typeof(TeamPlayersManager).Name, (ICacheManager)teamPlayersManager);
        }
        private void InitHandle()
        {
            IocUnity.RegisterType<IHandle, OrganizerHandle>(typeof(OrganizerHandle).Name);
            IocUnity.RegisterType<IHandle, SeasonHandle>(typeof(SeasonHandle).Name);
            IocUnity.RegisterType<IHandle, TeamHandle>(typeof(TeamHandle).Name);
            IocUnity.RegisterType<IHandle, PlayerHandle>(typeof(PlayerHandle).Name);
            IocUnity.RegisterType<IHandle, MatchHandle>(typeof(MatchHandle).Name);
            IocUnity.RegisterType<IHandle, InjuryHandle>(typeof(InjuryHandle).Name);
            IocUnity.RegisterType<IHandle, PlayerTransferHandle>(typeof(PlayerTransferHandle).Name);

            //  初始化拦截异常处理通过AOP形式到Unity容器中
            IocUnity.GetUnityContainer().AddNewExtension<Interception>().Configure<Interception>()
            .SetDefaultInterceptorFor<IHandle>(new InterfaceInterceptor());
        }
    }
}
