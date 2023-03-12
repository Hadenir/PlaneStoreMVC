namespace PlaneStore.Application.Utilities
{
    public class ServiceException : ApplicationException
    {
        public ServiceException(string message) : base(message)
        { }
    }
}
