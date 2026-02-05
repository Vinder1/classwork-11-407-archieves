const overlay = document.getElementById('overlay');
const openBtn = document.getElementById('showModalBtn');
const closeBtn = document.getElementById('closeModalBtn');

// Показать оверлей
openBtn.addEventListener('click', () => {
    overlay.classList.add('active');
});

// Скрыть оверлей
closeBtn.addEventListener('click', () => {
    overlay.classList.remove('active');
});

// Закрытие при клике на оверлей (не на окно)
overlay.addEventListener('click', (e) => {
    if (e.target === overlay) {
        overlay.classList.remove('active');
    }
});

// Закрытие по клавише Esc
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && overlay.classList.contains('active')) {
        overlay.classList.remove('active');
    }
});