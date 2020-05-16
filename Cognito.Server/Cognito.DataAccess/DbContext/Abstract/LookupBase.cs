using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.DbContext.Abstract
{
    public interface ILookupBase
    {
        int Id { get; set; }

        string Label { get; set; }
    }

    public interface ILookupBase<TId> : ILookupBase
    {
        new TId Id { get; set; }
    }

    public abstract class LookupBase<TId>
    {
        public virtual TId Id { get; set; }

        [Required]
        [MaxLength(64)]
        public virtual string Label { get; set; }
    }
}
