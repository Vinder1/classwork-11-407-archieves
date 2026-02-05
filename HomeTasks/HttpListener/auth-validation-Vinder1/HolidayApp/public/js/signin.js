import { auth } from './auth.js';

const form = document.getElementById('sign-in');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const login = document.getElementById('input-login').value;
    const password = document.getElementById('input-password').value;
    const response = await fetch('/signin', {
        method: 'POST',
        headers: auth.headers(),
        body: JSON.stringify({ login, password })
    });
    
    if (response.ok) {
        const data = await response.json();
        auth.setToken(data.token);
    }
    else {
        alert('Ошибка входа: несуществующий пользователь или неверный пароль. А может, ещё что-то. В любом случае, ты балбесина');
        return;
    }

    window.location.href = '/home';
});
