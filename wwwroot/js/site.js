// Small progressive enhancement: prevent accidental double-submit on forms.
document.addEventListener("submit", function (event) {
    const button = event.target.querySelector('button[type="submit"]');
    if (button && !event.target.classList.contains("allow-repeat")) {
        button.dataset.originalText = button.textContent;
        button.disabled = true;
        button.textContent = "Please wait...";
    }
});
