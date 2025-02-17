import { PaginationParams, ICreateOrder, IUpdateOrder, DataByPagination, Order } from '../interfaces';

export class GetOrders {
  static readonly type = '[Orders] Get orders';
  constructor(public paginationParams: PaginationParams) {}
}

export class GetOrdersSuccess {
  static readonly type = '[Orders] Get orders success';
  constructor(public orders: DataByPagination<Order[]>) {}
}

export class GetOrdersFail {
  static readonly type = '[Orders] Get orders fail';
  constructor(public error: string) {}
}

export class CreateOrder {
  static readonly type = '[Orders] Create order';
  constructor(public orderData: ICreateOrder) {}
}

export class CreateOrderSuccess {
  static readonly type = '[Orders] Create order success';
  constructor(public createdOrder: Order) {}
}

export class CreateOrderFail {
  static readonly type = '[Orders] Create order fail';
  constructor(public error: string) {}
}

export class GetOrderDynamic {
  static readonly type = '[Orders] Get order dynamic';
}

export class GetOrderDynamicSuccess {
  static readonly type = '[Orders] Get order dynamic success';
  constructor(public dynamicData: any) {}
}

export class GetOrderDynamicFail {
  static readonly type = '[Orders] Get order dynamic fail';
  constructor(public error: string) {}
}

export class UpdateOrder {
  static readonly type = '[Orders] Update order';
  constructor(public orderId: string, public updateData: IUpdateOrder) {}
}

export class UpdateOrderSuccess {
  static readonly type = '[Orders] Update order success';
}

export class UpdateOrderFail {
  static readonly type = '[Orders] Update order fail';
  constructor(public error: string) {}
}

export class DeleteOrder {
  static readonly type = '[Orders] Delete order';
  constructor(public orderId: string) {}
}

export class DeleteOrderSuccess {
  static readonly type = '[Orders] Delete order success';
}

export class DeleteOrderFail {
  static readonly type = '[Orders] Delete order fail';
  constructor(public error: string) {}
}
