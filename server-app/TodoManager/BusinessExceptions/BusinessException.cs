using System;

namespace TodoManager.BusinessExceptions
{
    public class BusinessException: Exception
    {
        protected BusinessException(string message, int errorCode):base(message)
        {
            ErrorCode = errorCode;
        }
        
        public int ErrorCode { get;}
    }
}