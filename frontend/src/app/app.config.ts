import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { NgxsModule } from '@ngxs/store';
import { AppState, UserState } from './store/app.state';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { AuthInterceptor } from './interceptors/authInterceptor';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideAnimationsAsync(),
    importProvidersFrom(
      NgxsModule.forRoot([UserState, AppState]),
      NgxsReduxDevtoolsPluginModule.forRoot(),
      NgxsLoggerPluginModule.forRoot()
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
  ],
};
