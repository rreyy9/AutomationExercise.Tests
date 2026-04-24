namespace AutomationExercise.UI.Tests.Utils
{
    /// <summary>
    /// Domain-specific assertion wrappers for readability.
    /// These are thin delegates to standard MSTest assertions — not replacements.
    /// Add methods here only when the same assertion pattern repeats across test classes.
    /// </summary>
    public static class AssertionHelpers
    {
        /// <summary>
        /// Asserts that a string field from the UI or API is not null or whitespace.
        /// Use for asserting product names, prices, labels, and other text fields
        /// that must be present but whose exact value is unknown or variable.
        /// </summary>
        public static void AssertFieldPresent(string value, string fieldName) =>
            Assert.IsFalse(
                string.IsNullOrWhiteSpace(value),
                $"Expected '{fieldName}' to be present but it was empty or whitespace.");

        /// <summary>
        /// Asserts the cart contains at least the expected minimum number of items.
        /// </summary>
        public static void AssertCartHasItems(int actualCount, int minimumExpected = 1) =>
            Assert.IsTrue(
                actualCount >= minimumExpected,
                $"Expected at least {minimumExpected} item(s) in cart but found {actualCount}.");

        /// <summary>
        /// Asserts a product count result is greater than zero — i.e. the list is not empty.
        /// Use after search, category filter, brand filter, or any operation that should return products.
        /// </summary>
        public static void AssertProductsReturned(int count, string context = "") =>
            Assert.IsTrue(
                count > 0,
                $"Expected at least one product to be returned{(string.IsNullOrEmpty(context) ? "." : $" ({context}).")}");
    }
}