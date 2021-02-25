using System.ComponentModel.DataAnnotations;

namespace KurbSide.Annotations
{
    /// <summary>
    /// Checks if the <b>double</b> is less than or equal to the specified max value.
    /// </summary>
    public class KSMaxValueAttribute : ValidationAttribute
    {
        private readonly double _maxValue;

        public KSMaxValueAttribute(double maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (double)value <= _maxValue;
        }
    }
}