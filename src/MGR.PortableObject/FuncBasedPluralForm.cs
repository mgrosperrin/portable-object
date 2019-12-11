using System;

namespace MGR.PortableObject
{
    /// <summary>
    /// Defines plural form computation based on a <see cref="Func{Int32, Int32}"/>.
    /// </summary>
    public class FuncBasedPluralForm : IPluralForm
    {
        private readonly Func<int, int> _pluralFormCompute;

        /// <summary>
        /// Creates a new instance of <see cref="FuncBasedPluralForm"/>.
        /// </summary>
        /// <param name="pluralFormCompute">The <see cref="Func{Int32, Int32}"/> to compute the plural form.</param>
        public FuncBasedPluralForm(Func<int, int> pluralFormCompute)
        {
            _pluralFormCompute = pluralFormCompute;
        }

        /// <inheritdoc />
        public int GetPluralFormForNumber(int numberOfItems) => _pluralFormCompute(numberOfItems);
    }
}
