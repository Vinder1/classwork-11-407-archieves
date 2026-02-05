import { auth } from './auth.js';

const user = await auth.getUser();
if (user == null){
    window.location.href = '/signin';
}

if (user.role != 'admin'){
    window.location.href = '/home';
}