using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using HtmlAgilityPack;
using QIC.Sport.Stats.Collector.BetRadar.Param;
using QIC.Sport.Stats.Collector.Cache;
using QIC.Sport.Stats.Collector.Cache.CacheData;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.BetRadar.Handle
{
    public class OrganizerHandle : BaseHandle, IHandle
    {
        public void Process(BaseData data)
        {
            BRData bd = data as BRData;
            string txt;
            if (!HtmlDecode(bd.Html, out txt)) return;

            var xml = new XmlHelper(txt);
            var cdataFlag = "//c";
            var cdata = xml.GetValue(cdataFlag);
            if (string.IsNullOrEmpty(cdata)) return;

            var root = GetHtmlRoot(cdata);

            var jd2 = root.SelectSingleNode("//li[@class='topelem sport-1']/ul[@class=' jdlvl_2']");

            List<HtmlNode> needNodes = new List<HtmlNode>();
            foreach (var c2 in jd2.ChildNodes)
            {
                needNodes.Clear();
                var cs = c2.ChildNodes[0];
                var cc = cs.Attributes["href"];
                if (cc == null)
                {
                    // 世界
                    continue;
                }
                //  大洲的信息
                var continentId = RegexGetStr(cs.Attributes["href"].Value, "22_", "',");
                var continentName = cs.InnerText;

                var jd3 = c2.SelectSingleNode("ul[contains(@class,'jdlvl_3')]");
                if (jd3 == null) continue;

                #region 国际，俱乐部
                //  处理国际和俱乐部组织的联赛
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>() { { "4", new List<string>() }, { "393", new List<string>() } };
                for (int i = 0; i < jd3.ChildNodes.Count; i++)
                {
                    if (i % 3 == 2)
                    {

                        //  第二列国际
                        var n2 = jd3.ChildNodes[i - 1];
                        //  第三列俱乐部
                        var n3 = jd3.ChildNodes[i];

                        OrganizerEntity oe;
                        if (i == 2) //  标题行的处理
                        {
                            var key = "393";
                            oe = OrganizerEntityManager.AddOrGetCacheEntity<OrganizerEntity>(key);
                            oe.OrganizerId = key;
                            oe.OrganizerName = n3.ChildNodes[0].InnerText;
                            oe.ContinentId = continentId;
                            oe.ContinentName = continentName;

                            key = "4";
                            oe = OrganizerEntityManager.AddOrGetCacheEntity<OrganizerEntity>(key);
                            oe.OrganizerId = key;
                            oe.OrganizerName = n2.ChildNodes[0].InnerText;
                            oe.ContinentId = continentId;
                            oe.ContinentName = continentName;
                            continue;
                        }

                        //  各行数据的处理
                        //  解析第二列所有联赛Id
                        var a2 = n2.ChildNodes[0].Attributes["href"];
                        if (a2 != null)
                        {
                            var seasonId = RegexGetStr(a2.Value, "seasonid','", "',");
                            if (string.IsNullOrEmpty(seasonId))
                            {
                                continue;
                            }
                            LeagueEntity le = LeagueEntityManager.AddOrGetCacheEntity<LeagueEntity>(n2.InnerText);
                            le.LeagueName = n2.InnerText;
                            le.AddSeasonId(seasonId, true);
                            if (!string.IsNullOrEmpty(seasonId))
                            {
                                dic["4"].Add(seasonId);
                            }
                        }

                        //  解析第三列所有联赛Id
                        var a3 = n3.ChildNodes[0].Attributes["href"];
                        if (a3 != null)
                        {
                            var seasonId = RegexGetStr(a3.Value, "seasonid','", "',");
                            if (string.IsNullOrEmpty(seasonId))
                            {
                                continue;
                            }
                            LeagueEntity le = LeagueEntityManager.AddOrGetCacheEntity<LeagueEntity>(n3.InnerText);
                            le.LeagueName = n3.InnerText;
                            le.AddSeasonId(seasonId, true);
                            if (!string.IsNullOrEmpty(seasonId))
                            {
                                dic["393"].Add(seasonId);
                            }
                        }
                    }
                }

                foreach (var kv in dic)
                {
                    var oe = OrganizerEntityManager.AddOrGetCacheEntity<OrganizerEntity>(kv.Key);
                    var cpDic = oe.CompareSetSeasonIds(kv.Value);
                    NextAssignTask(oe, cpDic);
                }
                #endregion

                #region 所有其他国家的数据
                var ff3 = jd3.SelectNodes("li[@class='floater ']"); //  根节点中显示的组织数据，需要进一步循环获取组织数据
                if (ff3 != null)
                {
                    foreach (var l3 in ff3)
                    {
                        var jd4 = l3.SelectSingleNode("ul[@class=' jdlvl_4']");
                        if (jd4 == null)
                        {
                            //  其他组织的联赛上一步已经处理
                            continue;
                        }
                        needNodes.AddRange(jd4.ChildNodes);
                    }
                }
                // 需要进一步处理的Organizers数据
                foreach (var node in needNodes)
                {
                    if (node.ChildNodes.Count < 3)
                    {
                        //  世界
                        continue;
                    }
                    var a = node.ChildNodes[0];
                    var organizerId = RegexGetStr(a.Attributes["href"].Value, "3_", ",");
                    var organizerName = a.InnerText;
                    OrganizerEntity ent = OrganizerEntityManager.AddOrGetCacheEntity<OrganizerEntity>(organizerId);
                    ent.ContinentId = continentId;
                    ent.ContinentName = continentName;
                    ent.OrganizerId = organizerId;
                    ent.OrganizerName = organizerName;

                    var uls = node.ChildNodes[2].ChildNodes;

                    var list = new List<string>();
                    foreach (var ul in uls)
                    {
                        var s = ul.ChildNodes[0].Attributes["href"].Value;
                        var seasonId = RegexGetStr(s, "seasonid','", "',");
                        if (string.IsNullOrEmpty(seasonId))
                        {
                            // 业余联赛
                            continue;
                        }
                        LeagueEntity le = LeagueEntityManager.AddOrGetCacheEntity<LeagueEntity>(ul.InnerText);
                        le.LeagueName = ul.InnerText;
                        le.AddSeasonId(seasonId, true);
                        list.Add(seasonId);
                    }
                    var cpDic = ent.CompareSetSeasonIds(list);
                    NextAssignTask(ent, cpDic);
                }
                #endregion
            }
        }

        private void NextAssignTask(OrganizerEntity entity, Dictionary<string, List<string>> taskDic)
        {
            foreach (var kv in taskDic)
            {
                if (kv.Key == "add")
                {
                    foreach (var sid in kv.Value)
                    {
                        SeasonParam sp = new SeasonParam()
                        {
                            SeasonId = sid,
                            SportId = entity.SportId,
                            ContinentId = entity.ContinentId,
                            OrganizerId = entity.OrganizerId
                        };

                        #region 测试只加入指定的联赛任务
                        //一般联赛有积分的情况
                        //if (sid == "42556")

                        //杯赛无积分的情况
                        //if (sid == "42240")

                        //世锦赛分组积分的情况
                        //if (sid == "10876")

                        //其他指定测试
                        //if(sid=="44070")
                        #endregion

                        LeagueManager.AddOrUpdateParam(sp);
                    }
                }
                else if (kv.Key == "del")
                {
                    foreach (var sid in kv.Value)
                    {
                        SeasonParam sp = new SeasonParam()
                        {
                            SeasonId = sid,
                            SportId = entity.SportId,
                            ContinentId = entity.ContinentId,
                            OrganizerId = entity.OrganizerId
                        };
                        LeagueManager.RemoveParam(sp);
                    }
                }
            }
        }
    }
}
