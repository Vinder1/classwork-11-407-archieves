export function validate(data) {
    const errors = [];

    // Username: 4–25, буквы (латиница + кириллица), цифры, _
    if (!data.username || !/^[a-zA-Zа-яА-ЯёЁ0-9_]{4,25}$/.test(data.username)) {
        errors.push("Имя должно быть от 4 до 25 символов и состоять только из '_' и латинских символов / символов кириллицы.");
    }

    // Login: 4–20, только латиница, цифры, _
    if (!data.login || !/^[a-zA-Z0-9_]{4,20}$/.test(data.login)) {
        errors.push("Логин должен быть от 4 до 20 символов и состоять только из '_' и латинских символов.");
    }

    // Password: минимум 8, латинские буквы + цифры
    if (!data.password || !/^(?=.*[a-zA-Z])(?=.*\d).{8,}$/.test(data.password)) {
        errors.push("Пароль должен состоять минимум из 8 символов и содержать и латинские буквы, и цифры.");
    }

    // Email
    if (!data.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(data.email)) {
        errors.push("Некорректная почта");
    }

    // Birthday: от 1 до 150 лет
    if (!data.birthday) {
        errors.push("Укажите дату рождения");
    } else {
        const b = new Date(data.birthday);
        const now = new Date();
        const minDate = new Date(now.getFullYear() - 150, now.getMonth(), now.getDate());
        const maxDate = new Date(now.getFullYear() - 1, now.getMonth(), now.getDate());
        if (b > maxDate || b < minDate || isNaN(b.getTime())) {
            errors.push("Вам должно быть от 1 до 150 лет");
        }
    }

    // Auf: число от -4 до 100_000, не делится на 13
    const auf = Number(data.auf);
    if (
        !data.auf ||
        isNaN(auf) ||
        auf <= -4 ||
        auf >= 100_000 ||
        auf % 13 === 0
    ) {
        errors.push("Бесполезное поле должно быть в диапазоне от -4 до 100_000 и не должно делиться на 13");
    }

    return errors;
}