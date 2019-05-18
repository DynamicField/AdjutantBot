import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginwithdiscordComponent } from './components/authorization/loginwithdiscord/loginwithdiscord.component';
import { LoginComponent } from './views/authorization/login/login.component';
import { PagenotfoundComponent } from './views/misc/pagenotfound/pagenotfound.component';
import { CallbackComponent } from './views/authorization/callback/callback.component';
import { CookieService } from 'ngx-cookie-service';
import {AuthInterceptor} from './interceptors/auth-interceptor';

const appRoutes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'login/callback', component: CallbackComponent },
  { path: '**', component: PagenotfoundComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    LoginwithdiscordComponent,
    LoginComponent,
    PagenotfoundComponent,
    CallbackComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [CookieService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
