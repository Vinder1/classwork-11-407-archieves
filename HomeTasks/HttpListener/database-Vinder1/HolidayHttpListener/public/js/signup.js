import { auth } from './auth.js';

const form = document.getElementById('sign-up');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const login = document.getElementById('input-login').value;
    const password = document.getElementById('input-password').value;
    const username = document.getElementById('input-name').value;
    const response = await fetch('/signup', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ login, username, password })
    });

    
    const result = await response.json();
    if (!result.ok){
        alert('Пользователь с таким логином уже существует. Будь оригинальнее');
        return;
    }

    auth.setLogin(login);
    auth.setPassword(password);

    if (result.redirect) {
        window.location.href = result.redirect;
    } else {
        window.location.href = '/home';
    }
});