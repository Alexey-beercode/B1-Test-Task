import { Component } from '@angular/core';
import {Location} from "@angular/common";

@Component({
  selector: 'app-back-button',
  template: `
    <button (click)="goBack()" class="back-btn">Назад</button>`,
  standalone: true,
  styleUrls: ['./back-button.component.css']
})
export class BackButtonComponent {
  constructor(private location: Location) {}

  goBack() {
    this.location.back();
  }
}
