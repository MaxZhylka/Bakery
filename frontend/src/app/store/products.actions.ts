import { PaginationParams, Product } from "../interfaces";

export class GetProducts {
    static readonly type = '[Products] Get products';
    constructor(public paginationParams: PaginationParams) { }
}
export class GetProductsWhereCountMoreThan10 {
    static readonly type = '[Product] Get Products Where Count More Than 10';
    constructor(public payload: PaginationParams) { }
}

export class GetProductsWherePriceMoreThan250 {
    static readonly type = '[Product] Get Products Where Price More Than 250';
    constructor(public payload: PaginationParams) { }
}

export class GetProductsWhereCountMoreThan50AndPriceLessThan100 {
    static readonly type = '[Product] Get Products Where Count More Than 50 And Price Less Than 100';
    constructor(public payload: PaginationParams) { }
}

export class CreateProduct {
    static readonly type = '[Product] Create Product';
    constructor(public product: Product) { }
}

export class UpdateProduct {
    static readonly type = '[Product]  Update Product';
    constructor(public product: Product) { }
}

export class DeleteProduct {
    static readonly type = '[Product] Delete Product';
    constructor(public productId: string) { }
}