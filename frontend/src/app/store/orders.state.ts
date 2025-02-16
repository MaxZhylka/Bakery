import { Injectable } from '@angular/core';
import { State, Action, StateContext, Store } from '@ngxs/store';
import { OrdersService } from '../services/order-service/order.service';
import {
  CreateOrder, CreateOrderFail, CreateOrderSuccess,
  GetOrders, GetOrdersFail, GetOrdersSuccess,
  GetOrderDynamic, GetOrderDynamicFail, GetOrderDynamicSuccess,
  UpdateOrder, UpdateOrderFail, UpdateOrderSuccess,
  DeleteOrder, DeleteOrderFail, DeleteOrderSuccess
} from './orders.actions';
import { catchError, tap } from 'rxjs/operators';
import { of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SetLoading } from './app.actions';


export interface OrdersStateModel {
  orders: any[];
  dynamicData: any;
  error: string | null;
}

@State<OrdersStateModel>({
  name: 'orders',
  defaults: {
    orders: [],
    dynamicData: null,
    error: null,
  }
})
@Injectable()
export class OrdersState {

  constructor(
    private readonly ordersService: OrdersService,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) { }

  @Action(GetOrders)
  getOrders(ctx: StateContext<OrdersStateModel>, { paginationParams }: GetOrders) {
    this.store.dispatch(new SetLoading(true));
    return this.ordersService.getOrders(paginationParams).pipe(
      tap((orders) => ctx.dispatch(new GetOrdersSuccess(orders))),
      catchError((error) => {
        ctx.dispatch(new GetOrdersFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
  
  @Action(CreateOrder)
  createOrder(ctx: StateContext<OrdersStateModel>, { orderData }: CreateOrder) {
    this.store.dispatch(new SetLoading(true));
    return this.ordersService.createOrder(orderData).pipe(
      tap((createdOrder) => ctx.dispatch(new CreateOrderSuccess(createdOrder))),
      catchError((error) => {
        ctx.dispatch(new CreateOrderFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
  
  @Action(UpdateOrder)
  updateOrder(ctx: StateContext<OrdersStateModel>, { orderId, updateData }: UpdateOrder) {
    this.store.dispatch(new SetLoading(true));
    return this.ordersService.updateOrder(orderId, updateData).pipe(
      tap(() => ctx.dispatch(new UpdateOrderSuccess())),
      catchError((error) => {
        ctx.dispatch(new UpdateOrderFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
  
  @Action(DeleteOrder)
  deleteOrder(ctx: StateContext<OrdersStateModel>, { orderId }: DeleteOrder) {
    this.store.dispatch(new SetLoading(true));
    return this.ordersService.deleteOrder(orderId).pipe(
      tap(() => ctx.dispatch(new DeleteOrderSuccess())),
      catchError((error) => {
        ctx.dispatch(new DeleteOrderFail(error.message));
        return of();
      }),
      tap(() => this.store.dispatch(new SetLoading(false)))
    );
  }
  

  @Action(GetOrdersSuccess)
  getOrdersSuccess(ctx: StateContext<OrdersStateModel>, { orders }: GetOrdersSuccess) {
    ctx.patchState({ orders, error: null });
  }
  
  @Action(GetOrdersFail)
  getOrdersFail(ctx: StateContext<OrdersStateModel>, { error }: GetOrdersFail) {
    ctx.patchState({ error });
  }
  
  @Action(CreateOrderSuccess)
  createOrderSuccess(ctx: StateContext<OrdersStateModel>, { createdOrder }: CreateOrderSuccess) {
    this.snackBar.open('Замовлення створено успішно', 'Закрити', { duration: 3000 });
  }
  
  @Action(CreateOrderFail)
  createOrderFail(ctx: StateContext<OrdersStateModel>, { error }: CreateOrderFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка створення замовлення', 'Закрити', { duration: 3000 });
  }
  
  @Action(UpdateOrderSuccess)
  updateOrderSuccess() {
    this.snackBar.open('Замовлення оновлено', 'Закрити', { duration: 3000 });
  }
  
  @Action(UpdateOrderFail)
  updateOrderFail(ctx: StateContext<OrdersStateModel>, { error }: UpdateOrderFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка оновлення замовлення', 'Закрити', { duration: 3000 });
  }
  
  @Action(DeleteOrderSuccess)
  deleteOrderSuccess() {
    this.snackBar.open('Замовлення видалено', 'Закрити', { duration: 3000 });
  }
  
  @Action(DeleteOrderFail)
  deleteOrderFail(ctx: StateContext<OrdersStateModel>, { error }: DeleteOrderFail) {
    ctx.patchState({ error });
    this.snackBar.open('Помилка видалення замовлення', 'Закрити', { duration: 3000 });
  }

  @Action(GetOrderDynamic)
  getOrderDynamic(ctx: StateContext<OrdersStateModel>) {
    this.ordersService.getOrderDynamic();
  }
}
