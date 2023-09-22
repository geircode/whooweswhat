namespace WhoOwesWhat.Domain.Exceptions
{
    public class PostNotFoundException : BaseException
    {
        public PostNotFoundException()
        {
           
        }

        public PostNotFoundException(string message)
        {
            CustomErrorMessage = message;
        }
    }
}