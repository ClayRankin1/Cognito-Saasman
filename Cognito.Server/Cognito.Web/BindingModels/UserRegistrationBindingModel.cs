namespace Cognito.Web.BindingModels
{
    public class UserRegistrationBindingModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DomainName { get; set; }
    }
}
