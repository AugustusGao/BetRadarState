using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class TeamHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            TeamParam param = bd.Param as TeamParam;

            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  解析成各个数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  解析队伍基本信息
            var teamInfo = cdata[2];

            //  解析进球数获得队员的点球个数，更新到队员点球信息缓存中
            var penGoals = cdata[14];

            //  解析全部名单，并添加球员任务
            var playersData = cdata[16];
            var root = GetHtmlRoot(playersData);
            var trsPlayer = root.SelectNodes("//tbody/tr");
            List<string> list = new List<string>();
            foreach (var tr in trsPlayer)
            {
                var s = tr.Attributes["onclick"].Value;
                var playerId = RegGetStr(s, "playerid', '", "',");
                list.Add(playerId);
            }
            TeamPlayersManager tpmManager = (TeamPlayersManager)IocUnity.GetService<ICacheManager>(typeof(TeamPlayersManager).Name);
            var tp = tpmManager.AddOrGetCacheEntity<TeamPlayers>(param.TeamId + "_" + param.SeasonId);
            var dic = tp.ComparePlayerIdList(list);
            NextAssignTask(param, dic);

            //  如果有添加获取伤停的任务
        }

        //  分配任务
        private void NextAssignTask(TeamParam param, Dictionary<string, List<string>> taskDic)
        {
            IWorkManager tm = IocUnity.GetService<IWorkManager>(typeof(PlayerManager).Name);
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        PlayerParam pp = param.CopyBaseParam<PlayerParam>();
                        pp.PlayerId = o;
                        tm.AddOrUpdateParam(pp);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        PlayerParam pp = param.CopyBaseParam<PlayerParam>();
                        pp.PlayerId = o;
                        tm.RemoveParam(pp);
                    });
                }
            }
        }
    }


}
