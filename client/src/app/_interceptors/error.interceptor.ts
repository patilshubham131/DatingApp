import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastrService: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(error=>{
        if(error){
          switch (error.status) {
            case 400:
              if(error.error.errors){
                const modelStateErrors = [];
                for(const key in error.error.errors){
                  if(error.error.errors[key]){
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                throw modelStateErrors.flat();
              }
              else{
                this.toastrService.error("Bad Request", error.status);
              }
              break;
            case 401:
              this.toastrService.error("Unauthrized", error.status);
              break;
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            case 500:
              const navigationExtas: NavigationExtras = {state:{
                error:error.error
              }}
              this.router.navigateByUrl('/server-error', navigationExtas);
              break;
            default:
              this.toastrService.error("something went wrong..");
              break;
          }
        }
        return throwError(error);
      })
    );
  }
}
