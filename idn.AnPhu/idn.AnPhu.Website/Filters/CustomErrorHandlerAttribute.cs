using idn.AnPhu.Utils;
using idn.AnPhu.Website.Extensions;
using System;
using System.Web.Mvc;
using System.Web.Routing;


namespace idn.AnPhu.Website.Filters
{
    public class CustomErrorHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;

            string exMsg = ex.Message;
            string exDetail = ex.StackTrace;
            string exTrace = ex.StackTrace;
            if (ex is ServiceException)
            {
                var sEx = ex as ServiceException;
                exMsg = sEx.ErrorMessage;
                exDetail = sEx.ErrorDetail;
            }

            var viewData = new ViewDataDictionary()
                {
                    {"Message", exMsg },
                    {"Detail", exDetail},
                    {"StackTrace", exTrace}

                };


            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "LogOn" }));

            var request = filterContext.RequestContext.HttpContext.Request;
            var rw = request.Headers.Get("X-Requested-With");
            filterContext.ExceptionHandled = true;


            var response = filterContext.RequestContext.HttpContext.Response;
            string message = "";
            //ajax
            if (!CUtils.IsNullOrEmpty(rw) && rw.Equals("XMLHttpRequest", StringComparison.InvariantCultureIgnoreCase))
            {
                message = HtmlExtensions.RenderPartialViewToString(filterContext.Controller.ControllerContext, "FilteredErrorAjax", null, viewData);
                response.StatusCode = 999;

            }
            //not ajax
            else
                //post to frame
                if (filterContext.RequestContext.HttpContext.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase)
                    && (request["insideframe"] ?? "").Equals("true", StringComparison.InvariantCultureIgnoreCase)

                    )
            {
                message = HtmlExtensions.RenderPartialViewToString(filterContext.Controller.ControllerContext, "FilteredErrorPost", null, viewData);
                response.StatusCode = 999;
            }

            //get
            else
            {
                message = HtmlExtensions.RenderPartialViewToString(filterContext.Controller.ControllerContext, "FilteredError", null, viewData);
                //response.StatusCode = 400;
                response.StatusCode = 200;

            }

            response.Write(message);

            response.StatusDescription = filterContext.Exception.Message;
            var check = response.IsClientConnected;
            response.BufferOutput = true;
            response.Clear();
            response.End();
        }
    }
}