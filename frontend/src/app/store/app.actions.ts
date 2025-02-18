import { AuthResponse, RegisterPayload, User } from "../interfaces";


export class SetLoading {
    static readonly type = '[App] Set Loading';
    constructor(public payload: boolean) {}
}

export class Login {
    static readonly type = '[User] Login';
    constructor(public payload: { email: string; password: string }) {}
}

export class Register {
    static readonly type = '[User] Register';
    constructor(public payload: RegisterPayload) {}
}

export class LoadChats {
    static readonly type = '[User] Load chats';
    constructor( public payload: {userId: string}) {}
}

export class RegisterSuccess {
    static readonly type = '[User] Register Success';
    constructor(public payload: AuthResponse ) {}
}

export class LoginSuccess {
    static readonly type = '[User] Login Success';
    constructor(public  user: User ) {}
}

export class LoginFail {
    static readonly type = '[User] Login Fail';
    constructor(public payload: string) {}
}

export class RegisterFail {
    static readonly type = '[User] Register Fail';
    constructor(public payload: string) {}
}

export class CheckAuth {
    static readonly type = '[Auth] Check Auth';
}

export class SetUser {
    static readonly type = '[Auth] Set User';
    constructor(public payload: User) {}
}

export class Logout {
    static readonly type = '[Auth] Logout';
}

export class LogoutSuccess {
    static readonly type = '[Auth] Logout success';
}

export class LogoutFail {
    static readonly type = '[Auth] Logout fail';
}