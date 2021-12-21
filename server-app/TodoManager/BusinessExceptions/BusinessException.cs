using System;

namespace TodoManager.BusinessExceptions
{
    public class BusinessException: Exception
    {
        protected BusinessException(string message):base(message)
        {
            
        }
    }
}