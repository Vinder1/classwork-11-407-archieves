import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null){
    window.location.href = '/signin';
}

function formatDate(dateString) {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString('ru-RU', options);
}

const box = document.getElementById('list-box');
if (box) {
    const response = await fetch(`api/userHolidays/byCreator/${user.id}`);
    const holidays = await response.json();

    holidays.forEach(holiday => {

        const article = document.createElement('article');
        article.classList.add('holiday-article');

        const title = document.createElement('h3');
        title.textContent = holiday.name;

        const date = document.createElement('p');
        date.textContent = `${formatDate(holiday.celebrationDay)}`;

        const button = document.createElement('button');
        button.classList.add('holiday-edit-btn');
        button.innerHTML = 'Изменить';
        button.addEventListener('click', (e) => {
            window.location.href = `holidays/${holiday.id}`;
        });

        article.appendChild(title);
        article.appendChild(date);
        article.appendChild(button);
        box.appendChild(article);
    });
}
