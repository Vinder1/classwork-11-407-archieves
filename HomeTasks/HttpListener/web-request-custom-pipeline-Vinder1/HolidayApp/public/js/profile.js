import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null){
    window.location.href = '/signin';
}

document.getElementById('input-login').value = user.login;
document.getElementById('input-name').value = user.username;
document.getElementById('input-password').value = '';
document.getElementById('user-balance').textContent = user.coins;

const form = document.getElementById('sign-in');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    if (e.submitter.id == "delete-acc") {
        alert('ха! Забайтил!');
        const response2 = await fetch(`/profile`, {
            method: 'DELETE',
            headers: auth.headers()
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

    const password = document.getElementById('input-password').value;
    const username = document.getElementById('input-name').value;

    var response = await fetch('/profile/update', {
        method: 'PUT',
        headers: auth.headers(),
        body: JSON.stringify({ username, password })
    });
    var result = await response.json();
    if (!response.ok){
        if (result.errors) {
            alert(result.errors.join('\n'));    
        } else {
            alert('Пользователь с таким логином уже существует. Будь оригинальнее');
        }
        return;
    }
    alert('Данные обновлены!');
});
