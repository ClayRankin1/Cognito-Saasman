namespace Cognito.DataAccess.Entities
{
    public class UserDomain
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public int DomainId { get; set; }

        public virtual Domain Domain { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
