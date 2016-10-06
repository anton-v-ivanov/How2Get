using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HowToGet.Web.Filters
{
    public class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        // cache length in seconds
        private readonly int _timespan;

        // client cache length in seconds
        private readonly int _clientTimeSpan;

        // cache for anonymous users only?
        private readonly bool _anonymousOnly;

        // cache key
        private string _cachekey = string.Empty;

        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        /// <summary>
        /// Set cache for WebAPI
        /// </summary>
        /// <param name="timespan">Time for server cache in seconds</param>
        /// <param name="clientTimeSpan">Time for client cache in seconds</param>
        /// <param name="anonymousOnly">Only for anonymous users</param>
        public WebApiOutputCacheAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            _timespan = timespan;
            _clientTimeSpan = clientTimeSpan;
            _anonymousOnly = anonymousOnly;
        }

		public WebApiOutputCacheAttribute(int timespan, int clientTimeSpan)
		{
			_timespan = timespan;
			_clientTimeSpan = clientTimeSpan;
			_anonymousOnly = false;
		}

	    public WebApiOutputCacheAttribute(int timespan)
	    {
		    _timespan = timespan;
			_clientTimeSpan = timespan;
			_anonymousOnly = false;
	    }

	    readonly Func<int, HttpActionContext, bool, bool> _isCachingTimeValid = (timespan, ac, anonymous) =>
		{
			if (timespan > 0)
			{
				if (anonymous)
					if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
						return false;

				if (ac.Request.Method == HttpMethod.Get) return true;
			}

			return false;
		};

		private CacheControlHeaderValue SetClientCache()
		{
			var cachecontrol = new CacheControlHeaderValue
			{
				MaxAge = TimeSpan.FromSeconds(_clientTimeSpan),
				MustRevalidate = true
			};
			return cachecontrol;
		}

	    public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                if (_isCachingTimeValid(_timespan, actionContext, _anonymousOnly))
                {
                    var mediaTypeWithQualityHeaderValue = actionContext.Request.Headers.Accept.FirstOrDefault();
                    if (mediaTypeWithQualityHeaderValue != null)
                        _cachekey = string.Join(":", new[] { actionContext.Request.RequestUri.PathAndQuery, mediaTypeWithQualityHeaderValue.ToString() });

                    if (WebApiCache.Contains(_cachekey))
                    {
                        var val = WebApiCache.Get(_cachekey) as string;

                        if (val != null)
                        {
                            var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(_cachekey + ":response-ct") ??
                                              new MediaTypeHeaderValue(_cachekey.Split(':')[1]);

                            actionContext.Response = actionContext.Request.CreateResponse();
                            actionContext.Response.Content = new StringContent(val);

                            actionContext.Response.Content.Headers.ContentType = contenttype;
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (!(WebApiCache.Contains(_cachekey)) && !string.IsNullOrWhiteSpace(_cachekey) && actionExecutedContext.Response != null)
            {
                var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                WebApiCache.Add(_cachekey, body, DateTime.Now.AddSeconds(_timespan));
                WebApiCache.Add(_cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(_timespan));
            }

            if (_isCachingTimeValid(_clientTimeSpan, actionExecutedContext.ActionContext, _anonymousOnly) && actionExecutedContext.Response != null)
                actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        }
    }
}