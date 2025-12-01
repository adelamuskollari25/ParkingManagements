import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const roleGuard = (allowedRoles: string[]): CanActivateFn => {
  return (route, state) => {
    const auth = inject(AuthService);
    const router = inject(Router);

    const role = auth.getRole();

    if (!role || !allowedRoles.includes(role)) {
      router.navigate(['/dashboard']);
      return false;
    }
    return true;
  }
}
