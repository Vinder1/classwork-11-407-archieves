import { auth } from './auth.js';

if (!auth.isSignedIn()) {
    window.location.href = '/signin';
}

const parts = window.location.pathname.split('/');
const id = parts[parts.length - 1];

const response = await fetch(`api/userHolidays/byId/${id}`);
const holiday = await response.json();

const response2 = await fetch(`api/user/byId/${holiday.creator}`);
const data = await response2.json();

if (!data.ok) {
    auth.clear();
    window.location.href = '/signin';
}

const user = data.user;

if (holiday.creator != user.id) {
    alert('Это не твой праздник. Тебе сюда нельзя!');
    window.location.href = '/home';
}

const form = document.getElementById('sign-up');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById('input-name').value;
    const date = document.getElementById('input-date').value;

    const response = await fetch('/api/userHolidays/edit', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ id: holiday.id, name, date })
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

    if (result.redirect) {
        window.location.href = result.redirect;
    } else {
        window.location.href = '/holidays';
    }
});

document.getElementById('delete').addEventListener("click", async () => {
    const response = await fetch(`/api/userHolidays/delete/${holiday.id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    });
    const result = await response.json();
    if (result.ok) {
        if (result.redirect) {
            window.location.href = result.redirect;
        } else {
            window.location.href = '/holidays';
        }
    }

    return;
});