using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ambev.DeveloperEvaluation.ORM.Common
{
    public class MoneyConverter : ValueConverter<Money, decimal>
    {
        public MoneyConverter() : base(
            v => v.Value, 
            v => new Money(v))
        { }
    }
}
