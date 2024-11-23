import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-selection',
  templateUrl: './task-selection.component.html',
  standalone: true,
  styleUrls: ['./task-selection.component.css']
})
export class TaskSelectionComponent {
  constructor(private router: Router) {}

  goToFirstTask() {
    this.router.navigate(['/first-task']);
  }

  goToSecondTask() {
    this.router.navigate(['/second-task']);
  }
}
