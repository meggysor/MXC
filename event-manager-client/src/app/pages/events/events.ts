import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Header } from '../../components/header/header';
import { EventService } from '../../services/event';
import { Event } from '../../models/event.model';
@Component({
  selector: 'app-events',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    Header
  ],
  templateUrl: './events.html',
  styleUrl: './events.scss'
})
export class Events implements OnInit {
  private readonly eventService = inject(EventService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);
  displayedColumns: string[] = ['name', 'location', 'capacity', 'actions'];
  dataSource = signal<Event[]>([]);
  isLoading = signal(false);
  hasLoaded = signal(false);
  ngOnInit(): void {
    this.loadEvents();
  }
  loadEvents(): void {
    this.isLoading.set(true);
    this.eventService.getAll().subscribe({
      next: (events) => {
        this.dataSource.set(events);
        this.isLoading.set(false);
        this.hasLoaded.set(true);
      },
      error: () => {
        this.isLoading.set(false);
        this.hasLoaded.set(true);
        this.snackBar.open('Failed to load events.', 'Close', { duration: 3000 });
      }
    });
  }
  onSortChange(sort: Sort): void {
    const data = [...this.dataSource()];
    if (!sort.active || sort.direction === '') {
      this.dataSource.set(data);
      return;
    }
    const isAsc = sort.direction === 'asc';
    data.sort((a, b) => {
      switch (sort.active) {
        case 'name': return this.compare(a.name, b.name, isAsc);
        case 'location': return this.compare(a.location, b.location, isAsc);
        case 'capacity': return this.compare(a.capacity ?? 0, b.capacity ?? 0, isAsc);
        default: return 0;
      }
    });
    this.dataSource.set(data);
  }
  private compare(a: string | number, b: string | number, isAsc: boolean): number {
    return (a < b ? -1 : a > b ? 1 : 0) * (isAsc ? 1 : -1);
  }
  onAddNew(): void {
    this.router.navigate(['/events/new']);
  }
  onEdit(event: Event): void {
    this.router.navigate(['/events/edit', event.id]);
  }
  onDelete(event: Event): void {
    if (!confirm(`Are you sure you want to delete "${event.name}"?`)) return;
    this.isLoading.set(true);
    this.eventService.delete(event.id!).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.snackBar.open('Event deleted.', 'Close', { duration: 2000 });
        this.loadEvents();
      },
      error: () => {
        this.isLoading.set(false);
        this.snackBar.open('Failed to delete event.', 'Close', { duration: 3000 });
      }
    });
  }
}