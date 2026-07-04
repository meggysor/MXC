import { inject, Service } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Event } from '../models/event.model';

@Service()
export class EventService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  getAll(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.baseUrl}/GetAll`);
  }

  getById(id: string): Observable<Event> {
    return this.http.get<Event>(`${this.baseUrl}/GetById`, {
      params: { id }
    });
  }

  create(event: Event): Observable<Event> {
    return this.http.post<Event>(`${this.baseUrl}/Create`, event);
  }

  update(event: Event): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/Update`, event);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/Delete`, {
      params: { id }
    });
  }
}