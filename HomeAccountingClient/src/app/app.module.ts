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

import { JwtModule} from "@auth0/angular-jwt"
import { AuthGuard } from './shared/guards/auth.guard';
import { JwtInterceptor } from './intercrptors/JwtInterceptor';
import { LoadingInterceptor } from './intercrptors/LoadingInterceptor';
import { ParentCategoriesComponent } from './parent-categories/parent-categories/parent-categories.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs'
import { ProgressbarModule } from 'ngx-bootstrap/progressbar'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { AccountsComponent } from './accounts/accounts.component';
import { CommonModule } from '@angular/common';
import { LedgersComponent } from './ledgers/ledgers.component';

export function tokenGetter() {
  return localStorage.getItem("token");
}


@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    HomeComponent,
    NotfoundComponent,
    ParentCategoriesComponent,
    AccountsComponent,
    LedgersComponent,
  ],
  imports: [
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:5001"],
        disallowedRoutes: []
      }
    }),
    BrowserModule,
    NgbModule,
    HttpClientModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    FormsModule,
    CommonModule,
    ProgressbarModule.forRoot(),
    RouterModule.forRoot([
      { path: 'home', component: HomeComponent, canActivate:[AuthGuard] },
      { path: '404', component: NotfoundComponent},
      { path: 'authentication', loadChildren: () => import('./authentification/authentification.module').then(m => m.AuthentificationModule) },
      { path: 'parent-categories', component: ParentCategoriesComponent, canActivate: [AuthGuard]},
      { path: 'accounts', component: AccountsComponent, canActivate: [AuthGuard]},
      { path: 'ledgers', component: LedgersComponent, canActivate: [AuthGuard]},
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: '**', redirectTo: '/404', pathMatch: 'full'}
    ]),
    BrowserAnimationsModule,
    
  ],

  providers: [{provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerService,multi: true},
              {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
              {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
