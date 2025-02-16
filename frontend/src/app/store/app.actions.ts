import { User } from "../interfaces";


export class SetLoading {
    static readonly type = '[App] Set Loading';
    constructor(public payload: boolean) {}
}

export class SetMessageLoading {
    static readonly type = '[App] Set Loading';
    constructor(public payload: boolean) {}
}

export class Login {
    static readonly type = '[User] Login';
    constructor(public payload: { email: string; password: string }) {}
}

export class Register {
    static readonly type = '[User] Register';
    constructor(public payload: { login: string; email: string; password: string }) {}
}

export class LoadChats {
    static readonly type = '[User] Load chats';
    constructor( public payload: {userId: string}) {}
}

export class RegisterSuccess {
    static readonly type = '[User] Register Success';
    constructor(public payload: { user: User }) {}
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

export class LoadMoreMessages {
    static readonly type = '[Chat] Load More Messages';
    constructor(public chatId: number, public paginationValue: number) {}
}