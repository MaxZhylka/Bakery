import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { NgxsModule } from '@ngxs/store';
import { AppState, UserState } from './store/app.state';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';

import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { ProductsState } from './store/products.state';
import { OrdersState } from './store/orders.state';
import { LogsState } from './store/logs.state';
import { authInterceptor } from './interceptors/authInterceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideAnimationsAsync(),
    importProvidersFrom(
      NgxsModule.forRoot([UserState, AppState, ProductsState, OrdersState, LogsState]),
      NgxsReduxDevtoolsPluginModule.forRoot(),
      NgxsLoggerPluginModule.forRoot()
    )
  ],
};
