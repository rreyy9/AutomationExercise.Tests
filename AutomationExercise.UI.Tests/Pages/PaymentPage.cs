namespace AutomationExercise.UI.Tests.Pages
{
    public class PaymentPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ──────────────────────────────────────────────────────────────
        // ✅ Payment page uses data-qa attributes throughout — prefer these
        private readonly ILocator _nameOnCard = page.Locator("input[data-qa='name-on-card']");
        private readonly ILocator _cardNumber = page.Locator("input[data-qa='card-number']");
        private readonly ILocator _cvc = page.Locator("input[data-qa='cvc']");
        private readonly ILocator _expiryMonth = page.Locator("input[data-qa='expiry-month']");
        private readonly ILocator _expiryYear = page.Locator("input[data-qa='expiry-year']");
        private readonly ILocator _payButton = page.Locator("button[data-qa='pay-button']");

        // Post-payment
        private readonly ILocator _successMessage = page.Locator("p:has-text('Congratulations')");
        private readonly ILocator _downloadInvoice = page.Locator("a.btn:has-text('Download Invoice')");
        private readonly ILocator _continueButton = page.Locator("a[data-qa='continue-button']");

        // ── Payment actions ───────────────────────────────────────────────────────
        public async Task EnterCardDetailsAsync(CardDetails card)
        {
            await _nameOnCard.FillAsync(card.NameOnCard);
            await _cardNumber.FillAsync(card.CardNumber);
            await _cvc.FillAsync(card.Cvc);
            await _expiryMonth.FillAsync(card.ExpiryMonth);
            await _expiryYear.FillAsync(card.ExpiryYear);
        }

        public async Task ConfirmPaymentAsync()
            => await _payButton.ClickAsync();

        /// <summary>
        /// Convenience: fills card details and submits payment in one call.
        /// </summary>
        public async Task EnterCardDetailsAndConfirmAsync(CardDetails card)
        {
            await EnterCardDetailsAsync(card);
            await ConfirmPaymentAsync();
        }

        // ── Post-payment actions ──────────────────────────────────────────────────
        public async Task DownloadInvoiceAsync()
        {
            // Playwright will intercept the download — caller should use
            // context.WaitForDownloadAsync() if they need to capture the file path.
            await _downloadInvoice.ClickAsync();
        }

        public async Task ClickContinueAsync()
            => await _continueButton.ClickAsync();

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<string> GetSuccessMessageAsync()
            => await _successMessage.InnerTextAsync();

        public async Task<bool> IsSuccessMessageVisibleAsync()
            => await _successMessage.IsVisibleAsync();

        public async Task<bool> IsDownloadInvoiceButtonVisibleAsync()
            => await _downloadInvoice.IsVisibleAsync();
    }
}