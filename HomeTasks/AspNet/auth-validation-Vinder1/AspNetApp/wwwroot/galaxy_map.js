const canvas = document.getElementById('galaxyCanvas');
const ctx = canvas.getContext('2d');

// Состояние
let stars = [];
let maxRadius = 0;
let minRadius = Infinity;
let rotation = 0;
let animationId = null;
let isAnimating = false;
let offsetX = 400;
let offsetY = 400;
let scale = 25;
let brightness = 0.8;
let spiralFactor = 1;
let isDragging = false;
let lastMouseX, lastMouseY;

// Цвета для разных радиусов
const colors = [
    '#ff6b6b', // красный
    '#ff8e8e',
    '#ffb56b', // оранжевый
    '#ffd700', // желтый
    '#ffffb0',
    '#b0ffb0', // зеленый
    '#8ed4ff', // голубой
    '#6ba5ff', // синий
    '#aa88ff', // фиолетовый
    '#d88eff',
    '#ff8ed4', // розовый
    '#ffb0b0',
    '#ffd48e',
    '#ffff8e',
    '#8effb0',
    '#8ed4ff'
];

// Загрузка данных
async function loadStars() {
    const loadingEl = document.getElementById('loading');
    const errorEl = document.getElementById('error');

    loadingEl.style.display = 'block';
    errorEl.style.display = 'none';

    try {
        const response = await fetch('/stars/all');
        if (!response.ok) {
            throw new Error(`HTTP ошибка: ${response.status}`);
        }

        const data = await response.json();

        // Пропускаем первый null элемент
        stars = data.slice(1).filter(group => group !== null).flat();

        // Вычисляем статистику
        maxRadius = Math.max(...stars.map(s => s.radius));
        minRadius = Math.min(...stars.map(s => s.radius));

        // Обновляем статистику
        document.getElementById('starCount').textContent = `Звезд: ${stars.length}`;
        document.getElementById('radiusRange').textContent = `Радиус: ${minRadius} - ${maxRadius}`;

        // Рисуем
        drawGalaxy();

    } catch (error) {
        console.error('Ошибка загрузки:', error);
        errorEl.textContent = `Ошибка загрузки: ${error.message}`;
        errorEl.style.display = 'block';
    } finally {
        loadingEl.style.display = 'none';
    }
}

// Рисование галактики
async function drawGalaxy() {
    ctx.clearRect(0, 0, 800, 800);

    // Рисуем звезды
    stars.forEach(star => {
        // Применяем вращение
        const angle = star.angleMilliradians / 1000;
        const rotatedAngle = angle + rotation;

        // Применяем спиральный фактор
        const spiralAngle = rotatedAngle + spiralFactor * Math.sin(star.radius * 0.5);

        // Вычисляем координаты
        const x = offsetX + star.radius * scale * Math.cos(spiralAngle);
        const y = offsetY + star.radius * scale * Math.sin(spiralAngle);

        // Проверяем, что точка в пределах канваса
        if (x >= 0 && x <= 800 && y >= 0 && y <= 800) {
            // Выбираем цвет на основе радиуса
            const colorIndex = Math.floor((star.radius - minRadius) / (maxRadius - minRadius) * (colors.length - 1));
            const color = colors[Math.min(colorIndex, colors.length - 1)];

            // Размер звезды зависит от радиуса
            const size = 1.5 + (star.radius / maxRadius) * 2;

            // Рисуем с прозрачностью
            ctx.beginPath();
            ctx.arc(x, y, size, 0, Math.PI * 2);
            ctx.fillStyle = color;
            ctx.globalAlpha = brightness;
            ctx.fill();

            // Добавляем свечение для ярких звезд
            if (star.radius > maxRadius * 0.7) {
                ctx.shadowColor = color;
                ctx.shadowBlur = 8;
                ctx.fill();
                ctx.shadowBlur = 0;
            }
        }
    });

    // Рисуем центр галактики
    ctx.beginPath();
    ctx.arc(offsetX, offsetY, 10, 0, Math.PI * 2);
    const gradient = ctx.createRadialGradient(offsetX, offsetY, 0, offsetX, offsetY, 20);
    gradient.addColorStop(0, '#ffffaa');
    gradient.addColorStop(1, '#ffaa00');
    ctx.fillStyle = gradient;
    ctx.shadowColor = '#ffaa00';
    ctx.shadowBlur = 20;
    ctx.fill();
    ctx.shadowBlur = 0;
}

function animate() {
    rotation += 0.002;
    drawGalaxy();
    animationId = requestAnimationFrame(animate);
}

document.getElementById("toggle-animation").addEventListener("click", async () =>
{
    if (isAnimating) {
        cancelAnimationFrame(animationId);
        isAnimating = false;
    } else {
        animate();
        isAnimating = true;
    }
});

document.getElementById("reset-view").addEventListener("click", async () =>
{
    rotation = 0;
    offsetX = 400;
    offsetY = 400;
    scale = 25;
    spiralFactor = 1;
    brightness = 0.8;

    document.getElementById('scale').value = 25;
    document.getElementById('brightness').value = 0.8;
    document.getElementById('spiral').value = 1;

    drawGalaxy();
});

// Обновление параметров
document.getElementById('brightness').addEventListener('input', function(event) {
    const value = event.target.value;
    brightness = parseFloat(value);
    drawGalaxy();
});

document.getElementById('scale').addEventListener('input', function(event) {
    const value = event.target.value;
    scale = parseFloat(value);
    drawGalaxy();
});

// document.getElementById('spiral').addEventListener('input', function(event) {
//     const value = event.target.value;
//     spiralFactor = parseFloat(value);
//     drawGalaxy();
// });

document.getElementById("reload-data").addEventListener("click", async () =>
{
    loadStars();
});

// Обработчики мыши для панорамирования
canvas.addEventListener('mousedown', (e) => {
    isDragging = true;
    lastMouseX = e.clientX;
    lastMouseY = e.clientY;
});

window.addEventListener('mousemove', (e) => {
    if (!isDragging) return;

    const deltaX = e.clientX - lastMouseX;
    const deltaY = e.clientY - lastMouseY;

    offsetX += deltaX;
    offsetY += deltaY;

    lastMouseX = e.clientX;
    lastMouseY = e.clientY;

    drawGalaxy();
});

window.addEventListener('mouseup', () => {
    isDragging = false;
});

// Обработчик колесика для зума
canvas.addEventListener('wheel', (e) => {
    e.preventDefault();
    const delta = e.deltaY > 0 ? -1 : 1;
    scale = Math.max(10, Math.min(50, scale + delta));
    document.getElementById('scale').value = scale;
    drawGalaxy();
});

// Загружаем данные при старте
loadStars();

// Загружаем каждые 30 секунд (опционально)
// setInterval(loadStars, 30000);