import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Events } from './pages/events/events';
import { EventForm } from './pages/event-form/event-form';
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';

export const routes: Routes = [
  { path: 'login', component: Login, canActivate: [guestGuard] },
  { path: 'events', component: Events, canActivate: [authGuard] },
  { path: 'events/new', component: EventForm, canActivate: [authGuard] },
  { path: 'events/edit/:id', component: EventForm, canActivate: [authGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];