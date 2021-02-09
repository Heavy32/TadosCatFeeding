namespace TadosCatFeeding
{
    public class ServiceResult<T>
    {
        public ServiceResultStatus Status { get; }
        public string Message { get; }
        public T ReturnedObject { get; }

        public ServiceResult(ServiceResultStatus status)
        {
            Status = status;
        }

        public ServiceResult(ServiceResultStatus status, T returnedObject) : this(status)
        {
            ReturnedObject = returnedObject;
        }

        public ServiceResult(ServiceResultStatus status, string message) : this(status)
        {
            Message = message;
        }

        public ServiceResult(ServiceResultStatus status, string message, T returnedObject) : this(status, message)
        {
            ReturnedObject = returnedObject;
        }
    }
}
