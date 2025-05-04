const loginForm = document.querySelector('#login-form')

document.getElementById('toggle-password').addEventListener('click', () => {
    const passwordInput = document.getElementById('password');
    const eyeIcon = document.getElementById('toggle-password');
    const isPassword = passwordInput.type === 'password';

    passwordInput.type = isPassword ? 'text' : 'password';
    eyeIcon.textContent = isPassword ? '🙈' : '👁️'; // toggle icon
});


loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();

    const formData = new FormData(loginForm)
    const email = formData.get('email')
    const password = formData.get('password')

    try {
        if (!email) throw new Error("Email is required.");
        if (!password) throw new Error("Password is required.");

        status.textContent = "Logging in...";

        const response = await fetch("/Auth/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ email, password }) // 🔁 Customize if needed
        });

        const data = await response.json();
        if (!response.ok) {
            console.log(data.errorMessage)
            throw new Error("Login failed");
        }

        const token = data.token; // 🔁 Adjust if your token key is different

        localStorage.setItem("token", token)

        statusText.style.color = 'rebeccapurple'
        statusText.textContent = "Login successful";
        window.ui.preauthorizeApiKey("Bearer", token);
    } catch (err) {
        statusText.textContent = err;
    } finally {
        clearStatusText()
        loginDialog.close()
    }
})


function clearStatusText() {
    setTimeout(() => {
        statusText.textContent = '';
    }, 3000)
}

document.querySelector('#close-dialog-button').addEventListener('click', () => {
    loginDialog.close()
})