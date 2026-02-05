import { auth } from './auth.js';

if (!auth.isSignedIn()) {
    window.location.href = '/signin';
}

document.addEventListener('DOMContentLoaded', async () => {
    const url = new URL('api/presents/canCreate', window.location.origin);
    url.searchParams.append('login', auth.getLogin());

    const response2 = await fetch(url);
    const data = await response2.json();
    if (data.ok) {
        if (!data.value) {
            alert('У тебя недостаточно монет! Нужно 5 на подарок.');
            window.location.href = '/presents';
        }
    } else {
        alert('Ошибка загрузки пользователя!');
        window.location.href = '/signin';
    }

    const form = document.getElementById('sign-up');
    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const receiverLogin = document.getElementById('input-login').value;
        const inside = document.getElementById('input-name').value;
        const holidayId = parseInt(document.getElementById('input-hid').value);

        const response = await fetch('/api/presents/new', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ inside, receiverLogin, senderLogin: auth.getLogin(), holidayId })
        });

        const result = await response.json();
        if (!result.ok) {
            if (result.redirect) {
                alert('Что-то не так с вашим сохранённым логином. Войдите ещё раз');
                window.location.href = result.redirect;
            } else {
                alert('Ошибка во входных данных');
            }
            return;
        }
        alert('Подарок отправлен!');

        if (result.redirect) {
            window.location.href = result.redirect;
        } else {
            window.location.href = '/holidays';
        }
    });
});