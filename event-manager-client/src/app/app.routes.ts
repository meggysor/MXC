import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Events } from './pages/events/events';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'events', component: Events },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', redirectTo: '/login' }
];