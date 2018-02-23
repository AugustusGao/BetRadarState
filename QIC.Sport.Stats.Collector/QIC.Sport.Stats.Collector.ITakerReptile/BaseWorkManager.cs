using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using ML.NetComponent.Http;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseWorkManager : IWorkManager
    {
        protected ConcurrentDictionary<string, BaseParam> DicParam = new ConcurrentDictionary<string, BaseParam>();
        protected ILog logger;
        protected int IntervalsTime = 10;    //  测试间隔时间调整

        private bool isClosed;
        private bool isCompleted;
        private Thread workThread;

        private static readonly LimitedConcurrencyLevelTaskScheduler executerScheduler = new LimitedConcurrencyLevelTaskScheduler(10);
        private static readonly TaskFactory executerFactory = new TaskFactory(executerScheduler);
        private static readonly LimitedConcurrencyLevelTaskScheduler processerScheduler = new LimitedConcurrencyLevelTaskScheduler(2);
        private static readonly TaskFactory processerFactory = new TaskFactory(processerScheduler);
        private List<Task> executers = new List<Task>();
        private List<Task> processers = new List<Task>();

        public bool IsCompleted { get { return isCompleted; } }

        public virtual void ExecuteTask(BaseParam param) { }
        public virtual void ProcessData(BaseData data) { }

        public void Start()
        {
            logger = LogManager.GetLogger(this.GetType());
            workThread = new Thread(Work);
            workThread.Start();
            var name = this.GetType().Name;
            Console.WriteLine(name + " Start Ok!");
        }

        public void Stop()
        {
            isClosed = true;
        }

        public void AddOrUpdateParam(BaseParam baseParam)
        {
            //  对比更新
            DicParam.AddOrUpdate(baseParam.GetKey(), baseParam, (k, v) =>
            {
                v = baseParam;
                return v;
            });
        }

        public void RemoveParam(BaseParam baseParam)
        {
            //  根据key移除
            BaseParam bp;
            DicParam.TryRemove(baseParam.GetKey(), out bp);
        }
        public void EnQueueData(BaseData baseData)
        {
            processers.Add(processerFactory.StartNew(() => ProcessData(baseData)));
        }

        protected virtual string RequestPage(string url, string arg = "", string method = "GET", string cookie = "", string referer = "", int timeOut = 100000)
        {
            try
            {
                HttpHelper request = new HttpHelper();
                if (method == "GET" && !string.IsNullOrEmpty(arg)) url = url + "?" + arg;
                HttpItem item = new HttpItem()
                {
                    Method = method,
                    URL = url,
                    Postdata = arg,
                    Cookie = cookie,
                    Referer = referer,
                    Timeout = timeOut
                };
                item.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36";
                if (method == "POST") item.ContentType = "application/x-www-form-urlencoded";
                item.Header.Add("Accept-Encoding", "gzip,deflate");
                item.Encoding = Encoding.UTF8;
                HttpResult result = request.GetHtml(item);
                return result.StatusCode == System.Net.HttpStatusCode.OK ? result.Html : string.Empty;
            }
            catch (WebException webEx)
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Error(string.Format("Url = " + url));
                return string.Empty;
            }
        }


        private void Work()
        {
            while (!isClosed)
            {
                try
                {
                    isCompleted = false;

                    foreach (var kv in DicParam)
                    {
                        executers.Add(executerFactory.StartNew(() => ExecuteTask(kv.Value)));
                    }

                    Task.WaitAll(executers.ToArray());
                    Task.WaitAll(processers.ToArray());

                    executers.Clear();
                    processers.Clear();
                    isCompleted = true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                Thread.Sleep(IntervalsTime);
            }
        }
    }
}
