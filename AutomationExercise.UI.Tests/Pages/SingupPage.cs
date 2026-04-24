namespace AutomationExercise.UI.Tests.Pages
{
    public class SignupPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ─────────────────────────────────────────────────────────────
        private readonly ILocator _heading = page.Locator("h2.title b");

        // Title radio buttons
        private readonly ILocator _titleMr = page.Locator("input#id_gender1");
        private readonly ILocator _titleMrs = page.Locator("input#id_gender2");

        // Pre-filled / locked fields
        private readonly ILocator _nameField = page.Locator("input#name");
        private readonly ILocator _emailField = page.Locator("input#email");

        // Account credentials
        private readonly ILocator _passwordField = page.Locator("input#password");

        // Date of birth dropdowns
        private readonly ILocator _dobDay = page.Locator("select#days");
        private readonly ILocator _dobMonth = page.Locator("select#months");
        private readonly ILocator _dobYear = page.Locator("select#years");

        // Opt-in checkboxes
        private readonly ILocator _newsletterChk = page.Locator("input#newsletter");
        private readonly ILocator _optinChk = page.Locator("input#optin");

        // Address / personal details
        private readonly ILocator _firstName = page.Locator("input#first_name");
        private readonly ILocator _lastName = page.Locator("input#last_name");
        private readonly ILocator _company = page.Locator("input#company");
        private readonly ILocator _address1 = page.Locator("input#address1");
        private readonly ILocator _address2 = page.Locator("input#address2");
        private readonly ILocator _country = page.Locator("select#country");
        private readonly ILocator _state = page.Locator("input#state");
        private readonly ILocator _city = page.Locator("input#city");
        private readonly ILocator _zipcode = page.Locator("input#zipcode");
        private readonly ILocator _mobileNumber = page.Locator("input#mobile_number");

        // Submit
        private readonly ILocator _createButton = page.Locator("button[data-qa='create-account']");

        // Post-creation confirmation
        private readonly ILocator _accountCreatedHeading = page.Locator("h2[data-qa='account-created']");
        private readonly ILocator _continueButton = page.Locator("a[data-qa='continue-button']");

        // ── Actions ───────────────────────────────────────────────────────────────
        public async Task FillAccountDetailsAsync(AccountDetails details)
        {
            // Title
            if (details.Title == "Mr")
                await _titleMr.CheckAsync();
            else
                await _titleMrs.CheckAsync();

            // Password
            await _passwordField.FillAsync(details.Password);

            // Date of birth
            await _dobDay.SelectOptionAsync(details.DateOfBirthDay);
            await _dobMonth.SelectOptionAsync(new SelectOptionValue { Label = details.DateOfBirthMonth });
            await _dobYear.SelectOptionAsync(details.DateOfBirthYear);

            // Optional opt-ins
            if (details.SignUpForNewsletter)
                await _newsletterChk.CheckAsync();
            if (details.ReceiveSpecialOffers)
                await _optinChk.CheckAsync();

            // Personal / address details
            await _firstName.FillAsync(details.FirstName);
            await _lastName.FillAsync(details.LastName);

            if (!string.IsNullOrEmpty(details.Company))
                await _company.FillAsync(details.Company);

            await _address1.FillAsync(details.Address1);

            if (!string.IsNullOrEmpty(details.Address2))
                await _address2.FillAsync(details.Address2);

            await _country.SelectOptionAsync(new SelectOptionValue { Label = details.Country });
            await _state.FillAsync(details.State);
            await _city.FillAsync(details.City);
            await _zipcode.FillAsync(details.Zipcode);
            await _mobileNumber.FillAsync(details.MobileNumber);
        }

        public async Task CreateAccountAsync()
            => await _createButton.ClickAsync();

        public async Task ClickContinueAsync()
            => await _continueButton.ClickAsync();

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<bool> IsAccountCreatedHeadingVisibleAsync()
            => await _accountCreatedHeading.IsVisibleAsync();

        public async Task<string> GetHeadingTextAsync()
            => await _heading.InnerTextAsync();
    }
}