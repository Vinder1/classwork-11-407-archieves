import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null) {
    window.location.href = '/signin';
}

if (user.coins < 5) {
    alert('У тебя недостаточно монет! Нужно 5 на подарок.');
    window.location.href = '/presents';
} 

const form = document.getElementById('sign-up');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const receiverLogin = document.getElementById('input-login').value;
    const inside = document.getElementById('input-name').value;
    const holidayId = parseInt(document.getElementById('input-hid').value);

    const response = await fetch('/api/presents/new', {
        method: 'POST',
        headers: auth.headers(),
        body: JSON.stringify({ inside, receiverLogin, holidayId })
    });

    const result = await response.json();
    if (!response.ok) {
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