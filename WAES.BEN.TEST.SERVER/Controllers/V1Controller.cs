using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WAES.BEN.TEST.SERVER.Logic;
using WAES.BEN.TEST.SERVER.Models;
using WAES.BEN.TEST.SERVER.Services;

namespace WAES.BEN.TEST.SERVER.Controllers
{
    public class V1Controller : ApiController
    {
        private static readonly IStringComparerService _base64ComparerService = new Base64StringComparerService();
        private static readonly ComparisonProcessor _processor = new ComparisonProcessor(_base64ComparerService);


        //http://localhost:60238/V1/diff/test1/right
        [HttpPost]
        public IHttpActionResult Diff(string id, string side, [FromBody]ComparisonRequestModel compareRequest)
        {
            var comparisonSide = ComparisonSide.Left;
            var isValidSide = Enum.TryParse(side, true, out comparisonSide);
            var isValidId = !string.IsNullOrEmpty(id);

            if (compareRequest == null)
            {
                return new TextActionResult(Request, "No text to compare", HttpStatusCode.NotAcceptable);
            }
            if (!isValidId)
            {
                return new TextActionResult(Request, "Id cannot be null or empty string", HttpStatusCode.NotAcceptable);
            }
            if (!isValidSide)
            {
                return new TextActionResult(Request, string.Format("{0} is an invalid side: must be 'right' or 'left' case insensetive", side), HttpStatusCode.NotAcceptable);
            }
            string message;
            if (_processor.LoadString(id, comparisonSide, compareRequest.Text, out message))
            {
                return new TextActionResult(Request, "string loaded succefully",HttpStatusCode.OK);
            }
            else
            {
                return new TextActionResult(Request,message,HttpStatusCode.NotAcceptable);
            }
        }

        public IHttpActionResult Diff(string id)
        {
            
            try
            {
                var comparionResulModel = _processor.Compare(id);
                var jsonText = JsonConvert.SerializeObject(comparionResulModel);
                return new TextActionResult(Request, jsonText,HttpStatusCode.OK);
            }
            catch(ArgumentException ex)
            {
                //This is for invalid arguments such as id which does not exist in repository
                return new TextActionResult(Request, ex.Message, HttpStatusCode.NotAcceptable);
            }
            catch(Exception ex)
            {
                //this is for a genral error message
                return new TextActionResult(Request,ex.Message,HttpStatusCode.InternalServerError);
            }
        }
    }
}
