import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);

  get userEmail(): string | null {
    return this.auth.getEmail();
  }

  onLogout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}