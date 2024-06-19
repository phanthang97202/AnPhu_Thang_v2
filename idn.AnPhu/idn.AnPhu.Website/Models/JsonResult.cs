using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;

namespace idn.AnPhu.Website.Models
{
    public class FieldError
    {
        public string FieldName { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class JsonResultEntry
    {
        public JsonResultEntry()
        {
            this.Success = true;
            Messages = new string[0];
            FieldErrors = new FieldError[0];
        }

        public JsonResultEntry(ModelStateDictionary modelState)
            : this()
        {
            this.AddModelState(modelState);
        }

        public JsonResultEntry SetFailed()
        {
            this.Success = false;
            return this;
        }

        public JsonResultEntry SetSuccess()
        {
            this.Success = true;
            return this;
        }

        public JsonResultEntry AddModelState(ModelStateDictionary modelState)
        {
            foreach (var ms in modelState)
            {
                foreach (var err in ms.Value.Errors)
                {
                    this.AddFieldError(ms.Key, err.ErrorMessage);
                }
            }

            return this;
        }

        public bool Success { get; set; }
        public string[] Messages { get; set; }
        public object Model { get; set; }
        public string RedirectUrl { get; set; }
        private bool redirectToOpener = false;
        public bool RedirectToOpener { get { return redirectToOpener; } set { redirectToOpener = value; } }
        public FieldError[] FieldErrors { get; set; }
        public bool IsServiceException { get; set; }
        public string Detail { get; set; }


        public JsonResultEntry AddFieldError(string fieldName, string message)
        {
            Success = false;
            FieldErrors = FieldErrors.Concat(new[] { new FieldError() { FieldName = fieldName, ErrorMessage = message } }).ToArray();
            return this;
        }
        public JsonResultEntry AddMessage(string message)
        {
            Messages = Messages.Concat(new[] { message }).ToArray();
            return this;
        }
        public JsonResultEntry AddException(BaseController context, Exception ex)
        {
            this.Success = false;
            if (ex is ServiceException)
            {
                this.IsServiceException = true;
                this.Detail = context.RenderRazorViewToString("ServiceException", ex);

            }
            else
            {
                //this.IsServiceException = false;
                this.IsServiceException = true;
                this.Detail = ex.Message + @"<br />" + ex.StackTrace; ;

            }
            //return AddMessage(e.Message);
            //return AddDetail(ShowException(ex));
            return AddDetail(this.Detail);
        }

        public JsonResultEntry AddDetail(string detail)
        {
            var detailCur = (!CUtils.IsNullOrEmpty(detail)) ? detail : "Error";
            Detail = detailCur;
            return this;
        }

        public string ShowException(ServiceException sex)
        {
            var sb = new StringBuilder();
            var errorMessage = "";
            var errorCode = "";
            var errorDetail = "";
            //if (sex != null)
            //{
            //    if (sex.ErrorMessage != null && sex.ErrorMessage.Trim().Length > 0)
            //    {
            //        errorMessage = sex.ErrorMessage.Trim();
            //    }
            //    if (sex.ErrorCode != null && sex.ErrorCode.Trim().Length > 0)
            //    {
            //        errorCode = sex.ErrorCode.Trim();
            //    }
            //    else
            //    {
            //        errorCode = errorMessage;
            //    }
            //    sb.Append(string.Format("Error Code: {0}", errorCode));
            //    sb.Append("<br/>--------------------------------------------------------<br/>");
            //    if (sex.InputParams != null)
            //    {
            //        foreach (var p in sex.InputParams)
            //        {
            //            if (!CUtils.IsNullOrEmpty(p.Key))
            //            {
            //                sb.Append(p.Key + "=");
            //                if (!CUtils.IsNullOrEmpty(p.Value))
            //                {
            //                    sb.Append(p.Value.ToString().Replace("\n", "<br/>"));
            //                }
            //                else
            //                {
            //                    sb.Append("");
            //                }
            //                sb.Append("<br/>--------------------------------------------------------<br/>");
            //            }

            //        }
            //    }

            //    sb.Append("<br/>====================Details===========================<br/>");
            //    if (sex.ErrorDetail != null)
            //    {
            //        errorDetail = sex.ErrorDetail.Replace("\n", "<br/>");
            //    }
            //    sb.Append(errorDetail);
            //    sb.Append("<br/>=======================================================");

            //    if (sex.FieldErrors != null && sex.FieldErrors.Count > 0)
            //    {
            //        foreach (var fieldError in sex.FieldErrors)
            //        {
            //            sb.Append(fieldError.Key + "=");
            //            if (!CUtils.IsNullOrEmpty(fieldError.Value))
            //            {
            //                sb.Append(fieldError.Value.ToString().Replace("\n", "<br/>"));
            //            }
            //            else
            //            {
            //                sb.Append("");
            //            }
            //            sb.Append("<br/>--------------------------------------------------------<br/>");
            //        }
            //    }

            //}
            var error = sb.ToString();
            var strError = "<div style=\"width: 100%; padding: 5px 0 10px 0; float: left;\"><span style=\"font: 700 17px/20px arial; color: #e00;\">ErrorMessage: " + errorMessage + "</span></div>";
            strError += "<div class=\"error_detail_panel\"  style=\"width: 100%; float: left; border-top:solid 1px #efefef;\">";
            strError += "<p style=\"font: 500 12px/20px arial;\">" + error + "</p>";
            strError += "</div>";
            return strError;
        }
    }
}