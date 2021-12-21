namespace TodoManager.BusinessExceptions
{
    public class ItemAlreadyExistsException: BusinessException
    {
        private const string MessageFormat = "Item with title '{0}' already exists";
        
        public ItemAlreadyExistsException(string name):base(string.Format(MessageFormat, name))
        {
            
        }
    }
}