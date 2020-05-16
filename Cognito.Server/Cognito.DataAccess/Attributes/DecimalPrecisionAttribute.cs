using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cognito.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DecimalPrecisionAttribute : ColumnAttribute
    {
        public DecimalPrecisionAttribute(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;
            TypeName = $"decimal({precision}, {scale})";
        }

        public byte Precision { get; }

        public byte Scale { get; }
    }
}