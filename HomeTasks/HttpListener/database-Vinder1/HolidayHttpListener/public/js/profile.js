import { auth } from './auth.js';

document.addEventListener('DOMContentLoaded', async () => {
    if (auth.isSignedIn()) {
        const login = auth.getLogin();
        const password = auth.getPassword();
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
                alert('Базу данных почистили, тебя стёрли');
                auth.clear();
                window.location.href = '/signin';
            }
        }
        document.getElementById('input-login').value = login;
        document.getElementById('input-name').value = data.username;
        document.getElementById('input-password').value = password;
        document.getElementById('user-balance').textContent = data.coins;
    } else {
        window.location.href = '/signin';
    }
});

const form = document.getElementById('sign-in');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    if (e.submitter.id == "delete-acc") {
        alert('ха! Забайтил!');
        const response2 = await fetch(`/profile/${auth.getLogin()}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        auth.clear();
        const result = await response2.json();
        if (result.redirect) {
            window.location.href = result.redirect;
        } else {
            window.location.href = '/home';
        }

        return;
    }

    const login = document.getElementById('input-login').value;
    const password = document.getElementById('input-password').value;
    const username = document.getElementById('input-name').value;
    const response = await fetch('/profile', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ login, username, password })
    });
    alert('Данные обновлены!');

});
