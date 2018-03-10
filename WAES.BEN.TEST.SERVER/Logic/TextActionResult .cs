using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WAES.BEN.TEST.SERVER.Logic
{
    public class TextActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;

        #region ctor

        public TextActionResult(HttpRequestMessage request,string text,HttpStatusCode statusCode)
        {
            _request = request;
            Text = text;
            StatusCode = statusCode;
        }

        #endregion

        #region properties

            /// <summary>
            /// Gets the text to be sent as the respons
            /// </summary>
            public string Text { get; private set; }

            /// <summary>
            /// Gets the status code to send in the response
            /// </summary>
            public HttpStatusCode StatusCode { get; private set; }

            
        #endregion

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode=StatusCode,
                Content = new StringContent(Text),
                RequestMessage = _request
            };
            return Task.FromResult(response);

        }


    }
}