import { State, Action, StateContext, Selector, Store } from '@ngxs/store';
import { LogsService } from '../services/logs-service/logs.service';
import { GetLogs } from './logs.actions';
import { DataByPagination, Log } from '../interfaces';
import { Injectable } from '@angular/core';
import { catchError, tap, finalize } from 'rxjs/operators';
import { of } from 'rxjs';
import { SetLoading } from './app.actions';

export interface LogsStateModel {
  logs: DataByPagination<Log[]>;
  error: string | null;
}

@State<LogsStateModel>({
  name: 'logs',
  defaults: {
    logs: { data: [], total: 0 },
    error: null,
  }
})
@Injectable()
export class LogsState {
  constructor(private readonly logsService: LogsService, private readonly store: Store) { }

  @Selector()
  static logs(state: LogsStateModel): DataByPagination<Log[]> {
    return state.logs;
  }

  @Action(GetLogs)
  public getLogs(
    ctx: StateContext<LogsStateModel>,
    { paginationParams }: GetLogs
  ) {
    this.store.dispatch(new SetLoading(true));
    ctx.patchState({ error: null });

    return this.logsService.getLogs(paginationParams).pipe(
      tap((response) => {
        ctx.patchState({
          logs: response
        });
      }),
      catchError((error) => {
        ctx.patchState({ error: error.message });
        return of(error);
      }), finalize(() => {
        this.store.dispatch(new SetLoading(false));
      })
    );
  }
}
