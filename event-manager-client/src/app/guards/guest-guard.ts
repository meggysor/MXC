import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth';

/// <summary>
/// Prevents already-logged-in users from accessing the login page and redirects them straight to the events list instead.
/// </summary>
export const guestGuard: CanActivateFn = (route, state) => {
  const auth = inject(Auth);
  const router = inject(Router);

  if (!auth.isLoggedIn()) {
    return true;
  }

  router.navigate(['/events']);
  return false;
};