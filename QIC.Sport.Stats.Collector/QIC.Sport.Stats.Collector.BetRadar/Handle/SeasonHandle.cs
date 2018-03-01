using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ML.Infrastructure.Caching;
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
            CheckSetHistoryParam(param);

            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            //  首先添加联赛层级的比赛任务
            MatchParam mp = param.CopyCreateParam<MatchParam>();
            LeagueManager.AddOrUpdateParam(mp);

            if (param.CurrentUrlKey == "tableIds")
            {
                ProcessTableIdsTxt(param, txt);
                return;
            }
            else if (param.CurrentUrlKey == "teamIds")
            {
                ProcessTeamIdsTxt(param, txt);
                return;
            }

            var xml = new XmlHelper(txt);

            //  获取赛季名称
            var title = xml.GetAttributeValue("//page", "title");
            var seasonName = title.Split('>').Last();

            SeasonEntity currentSeasonEntity = new SeasonEntity();
            currentSeasonEntity.SportId = param.SportId;
            currentSeasonEntity.SeasonId = param.SeasonId;
            currentSeasonEntity.SeasonName = seasonName;

            //  获取积分数据块
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);
            if (cdata.Count == 0)
            {
                //  杯赛没有积分数据的，添加获取所有队伍Id的任务
                param = param.CopyCreateParam<SeasonParam>();
                param.SetCurrentUrlKey("teamIds");
                LeagueManager.AddOrUpdateParam(param);
                return;
            }
            var teamRankData = "";
            if (cdata.Count > 5)
            {
                teamRankData = cdata[4];
            }
            if (string.IsNullOrEmpty(teamRankData)) { return; }

            var currentRound = RegexGetStr(cdata[0], "sb-current\"><div class=\"label\">", "<");
            currentSeasonEntity.CurrentRound = currentRound;

            SeasonEntity se = SeasonEntityManager.AddOrGetCacheEntity<SeasonEntity>(param.SeasonId);
            se.CompareSetSeasonInfo(currentSeasonEntity);

            //  解析积分数据，并添加队伍任务,添加队伍积分数据，其他数据可由已经结束的比赛结果计算
            List<string> teamIdList = new List<string>();
            List<SeasonTeamRank> trList = new List<SeasonTeamRank>();
            var root = GetHtmlRoot(teamRankData);
            var tables = root.SelectNodes("//table[@class='normaltable']/tbody");
            if (tables.Count == 0)
                return;
            if (tables.Count > 1)
            {
                //  多个积分榜数据，有分组别的，添加获取所有组别Id的任务
                param = param.CopyCreateParam<SeasonParam>();
                param.SetCurrentUrlKey("tableIds");
                LeagueManager.AddOrUpdateParam(param);
                //  同时添加获取全部队伍Id的任务
                param = param.CopyCreateParam<SeasonParam>();
                param.SetCurrentUrlKey("teamIds");
                LeagueManager.AddOrUpdateParam(param);
                return;
            }
            foreach (var node in tables[0].ChildNodes)
            {
                var teamId = RegexGetStr(node.ChildNodes[2].InnerHtml, "teamid','", "',");
                teamIdList.Add(teamId);
                var trDataArr = node.ChildNodes.Select(o => o.InnerText).ToArray();

                SeasonTeamRank tr = new SeasonTeamRank()
                {
                    SeasonId = param.SeasonId,
                    TeamId = teamId,
                    TeamPoints = trDataArr[11],
                    Description = trDataArr[12]
                };
                trList.Add(tr);
            }
            SeasonTeams st = SeasonTeamsManager.AddOrGetCacheEntity<SeasonTeams>(param.SeasonId);
            st.CompareSetTeamRank(trList);


            //  要分配的队伍任务
            var teamTaskDic = se.CompareSetTeamIdList(teamIdList);
            TeamAssignTask(param, teamTaskDic);
        }

        private void ProcessTeamIdsTxt(BRBaseParam param, string txt)
        {
            var xml = new XmlHelper(txt);
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);

            //  获取赛季名称
            var title = xml.GetAttributeValue("//page", "title");
            var seasonName = title.Split('>').Last();

            SeasonEntity currentSeasonEntity = new SeasonEntity();
            currentSeasonEntity.SportId = param.SportId;
            currentSeasonEntity.SeasonId = param.SeasonId;
            currentSeasonEntity.SeasonName = seasonName;
            var se = SeasonEntityManager.AddOrGetCacheEntity<SeasonEntity>(param.SeasonId);
            se.CompareSetSeasonInfo(currentSeasonEntity);

            //  获取赛季teamIds
            var root = GetHtmlRoot(cdata[1]);
            var ul = root.SelectSingleNode("//ul[@class='slickselect']");
            if (ul == null || ul.ChildNodes == null) return;

            List<string> teamIdList = new List<string>();
            foreach (var li in ul.ChildNodes)
            {
                var teamId = RegexGetStr(li.OuterHtml, "teamid','", "',");
                teamIdList.Add(teamId);
            }
            var dic = se.CompareSetTeamIdList(teamIdList);
            TeamAssignTask(param, dic);
        }

        private void ProcessTableIdsTxt(BRBaseParam param, string txt)
        {
            var xml = new XmlHelper(txt);
            var cdataFlag = "//c";
            var cdata = xml.GetValues(cdataFlag);
            //  解析SeasonType
            var root = GetHtmlRoot(cdata[1]);
            var ul = root.SelectSingleNode("//ul[@class='navtabs gismo']");
            if (ul == null || ul.ChildNodes == null) return;

            List<string> list = new List<string>();
            foreach (var li in ul.ChildNodes)
            {
                var seasonTypeId = RegexGetStr(li.OuterHtml, "seasontypeid','", "',");
                var seasonTypeName = li.InnerText;
                if (!string.IsNullOrEmpty(seasonTypeId))
                {
                    var sti = SeasonTypeInfoManager.AddOrGetCacheEntity<SeasonTypeInfo>(param.SeasonId + "_" + seasonTypeId);
                    sti.CompareSet(new SeasonTypeInfo() { SeasonId = param.SeasonId, SeasonType = seasonTypeId, Name = seasonTypeName });
                    list.Add(seasonTypeId);
                }
            }
            var se = SeasonEntityManager.AddOrGetCacheEntity<SeasonEntity>(param.SeasonId);
            var dic = se.CompareSetSeasonTypeList(list);
            //  SeasonTypeAssignTask(param, dic);   除了type=21外的其他类型无需要的数据暂时不用请求处理

            //  解析21类型即分组信息，并添加任务
            var tableData = GetDataLikeKey(cdata, "template_cornerbox template_First");
            if (string.IsNullOrEmpty(tableData)) return;

            root = GetHtmlRoot(tableData);
            var tableIdList = new List<string>();
            var h2s = root.SelectNodes("//div[@class='multiview_wrap']/h2");
            if (h2s == null)
                return;

            foreach (var h2 in h2s)
            {
                var tableId = RegexGetStr(h2.InnerHtml, "tableid','", "',");
                tableIdList.Add(tableId);
            }
            var st = SeasonTypeInfoManager.AddOrGetCacheEntity<SeasonTypeInfo>(param.SeasonId + "_" + "21");
            dic = st.CompareSetTabelIdList(tableIdList);
            SeasonTableAssignTask(param, dic);
        }

        //  分配SeasonType任务
        public void SeasonTypeAssignTask(BRBaseParam param, Dictionary<string, List<string>> taskDic)
        {
            //  更新SeasonType任务，除了21类型，将在后面添加,因为需要tableId参数
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        SeasonTypeParam st = param.CopyCreateParam<SeasonTypeParam>();
                        st.SeasonTypeId = o;
                        if (o != "21") LeagueManager.AddOrUpdateParam(st);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        SeasonTypeParam st = param.CopyCreateParam<SeasonTypeParam>();
                        st.SeasonTypeId = o;
                        LeagueManager.RemoveParam(st);
                    });
                }
            }
        }

        //  分配SeasonTable任务
        public void SeasonTableAssignTask(BRBaseParam param, Dictionary<string, List<string>> taskDic)
        {
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        SeasonTableParam st = param.CopyCreateParam<SeasonTableParam>();
                        st.SeasonTypeId = "21";
                        st.TableId = o;
                        LeagueManager.AddOrUpdateParam(st);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        SeasonTableParam st = param.CopyCreateParam<SeasonTableParam>();
                        st.SeasonTypeId = "21";
                        st.TableId = o;
                        LeagueManager.RemoveParam(st);
                    });
                }
            }
        }

        //  分配Team任务
        private void TeamAssignTask(BRBaseParam param, Dictionary<string, List<string>> taskDic)
        {
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    kv.Value.ForEach(o =>
                    {
                        TeamParam tp = param.CopyCreateParam<TeamParam>();
                        tp.TeamId = o;
                        TeamManager.AddOrUpdateParam(tp);
                    });
                }
                else
                {
                    kv.Value.ForEach(o =>
                    {
                        TeamParam tp = param.CopyCreateParam<TeamParam>();
                        tp.TeamId = o;
                        TeamManager.RemoveParam(tp);
                    });
                }
            }
        }
    }
}
