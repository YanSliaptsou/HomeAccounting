import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../shared/services/authentication.service';
import { AuthResponseDto } from '../_interfaces/AuthResponseDto';
import { UserResponseDto } from '../_interfaces/UsersInterfaces/UserResponseDto';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public isUserAuthenticated: boolean;
  public appUser : UserResponseDto = {
    mainCurrencyId : null,
    userName : null
  };

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged
  .subscribe(res => {
    this.isUserAuthenticated = res;
  })
   }


  ngOnInit(): void {
    if(this.isUserAuthenticated){
      this.getUser();
    }
  }

  public logout = () => {
    this.authService.logout();
    this.router.navigate(["/authentication/login"]);
  }

  public getUser(){
    this.authService.getUser().subscribe((response : any) => {
      this.appUser = response.data;
    })
  }

}
