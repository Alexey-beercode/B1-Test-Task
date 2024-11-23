import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {Location} from "@angular/common";

@Component({
  selector: 'app-back-button',
  template: `
    <button (click)="goBack()" class="back-btn">Назад</button>`,
  standalone: true,
  styleUrls: ['./back-button.component.css']
})
export class BackButtonComponent {
  constructor(private router: Router,
              private location: Location) {}

  goBack() {
    this.location.back();
  }
}
