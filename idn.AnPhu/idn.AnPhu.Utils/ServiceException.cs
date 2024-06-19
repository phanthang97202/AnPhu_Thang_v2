using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Utils
{
    public class ServiceException : Exception
    {
        public static string ERROR_CODE_CLIENT = "ERROR_CLIENT";
        //
        private string _errorMessage;
        private string _errorDetail;
        private string _errorCode;
        //
        private object _tag;
        //
        public Boolean isWarning = false;

        public string ErrorDetail
        {
            set { _errorDetail = value; }
            get { return _errorDetail; }
        }

        public string ErrorCode
        {
            set { _errorCode = value; }
            get { return _errorCode; }
        }

        public string ErrorMessage
        {
            set { _errorMessage = value; }
            get { return _errorMessage; }
        }

        public object Tag
        {
            set { _tag = value; }
            get { return _tag; }
        }

        public bool HasError()
        {
            return (this.ErrorCode != null && this.ErrorCode.Length > 0);
        }
    }
}
