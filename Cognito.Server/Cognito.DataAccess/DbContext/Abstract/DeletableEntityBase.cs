namespace Cognito.DataAccess.DbContext.Abstract
{
    public class DeletableEntityBase : IDeletableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
