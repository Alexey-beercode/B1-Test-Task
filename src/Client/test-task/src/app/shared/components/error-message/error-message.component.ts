import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-error-message',
  templateUrl: './error-message.component.html',
  standalone: true, // Указываем, что компонент standalone
  imports: [CommonModule], // Добавляем CommonModule
  styleUrls: ['./error-message.component.css']
})
export class ErrorMessageComponent {
  @Input() message: string | null = null;
}
