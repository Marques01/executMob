namespace Domain.CostumerExceptions
{
    public class ValidationException(List<string> errorMessages) : Exception
    {
        public List<string> ErrorMessages { get; } = errorMessages;
    }

}
