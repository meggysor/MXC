import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  email = '';
  password = '';
  isLoading = false;

  onSubmit(): void {
    if (!this.email || !this.password) {
      return;
    }

    this.isLoading = true;

    this.auth.login({ email: this.email, passWord: this.password }).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/events']);
      },
      error: () => {
        this.isLoading = false;
        this.snackBar.open('Invalid email or password.', 'Close', { duration: 3000 });
      }
    });
  }
}