import { auth } from './auth.js';

const nav = document.querySelector('nav');
if (nav) {
    const links = [
        { href: 'home', label: 'К главной странице', icon: 'home.png' },
        { href: 'contact', label: 'Контакты', icon: 'contact.png' },
        { href: 'about', label: 'О проекте', icon: 'help.png' },
        { href: 'signup', label: 'Зарегистрироваться', icon: 'signup.png', id: 'signup-ref' },
        { href: 'signin', requiredSignIn: false, label: 'Вход', icon: 'signin.png', id: 'signin-ref' },
        { href: 'profile', requiredSignIn: true, label: 'Профиль', icon: 'profile.png', id: 'profile-ref' },
        { href: 'holidays', requiredSignIn: true, label: 'Мои праздники', icon: 'post.png', id: 'holidays-ref' },
        { href: 'presents', requiredSignIn: true, label: 'Мои подарки', icon: 'present.png', id: 'presents-ref' }
    ];

    const signedIn = auth.isSignedInLight();

    links.forEach(item => {
        if (item.requiredSignIn === true && !signedIn){
            return;
        }
        if (item.requiredSignIn === false && signedIn){
            return;
        }

        const link = document.createElement('a');
        link.href = item.href;
        link.setAttribute('aria-label', item.label);
        if (item.id) {
            link.id = item.id;
        }

        const img = document.createElement('img');
        img.src = `image/${item.icon}`;
        img.alt = item.label;
        img.className = 'corner-icon';

        link.classList.add('scale-on-hover');

        link.appendChild(img);
        nav.appendChild(link);
    });
}
