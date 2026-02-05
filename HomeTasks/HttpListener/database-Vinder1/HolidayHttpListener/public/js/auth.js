export const auth = {
    isSignedIn() {
        return this.hasLogin() && this.hasPassword();
    },

    hasLogin() {
        return document.cookie.split(';').some(cookie =>
            cookie.trim().startsWith('login=')
        );
    },

    setLogin(login) {
        const expires = new Date(Date.now() + 3600*24*1000).toUTCString();
        document.cookie = `login=${encodeURIComponent(login)}; expires=${expires}; path=/`;
    },

    getLogin() {
        return document.cookie.split(';').find(cookie =>
            cookie.trim().startsWith('login=')
        ).trim().split('=')[1];
    },

    setPassword(password) {
        const expires = new Date(Date.now() + 3600*24*1000).toUTCString();
        document.cookie = `password=${encodeURIComponent(password)}; expires=${expires}; path=/`;
    },

    hasPassword() {
        return document.cookie.split(';').some(cookie =>
            cookie.trim().startsWith('password=')
        );
    },

    getPassword() {
        return document.cookie.split(';').find(cookie =>
            cookie.trim().startsWith('password=')
        ).trim().split('=')[1];
    },

    clear(){
        document.cookie = 'login=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/';
    }
}