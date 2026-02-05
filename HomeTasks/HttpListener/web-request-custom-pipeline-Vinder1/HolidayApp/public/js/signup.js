import { auth } from './auth.js';
import { validate } from './validation.js';

const form = document.getElementById('sign-up');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const login = document.getElementById('input-login').value;
    const password = document.getElementById('input-password').value;
    const username = document.getElementById('input-name').value;

    var errors = validate({ login, username, password });
    if (!errors.length == 0){
        alert(errors.join('\n'));
        return;
    }

    const response = await fetch('/signup', {
        method: 'POST',
        headers: auth.headers(),
        body: JSON.stringify({ login, username, password })
    });

    const result = await response.json();
    if (!response.ok){
        if (result.errors) {
            alert(result.errors.join('\n'));    
        } else {
            alert('Пользователь с таким логином уже существует. Будь оригинальнее');
        }
        return;
    }

    auth.setToken(result.token);

    if (result.redirect) {
        window.location.href = result.redirect;
    } else {
        window.location.href = '/home';
    }
});