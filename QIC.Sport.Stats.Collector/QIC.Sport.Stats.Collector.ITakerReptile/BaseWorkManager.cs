using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using ML.Infrastructure.Config;
using ML.NetComponent.Http;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseWorkManager : IWorkManager
    {
        protected ConcurrentDictionary<string, BaseParam> DicParam = new ConcurrentDictionary<string, BaseParam>();
        protected ILog logger;
        protected int IntervalsTime = 10;    

        private bool isClosed;
        private bool isCompleted;
        private Thread workThread;

        private TaskFactory executerFactory;
        private TaskFactory processerFactory;
        private List<Task> executers = new List<Task>();
        private List<Task> processers = new List<Task>();

        public bool IsCompleted { get { return isCompleted; } }
        public int Count { get { return DicParam.Count; } }

        public virtual void ExecuteTask(BaseParam param) { }
        public virtual void ProcessData(BaseData data) { }

        public void Start()
        {
            logger = LogManager.GetLogger(this.GetType());
            InitTaskFactory();

            workThread = new Thread(Work);
            workThread.Start();
            Console.WriteLine( this.GetType().Name + " Start Ok!");
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
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    isCompleted = false;
                    foreach (var kv in DicParam)
                    {
                        executers.Add(executerFactory.StartNew(() => ExecuteTask(kv.Value)));
                    }

                    Task.WaitAll(executers.ToArray());
                    Task.WaitAll(processers.ToArray());

                    sw.Stop();

                    if (sw.Elapsed.TotalSeconds > 10)
                        logger.Debug(this.GetType().Name + " completed in total seconds = " + sw.Elapsed.TotalSeconds + "s  ,executers count = " + executers.Count() + " ,processers count = " + processers.Count());
                    //Console.WriteLine(this.GetType().Name + " completed in total seconds = " + sw.Elapsed.TotalSeconds + "s");

                    executers.Clear();
                    processers.Clear();
                    isCompleted = true;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    Thread.Sleep(IntervalsTime);
                }

                Thread.Sleep(IntervalsTime);
            }
        }

        private void InitTaskFactory()
        {
            var countStr = ConfigSingleton.CreateInstance().GetAppConfig<string>(this.GetType().Name, "3,1");
            var executerThreadCount = Convert.ToInt32(countStr.Split(',')[0]);
            var processerThreadCount = Convert.ToInt32(countStr.Split(',')[1]);
            var executerScheduler = new LimitedConcurrencyLevelTaskScheduler(executerThreadCount);
            executerFactory = new TaskFactory(executerScheduler);
            var processerScheduler = new LimitedConcurrencyLevelTaskScheduler(processerThreadCount);
            processerFactory = new TaskFactory(processerScheduler);
        }
    }
}
