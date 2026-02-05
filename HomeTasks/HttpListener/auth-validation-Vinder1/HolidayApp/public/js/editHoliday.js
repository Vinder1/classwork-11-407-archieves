import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null){
    window.location.href = '/signin';
}


const parts = window.location.pathname.split('/');
const id = parts[parts.length - 1];

const response = await fetch(`api/userHolidays/byId/${id}`);
const holiday = await response.json();

if (user == null) {
    auth.clear();
    window.location.href = '/signin';
}

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
        headers: auth.headers(),
        body: JSON.stringify({ id: holiday.id, name, date })
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

    if (result.redirect) {
        window.location.href = result.redirect;
    } else {
        window.location.href = '/holidays';
    }
});

document.getElementById('delete').addEventListener("click", async () => {
    const response = await fetch(`/api/userHolidays/delete/${holiday.id}`, {
        method: 'DELETE',
        headers: auth.headers(),
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