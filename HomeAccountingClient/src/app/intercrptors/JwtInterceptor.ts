import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';

import {take} from "rxjs/operators";
import { AuthenticationService } from '../shared/services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private authenticationService : AuthenticationService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let isAuthenticated : boolean;

    this.authenticationService.authChanged.pipe(take(1))
      .subscribe(res => isAuthenticated = res);

    if (isAuthenticated)
    {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${localStorage.getItem("token")}`
        }
      });

    }
    return next.handle(request);
  }
}
