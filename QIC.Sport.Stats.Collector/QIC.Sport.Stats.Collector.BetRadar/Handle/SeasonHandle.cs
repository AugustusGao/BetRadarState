﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ML.Infrastructure.Caching;
using ML.Infrastructure.IOC;
using QIC.Sport.Stats.Collector.BetRadar.Manager;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Cache.CacheDataManager;
using QIC.Sport.Stats.Collector.Cache.CombinedData;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using ICacheManager = QIC.Sport.Stats.Collector.Cache.ICacheManager;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class SeasonHandle : BaseHandle, IHandle
    {
        public void Process(ITakerReptile.Dto.BaseData data)
        {
            BRData bd = data as BRData;
            SeasonParam param = bd.Param as SeasonParam;

            if (string.IsNullOrEmpty(bd.Html)) return;
            var txt = HttpUtility.HtmlDecode(bd.Html);

            var xml = new XmlHelper(txt);

            //  获取赛季名称
            var title = xml.GetAttributeValue("//page", "title");
            var seasonName = title.Split('>').Last();

            SeasonEntityManager sem = (SeasonEntityManager)IocUnity.GetService<ICacheManager>(typeof(SeasonEntityManager).Name);
            SeasonEntity se = sem.AddOrGetCacheEntity<SeasonEntity>(param.SeasonId);
            se.SeasonName = seasonName;

            //  获取积分数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);
            var html = "";
            if (cdata.Count > 5)
            {
                html = cdata[4];
            }

            //  解析积分数据，并添加队伍任务,添加队伍积分数据，其他数据可由已经结束的比赛结果计算
            List<string> teamIdList = new List<string>();
            var root = GetHtmlRoot(html);
            var table = root.SelectSingleNode("//table[@class='normaltable']/tbody");
            foreach (var node in table.ChildNodes)
            {
                var teamId = RegGetStr(node.ChildNodes[2].InnerHtml, "teamid','", "',");
                teamIdList.Add(teamId);
                var trDataArr = node.ChildNodes.Select(o => o.InnerText).ToArray();

                TeamRank tr = new TeamRank()
                {
                    SeasonId = param.SeasonId,
                    TeamId = teamId,
                    TeamPoints = trDataArr[11],
                    Description = trDataArr[12]
                };
                se.AddOrUpdateTeamRank(tr);
            }

            //  要分配的队伍任务
            var teamTaskDic = se.CompareTeamIdList(teamIdList);
            NextAssignTask(param, teamTaskDic);

            //  添加联赛层级的比赛任务
            MatchParam mp = param.CopyBaseParam<MatchParam>();
            IWorkManager sm = IocUnity.GetService<IWorkManager>(typeof(LeagueManager).Name);
            sm.AddOrUpdateParam(mp);
        }

        //  分配任务
        private void NextAssignTask(SeasonParam param, Dictionary<string, List<string>> taskDic)
        {
            IWorkManager tm = IocUnity.GetService<IWorkManager>(typeof(TeamManager).Name);
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        TeamParam tp = param.CopyBaseParam<TeamParam>();
                        tp.TeamId = o;
                        tm.AddOrUpdateParam(tp);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        TeamParam tp = param.CopyBaseParam<TeamParam>();
                        tp.TeamId = o;
                        tm.RemoveParam(tp);
                    });
                }
            }
        }
    }
}
