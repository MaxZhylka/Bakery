export interface AppStateModel {
    isLoading: boolean;
}

export interface User {
    id: string;
    name: string;
    email: string;
    role: Roles;
    createdAt: string;
}

export interface DataByPagination<T> {
    data: T;
    total: number;
}

export interface UserStateModel {
    currentUser: User | null;
}

export interface LoginPayload {
    email: string;
    password: string;
}

export interface RegisterPayload {
    name: string;
    email: string;
    password: string;
}

export interface Order {
    id: string;
    productName: string;
    productCount: number;
    price: number;
    createdAt: string;
    customerName: string;
}

export interface Product {
    id: string;
    name: string;
    price: number;
    productCount: number;
    createdAt: string;
}

export interface Log {
    id: string;
    userName: string;
    userRole: Roles;
    operation: string;
    details: string;
    timestamp: string;
}

export interface PaginationParams {
    size: number;
    offset: number;
}

export interface ICreateOrder {
    productId: string;
    productCount: number;
    price: number;
    customerName: string;
};


export interface AuthResponse extends User {
    accessToken: string;
}

export interface IUpdateOrder {
    productId: string;
    productCount: number;
    price: number;
    customerName: string;
}

export interface ICreateUser {
    name: string;
    password: string;
    role: Roles;
    email: string;
}

export enum Roles {
    Admin = 'Admin',
    User = 'User',
    Manager = 'Manager'
}