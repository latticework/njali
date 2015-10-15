using System;
using System.Threading.Tasks;
using System.Web;
using Jali.Serve.Definition;
using Nito.AsyncEx;

namespace Jali.Serve.AspNet.Mvc
{
    // http://stackoverflow.com/questions/9225420/using-task-or-async-await-in-ihttpasynchandler
    public class JaliMvcHttpHandler: IHttpAsyncHandler
    {
        public JaliMvcHttpHandler(Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            // TODO: JaliMvcHttpHandler.ctor: Assign IsReusable to reasoned value.
            this.IsReusable = false;

            this.Service = service;
        }

        public Service Service { get; }

        public void ProcessRequest(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            this.EndProcessRequest(this.BeginProcessRequest(context, null, null));
        }

        public async Task ProcessRequestAsync(HttpContextBase context)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public bool IsReusable { get; }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            var task = ProcessRequestAsync(new HttpContextWrapper(context));
            return AsyncFactory.ToBegin(task, cb, extraData);
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            AsyncFactory.ToEnd(result);
        }
    }
}
