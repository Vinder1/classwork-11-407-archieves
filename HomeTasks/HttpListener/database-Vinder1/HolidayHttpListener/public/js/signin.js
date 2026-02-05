import { auth } from './auth.js';

if (auth.isSignedIn()){
    window.location.href = '/home';
}

const form = document.getElementById('sign-in');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const login = document.getElementById('input-login').value;
    const password = document.getElementById('input-password').value;
    const response = await fetch('/signin', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ login, password })
    });
    const data = await response.json();

    if (response.ok) {
        if (data.id == -1) {
            alert('Ошибка входа: несуществующий пользователь или неверный пароль. В любом случае, ты балбесина');
            return;
        }
    }
    else {
        alert('Ошибка входа, неясно какая');
        return;
    }

    auth.setLogin(login);
    auth.setPassword(password);

    window.location.href = '/home';
});
