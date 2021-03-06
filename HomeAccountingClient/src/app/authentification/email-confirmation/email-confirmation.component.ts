import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-email-confirmation',
  templateUrl: './email-confirmation.component.html',
  styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent implements OnInit {

  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;

  constructor(private authService: AuthenticationService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.confirmEmail();
  }

  private confirmEmail = () => {
    this.showError = this.showSuccess = false;
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];

    this.authService.confirmEmail('api/users-accounts/emailconfirmation', token, email)
    .subscribe(response => {
        this.showSuccess = true
      }, error => {
        this.showError = true;
        this.errorMessage = error;
      })
  }

}
