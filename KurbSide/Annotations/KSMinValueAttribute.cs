using System.ComponentModel.DataAnnotations;

namespace KurbSide.Annotations
{
    /// <summary>
    /// Checks if the <b>value</b> is greater than or equal to the specified min value.
    /// </summary>
    public class KSMinValueAttribute : ValidationAttribute
    {
        private readonly double _minValue;

        public KSMinValueAttribute(double minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (decimal)value >= (decimal)_minValue;
        }
    }
}