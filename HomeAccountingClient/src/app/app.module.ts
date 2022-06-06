import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NotfoundComponent } from './notfound/notfound.component';
import { ErrorHandlerService } from './shared/services/error-handler.service';

import {JwtModule} from "@auth0/angular-jwt"
import { AuthGuard } from './shared/guards/auth.guard';

export function tokenGetter() {
  return localStorage.getItem("token");
}


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    HomeComponent,
    NotfoundComponent,
  ],
  imports: [
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {return tokenGetter()},
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    BrowserModule,
    NgbModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent, canActivate:[AuthGuard] },
      { path: '404', component: NotfoundComponent},
      { path: 'authentication', loadChildren: () => import('./authentification/authentification.module').then(m => m.AuthentificationModule) },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: '**', redirectTo: '/404', pathMatch: 'full'}
    ]),
    
  ],

  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorHandlerService,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
