namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface IEntity : IDeletableEntity
    {
        int Id { get; set; }
    }
}
