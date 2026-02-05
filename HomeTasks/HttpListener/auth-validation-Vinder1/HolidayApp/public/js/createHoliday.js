import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null) {
    window.location.href = '/signin';
}

const form = document.getElementById('sign-up');
form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById('input-name').value;
    const date = document.getElementById('input-date').value;
    
    const response = await fetch('/api/userHolidays/new', {
        method: 'POST',
        headers: auth.headers(),
        body: JSON.stringify({ name, date })
    });
    
    const result = await response.json();
    if (!result.ok) {
        if (result.redirect)
        {
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