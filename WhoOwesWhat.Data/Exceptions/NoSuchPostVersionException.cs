
namespace WhoOwesWhat.Domain.Exceptions
{
    public class NoSuchPostVersionException : BaseException
    {
        public NoSuchPostVersionException()
        {

        }

        public NoSuchPostVersionException(string message)
        {
            CustomErrorMessage = message;
        }
    }


}