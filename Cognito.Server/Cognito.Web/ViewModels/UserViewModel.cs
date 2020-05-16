namespace Cognito.Web.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? DomainId { get; set; }

        public int? ContactId { get; set; }

        public int? SecurityLevelId { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
