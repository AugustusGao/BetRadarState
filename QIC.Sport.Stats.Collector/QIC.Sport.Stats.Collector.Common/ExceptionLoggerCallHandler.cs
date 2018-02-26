using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Newtonsoft.Json;

namespace QIC.Sport.Stats.Collector.Common
{
    public class ExceptionLoggerCallHandler : ICallHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ExceptionLoggerCallHandler));
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("\tParameters:");
                for (int i = 0; i < input.Arguments.Count; i++)
                {
                    var parameter = JsonConvert.SerializeObject(input.Arguments[i]);
                    sb.AppendFormat("\t\tParam{0} -> {1}", i, parameter.ToString());
                }
                sb.AppendLine(result.Exception.ToString());
                logger.Error(sb.ToString());
                result.Exception = null;
            }
            return result;
        }

        public int Order { get; set; }
    }
    public class ExceptionHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new ExceptionLoggerCallHandler() { Order = this.Order };
        }
    }
}
