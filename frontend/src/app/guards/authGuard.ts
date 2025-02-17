import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { Observable, of } from 'rxjs';
import { catchError, map, switchMap, take } from 'rxjs/operators';

import { CheckAuth } from '../store/app.actions';
import { UserState } from '../store/app.state';

export const authGuard = (roles?: string[]): Observable<boolean> => {
    const store = inject(Store);
    const router = inject(Router);


    return store.dispatch(new CheckAuth()).pipe(
        switchMap(() =>
            store.select(UserState.currentUser).pipe(
                take(1),
                map((currentUser) => {
                    if ((currentUser && !roles) || (currentUser?.role && roles?.includes(currentUser.role))) {
                        return true;
                    } else {
                        router.navigate(['/login']);
                        return false;
                    }
                })
            )
        ),
        catchError(() => {
            router.navigate(['/login']);
            return of(false);
        })
    );
};