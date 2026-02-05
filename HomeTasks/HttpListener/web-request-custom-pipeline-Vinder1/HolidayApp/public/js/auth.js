export const auth = {
    // async isSignedIn() {
    //     // return this.hasLogin() && this.hasPassword();
    //     if (this.getToken() == null)
    //         return false;
    //     const response = await fetch('/api/login', {
    //         method: 'GET',
    //         headers: this.headers()
    //     });
    //     if (!response.ok) {
    //         this.clear();
    //         return false;
    //     }
    //     return true;
    // },

    isSignedInLight() {
        return this.getToken() != null;
    },

    getToken() {
        const token = localStorage.getItem("token");
        return token;
    },

    async getUser() {
        const response = await fetch('/api/getLocalUser', {
            method: 'GET',
            headers: this.headers()
        });
        const newToken = response.headers.get('X-New-Token');
        if (newToken) {
            this.setToken(newToken);
        }
        if (!response.ok) {
            this.clear();
            return null;
        }
        var result = await response.json();
        return result.user;
    },

    setToken(value) {
        localStorage.setItem("token", value);
    },

    headers() {
        const headers = { "Content-Type": "application/json" };
        const token = this.getToken();
        if (token) {
            headers["Authorization"] = `Bearer ${token}`;
        }
        return headers;
    },

    // hasLogin() {
    //     return document.cookie.split(';').some(cookie =>
    //         cookie.trim().startsWith('login=')
    //     );
    // },

    // setLogin(login) {
    //     const expires = new Date(Date.now() + 3600 * 24 * 1000).toUTCString();
    //     document.cookie = `login=${encodeURIComponent(login)}; expires=${expires}; path=/`;
    // },

    // getLogin() {
    //     return document.cookie.split(';').find(cookie =>
    //         cookie.trim().startsWith('login=')
    //     ).trim().split('=')[1];
    // },

    // setPassword(password) {
    //     const expires = new Date(Date.now() + 3600 * 24 * 1000).toUTCString();
    //     document.cookie = `password=${encodeURIComponent(password)}; expires=${expires}; path=/`;
    // },

    // hasPassword() {
    //     return document.cookie.split(';').some(cookie =>
    //         cookie.trim().startsWith('password=')
    //     );
    // },

    // getPassword() {
    //     return document.cookie.split(';').find(cookie =>
    //         cookie.trim().startsWith('password=')
    //     ).trim().split('=')[1];
    // },

    clear() {
        //document.cookie = 'login=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/';
        localStorage.removeItem("token");
    }
}