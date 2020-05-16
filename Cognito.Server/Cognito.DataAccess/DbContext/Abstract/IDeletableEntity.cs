namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
