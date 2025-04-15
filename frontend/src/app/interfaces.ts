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
    customerId: string;
};


export interface AuthResponse extends User {
    accessToken: string;
}

export interface IUpdateOrder {
    productId: string;
    productCount: number;
    price: number;
    customerId: string;
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

export interface ProductSales {
    productName: string;
    totalSold: string;
}

export enum LoanTerm {
    OneMonth = 'OneMonth',
    TwoMonth = 'TwoMonth',
    ThreeMonth = 'ThreeMonth',
    SixMonth = 'SixMonth',
    OneYear = 'OneYear'
}

export const LoanTermViewMap: Record<LoanTerm, string> = {
    [LoanTerm.OneMonth]: '1 місяць',
    [LoanTerm.TwoMonth]: "2 місяці",
    [LoanTerm.ThreeMonth]: '3 місяці',
    [LoanTerm.SixMonth]: '6 місяців',
    [LoanTerm.OneYear]: "1 рік"
};

export const PercentByTermMap: Record<LoanTerm, number> = {
    [LoanTerm.OneMonth]: 2,
    [LoanTerm.TwoMonth]: 4,
    [LoanTerm.ThreeMonth]: 6,
    [LoanTerm.SixMonth]: 12,
    [LoanTerm.OneYear]: 24
}

export const PaymentCountByTermMap: Record<LoanTerm, number> = {
    [LoanTerm.OneMonth]: 1,
    [LoanTerm.TwoMonth]: 2,
    [LoanTerm.ThreeMonth]: 3,
    [LoanTerm.SixMonth]: 6,
    [LoanTerm.OneYear]: 12
}

export enum LoanApplicationStatus {
    Moderation = 'Moderation',
    Approved = 'Approved',
    Rejected = 'Rejected',
}

export const ApplicationStatusesText: Record<LoanApplicationStatus, string> = {
    [LoanApplicationStatus.Moderation]: "Потребує перевірки",
    [LoanApplicationStatus.Approved]: "Одобрено",
    [LoanApplicationStatus.Rejected]: "Відхилено",
}

export enum LoanStatus {
    Active = 'Active',
    GetMoney = 'GetMoney',
    NeedPayment = 'NeedPayment',
    Completed = 'Completed',
}

export enum PaymentStatus {
    Active = 'В обробці',
    Completed = 'Успішно'
}
export interface LoanApplication {
    id: string;
    userId: string;
    value: number;
    status: LoanApplicationStatus;
    term: LoanTerm;
    createdAt: Date;
    rejectionReason?: string;
    clientEmail?: string;
}

export interface Loan {
    id: string;
    userId: string;
    percent: number;
    valueToPayOnCurrentMonth: number;
    valueToPay: number;
    status: LoanStatus;
    nextPaymentDate: Date;
    createdAt: Date;
    completedValue: number;
    leftValue: number;
    clientEmail: string;
}

export interface Payment {
    id: string;
    userId: string;
    value: number;
    status: PaymentStatus;
    createdAt: Date;
}

export const mockLoans: Loan[] = [
    {
        id: '1',
        userId: 'user1',
        percent: 10,
        valueToPayOnCurrentMonth: 1000,
        valueToPay: 5000,
        status: LoanStatus.NeedPayment,
        nextPaymentDate: new Date('2025-05-01'),
        createdAt: new Date('2025-03-01'),
        completedValue: 4000,
        leftValue: 1000,
        clientEmail: 'user1@example.com',
    },
    {
        id: '2',
        userId: 'user2',
        percent: 15,
        valueToPayOnCurrentMonth: 2000,
        valueToPay: 8000,
        status: LoanStatus.GetMoney,
        nextPaymentDate: new Date('2025-04-20'),
        createdAt: new Date('2025-03-15'),
        completedValue: 0,
        leftValue: 8000,
        clientEmail: 'user2@example.com',
    },
    {
        id: '3',
        userId: 'user3',
        percent: 12,
        valueToPayOnCurrentMonth: 1500,
        valueToPay: 6000,
        status: LoanStatus.Active,
        nextPaymentDate: new Date('2025-04-25'),
        createdAt: new Date('2025-02-20'),
        completedValue: 3000,
        leftValue: 3000,
        clientEmail: 'user3@example.com',
    },
    {
        id: '4',
        userId: 'user4',
        percent: 8,
        valueToPayOnCurrentMonth: 0,
        valueToPay: 4000,
        status: LoanStatus.Completed,
        nextPaymentDate: new Date('2025-04-01'),
        createdAt: new Date('2025-01-10'),
        completedValue: 4000,
        leftValue: 0,
        clientEmail: 'user4@example.com',
    }
];

export const mockLoanApplications: LoanApplication[] = [
    {
        id: '1',
        userId: 'u1',
        value: 5000,
        status: LoanApplicationStatus.Moderation,
        term: LoanTerm.ThreeMonth,
        createdAt: new Date('2025-04-10'),
        clientEmail: 'user1@example.com',
    },
    {
        id: '2',
        userId: 'u2',
        value: 8000,
        status: LoanApplicationStatus.Approved,
        term: LoanTerm.OneYear,
        createdAt: new Date('2025-04-09'),
        clientEmail: 'user2@example.com',
    },
    {
        id: '3',
        userId: 'u3',
        value: 3000,
        status: LoanApplicationStatus.Rejected,
        term: LoanTerm.SixMonth,
        createdAt: new Date('2025-04-08'),
        rejectionReason: 'Неправильно вказані дані',
        clientEmail: 'user3@example.com',
    }
];

export interface ILoanApplicationCreate {
    value: number;
    term: LoanTerm;
    userId?: string;
}
