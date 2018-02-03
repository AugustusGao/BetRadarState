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

            var teamEntityManager = new TeamEntityManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(TeamEntityManager).Name, teamEntityManager);
            DicCacheManagers.Add(typeof(TeamEntityManager).Name, (ICacheManager)teamEntityManager);

            var teamPlayersManager = new TeamPlayersManager();
            IocUnity.RegisterInstance<ICacheManager>(typeof(TeamPlayersManager).Name, teamPlayersManager);
            DicCacheManagers.Add(typeof(TeamPlayersManager).Name, (ICacheManager)teamPlayersManager);
        }
    }
}
