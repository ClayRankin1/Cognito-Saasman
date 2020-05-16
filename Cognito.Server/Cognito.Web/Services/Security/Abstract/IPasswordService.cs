namespace Cognito.Web.Services.Security.Abstract
{
    public interface IPasswordService
    {
        string GetRandomPassword(int length = 15);
    }
}
