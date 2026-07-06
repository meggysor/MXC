import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Header } from '../../components/header/header';
import { EventService } from '../../services/event';
import { Event } from '../../models/event.model';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    Header
  ],
  templateUrl: './event-form.html',
  styleUrl: './event-form.scss'
})

export class EventForm implements OnInit {
  private readonly eventService = inject(EventService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  isEditMode = signal(false);
  isLoading = signal(false);
  eventId: string | null = null;

  event: Event = {
    name: '',
    location: '',
    country: '',
    capacity: undefined
  };

  ngOnInit(): void {
    this.eventId = this.route.snapshot.paramMap.get('id');

    if (this.eventId) {
      this.isEditMode.set(true);
      this.loadEvent(this.eventId);
    }
  }

  private loadEvent(id: string): void {
    this.isLoading.set(true);
    this.eventService.getById(id).subscribe({
      next: (event) => {
        this.event = event;
        this.isLoading.set(false);
      },
      error: () => {
        this.isLoading.set(false);
        this.snackBar.open('Failed to load event.', 'Close', { duration: 3000 });
        this.router.navigate(['/events']);
      }
    });
  }

  onSubmit(form: any): void {
    if (form.invalid) {
      return;
    }

    this.isLoading.set(true);

    if (this.isEditMode()) {
      this.eventService.update(this.event).subscribe({
        next: () => this.onSaveSuccess('Event updated.'),
        error: () => this.onSaveError()
      });
    } else {
      this.eventService.create(this.event).subscribe({
        next: () => this.onSaveSuccess('Event created.'),
        error: () => this.onSaveError()
      });
    }
  }

  private onSaveSuccess(message: string): void {
    this.isLoading.set(false);
    this.snackBar.open(message, 'Close', { duration: 2000 });
    this.router.navigate(['/events']);
  }

  private onSaveError(): void {
    this.isLoading.set(false);
    this.snackBar.open('Something went wrong. Please try again.', 'Close', { duration: 3000 });
  }

  onCancel(): void {
    this.router.navigate(['/events']);
  }
}