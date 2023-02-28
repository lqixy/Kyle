using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyle.Extensions
{
    public struct ConditionalValue<TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <cref name="ConditionalValue{TValue}"/> class with the given value.
        /// </summary>
        /// <param name="hasValue">Indicates whether the value is valid.</param>
        /// <param name="value">The value.</param>
        public ConditionalValue(bool hasValue, TValue value)
        {
            HasValue = hasValue;
            Value = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current <cref name="ConditionalValue{TValue}"/> object has a valid value of its underlying type.
        /// </summary>
        /// <returns><languageKeyword>true</languageKeyword>: Value is valid, <languageKeyword>false</languageKeyword> otherwise.</returns>
        public bool HasValue { get; }

        /// <summary>
        /// Gets the value of the current <cref name="ConditionalValue{TValue}"/> object if it has been assigned a valid underlying value.
        /// </summary>
        /// <returns>The value of the object. If HasValue is <languageKeyword>false</languageKeyword>, returns the default value for type of the TValue parameter.</returns>
        public TValue Value { get; }
    }
}
