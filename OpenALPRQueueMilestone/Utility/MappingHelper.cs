using System;
using System.Linq;

namespace OpenALPRQueueConsumer.Utility
{
    public class MappingHelper
    {
        /// <summary>
        /// Reurn oject
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Object mapper
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public MappingHelper(object source, object destination)
        {
            Type sourceType = source.GetType();
            Type destinationType = destination.GetType();

            var sourceProperties = sourceType.GetProperties().Select(sp => new { prop = sp, underlying = Nullable.GetUnderlyingType(sp.PropertyType) ?? sp.PropertyType });
            var destionationProperties = destinationType.GetProperties().Select(dp => new { prop = dp, underlying = Nullable.GetUnderlyingType(dp.PropertyType) ?? dp.PropertyType }); ;

            var commonProperties = from sp in sourceProperties
                                   join dp in destionationProperties on new { sp.prop.Name, sp.underlying } equals
                                       new { dp.prop.Name, dp.underlying }
                                   select new { sp = sp.prop, dp = dp.prop };

            var list = commonProperties.ToList();

            foreach (var match in commonProperties)
            {
                match.dp.SetValue(destination, match.sp.GetValue(source, null), null);
            }

            Value = destination;
        }
    }
}
