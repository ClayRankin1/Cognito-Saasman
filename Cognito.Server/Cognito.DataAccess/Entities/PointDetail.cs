namespace Cognito.DataAccess.Entities
{
    public class PointDetail
    {
        public int PointId { get; set; }

        public virtual Point Point { get; set; }

        public int DetailId { get; set; }

        public virtual Detail Detail { get; set; }
    }
}
