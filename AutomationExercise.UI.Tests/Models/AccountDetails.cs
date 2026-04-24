namespace AutomationExercise.UI.Tests.Models
{
    /// <summary>
    /// Data container for the SignupPage account details form.
    /// Use Bogus to generate realistic values in tests — never hardcode.
    /// </summary>
    public record AccountDetails
    {
        public required string Title { get; init; }        // "Mr" or "Mrs"
        public required string Password { get; init; }
        public required string DateOfBirthDay { get; init; }   // e.g. "15"
        public required string DateOfBirthMonth { get; init; } // e.g. "March"
        public required string DateOfBirthYear { get; init; }  // e.g. "1990"
        public bool SignUpForNewsletter { get; init; }
        public bool ReceiveSpecialOffers { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string Company { get; init; } = string.Empty;
        public required string Address1 { get; init; }
        public string Address2 { get; init; } = string.Empty;
        public required string Country { get; init; }     // must match <option> text
        public required string State { get; init; }
        public required string City { get; init; }
        public required string Zipcode { get; init; }
        public required string MobileNumber { get; init; }
    }

    /// <summary>
    /// Data container for the PaymentPage card details form.
    /// Always use dummy / test card values — never real card data.
    /// </summary>
    public record CardDetails
    {
        public required string NameOnCard { get; init; }
        public required string CardNumber { get; init; }
        public required string Cvc { get; init; }
        public required string ExpiryMonth { get; init; }  // e.g. "12"
        public required string ExpiryYear { get; init; }   // e.g. "2027"
    }
}