using Cognito.DataAccess.Attributes;
using Cognito.DataAccess.DbContext.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cognito.DataAccess.Entities
{
    public class AccruedTime : EntityBase
    {
        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public int TaskId { get; set; }

        public virtual ProjectTask Task { get; set; }

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        [DecimalPrecision(10, 2)]
        public decimal Total { get; set; }
    }
}
