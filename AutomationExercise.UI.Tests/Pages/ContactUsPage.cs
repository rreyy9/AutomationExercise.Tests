namespace AutomationExercise.UI.Tests.Pages
{
    public class ContactUsPage(IPage page)
    {
        public NavBar Nav { get; } = new NavBar(page);

        // ── Locators ──────────────────────────────────────────────────────────────
        // ✅ Contact Us page uses data-qa attributes on all form fields
        private readonly ILocator _pageHeading = page.Locator("h2.title:has-text('GET IN TOUCH')");
        private readonly ILocator _nameInput = page.Locator("input[data-qa='name']");
        private readonly ILocator _emailInput = page.Locator("input[data-qa='email']");
        private readonly ILocator _subjectInput = page.Locator("input[data-qa='subject']");
        private readonly ILocator _messageInput = page.Locator("textarea[data-qa='message']");

        // ⚠️ File upload: use SetInputFilesAsync with the input element ref.
        //    Do NOT click — clicking opens a native OS dialog Playwright can't interact with.
        private readonly ILocator _fileUpload = page.Locator("input[name='upload_file']");

        // ⚠️ Submit is <input type="submit">, not a <button> element
        private readonly ILocator _submitButton = page.Locator("input[data-qa='submit-button']");

        private readonly ILocator _successMessage = page.Locator("div.status.alert.alert-success");
        private readonly ILocator _homeButton = page.Locator("a.btn-success:has-text('Home')");

        // ── Navigation ────────────────────────────────────────────────────────────
        public async Task GoToAsync()
            => await page.GotoAsync("/contact_us");

        // ── Form actions ──────────────────────────────────────────────────────────
        public async Task FillContactFormAsync(string name, string email, string subject, string message)
        {
            await _nameInput.FillAsync(name);
            await _emailInput.FillAsync(email);
            await _subjectInput.FillAsync(subject);
            await _messageInput.FillAsync(message);
        }

        public async Task UploadFileAsync(string absoluteFilePath)
            => await _fileUpload.SetInputFilesAsync(absoluteFilePath);

        /// <summary>
        /// Submits the contact form. The site triggers a browser confirm() dialog on submit.
        /// This method registers the dialog handler BEFORE clicking to auto-accept it.
        /// </summary>
        public async Task SubmitAsync()
        {
            // ⚠️ Register the dialog handler BEFORE clicking — the dialog fires synchronously
            //    and will block forever if no handler is registered before the click.
            page.Dialog += async (_, dialog) => await dialog.AcceptAsync();
            await _submitButton.ClickAsync();
        }

        public async Task<HomePage> ClickHomeAsync()
        {
            await _homeButton.ClickAsync();
            return new HomePage(page);
        }

        // ── State queries ─────────────────────────────────────────────────────────
        public async Task<string> GetSuccessMessageAsync()
            => await _successMessage.InnerTextAsync();

        public async Task<bool> IsSuccessMessageVisibleAsync()
            => await _successMessage.IsVisibleAsync();

        public async Task<bool> IsGetInTouchHeadingVisibleAsync()
            => await _pageHeading.IsVisibleAsync();
    }
}