import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null) {
    window.location.href = '/signin';
}

const box = document.getElementById('list-box');
if (box) {
    const response = await fetch('api/presents', {
        method: 'GET',
        headers: auth.headers()
    });
    const presents = await response.json();

    presents.forEach(async present => {

        const article = document.createElement('article');
        article.classList.add('holiday-article');

        const title = document.createElement('h3');
        title.textContent = present.inside;

        const date = document.createElement('p');
        date.textContent = `От: ${user.username}`;

        article.appendChild(title);
        article.appendChild(date);
        box.appendChild(article);
    });
}

document.getElementById('balance').textContent = user.coins;