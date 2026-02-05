import { auth } from './auth.js';

if (!auth.isSignedIn()) {
    window.location.href = '/signin';
}

const box = document.getElementById('list-box');
if (box) {
    const response = await fetch(`api/presents/${auth.getLogin()}`);
    const presents = await response.json();

    presents.forEach(async present => {

        const article = document.createElement('article');
        article.classList.add('holiday-article');

        const title = document.createElement('h3');
        title.textContent = present.inside;

        const response2 = await fetch(`api/user/byId/${present.fromId}`);
        const data = await response2.json();

        const date = document.createElement('p');
        date.textContent = `От: ${data.user.username}`;

        article.appendChild(title);
        article.appendChild(date);
        box.appendChild(article);
    });
}


// document.addEventListener('DOMContentLoaded', async () => {
const response2 = await fetch(`api/user/${auth.getLogin()}`);
const data = await response2.json();
if (data.ok) {
    document.getElementById('balance').textContent = data.user.coins;
} else {
    alert('Ошибка загрузки пользователя!');
    window.location.href = '/signin';
}
// });
