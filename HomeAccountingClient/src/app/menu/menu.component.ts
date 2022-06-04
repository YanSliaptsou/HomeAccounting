import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../shared/services/authentication.service';
import { AuthResponseDto } from '../_interfaces/AuthResponseDto';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  public isUserAuthenticated: boolean;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged
  .subscribe(res => {
    this.isUserAuthenticated = res;
  })
   }

  ngOnInit(): void {
    
  }

  public logout = () => {
    this.authService.logout();
    this.router.navigate(["/authentication/login"]);
  }

}
