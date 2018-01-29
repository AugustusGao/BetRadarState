using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QIC.Sport.Stats.Collector.Common;
using QIC.Sport.Stats.Collector.ITakerReptile.Dto;

namespace QIC.Sport.Stats.Collector.ITakerReptile
{
    public class BaseWorkManager : IWorkManager
    {
        protected ConcurrentDictionary<string, BaseParam> DicParam = new ConcurrentDictionary<string, BaseParam>();

        private bool isClosed;
        private Thread workThread;
        private bool isCompleted;

        private static readonly LimitedConcurrencyLevelTaskScheduler executerScheduler = new LimitedConcurrencyLevelTaskScheduler(2);
        private static readonly TaskFactory executerFactory = new TaskFactory(executerScheduler);
        private static readonly LimitedConcurrencyLevelTaskScheduler processerScheduler = new LimitedConcurrencyLevelTaskScheduler(2);
        private static readonly TaskFactory processerFactory = new TaskFactory(processerScheduler);
        private List<Task> executers;
        private List<Task> processers;

        public bool IsCompleted { get { return isCompleted; } }

        public virtual void ExecuteTask(BaseParam param) { }
        public virtual void ProcessData(BaseData data) { }

        public void Start()
        {
            workThread = new Thread(Work);
        }

        public void Stop()
        {

        }
        public void AddOrUpdateParam()
        {
            throw new NotImplementedException();
        }

        public void RemoveParam()
        {
            throw new NotImplementedException();
        }

        public void EnQueueData(BaseData baseData)
        {
            processers.Add(processerFactory.StartNew(() => ProcessData(baseData)));
        }

        private void Work()
        {
            while (!isClosed)
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
        }
    }
}
