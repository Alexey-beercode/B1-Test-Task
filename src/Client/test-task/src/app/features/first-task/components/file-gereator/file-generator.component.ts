import {Component, EventEmitter, Output,OnInit,ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common'; // Добавьте этот импорт
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import {FileGeneratorService} from '../../../../services/file-generator.service';

@Component({
  selector: 'app-file-generator',
  templateUrl: './file-generator.component.html',
  standalone: true,
  imports: [
    CommonModule, // Добавьте этот модуль в imports
    LoadingSpinnerComponent,
    ErrorMessageComponent
  ]
})
export class FileGeneratorComponent {
  loading = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private fileGeneratorService: FileGeneratorService,private cdr: ChangeDetectorRef) {

  }
  @Output() generationComplete = new EventEmitter<boolean>();
  @Output() loadingChange = new EventEmitter<boolean>();
  @Output() errorChange = new EventEmitter<string | null>();

  ngOnInit(): void {
    this.generateFiles(); // Вызываем метод при инициализации

  }
  generateFiles(): void {
    console.log('Метод generateFiles вызван');
    this.loading = true;
    this.errorMessage = null;
    this.loadingChange.emit(true);
    this.errorChange.emit(null);

    this.fileGeneratorService.generateFiles().subscribe({
      next: () => {
        this.loading = false;
        this.successMessage = 'Файлы успешно сгенерированы';
        this.loadingChange.emit(false);
        this.generationComplete.emit(true);
        console.log('Файлы сгенерированы успешно');
      },
      error: (err) => {
        console.log('Ошибка при генерации файлов:', err);
        this.loading = false;
        this.errorMessage = 'Ошибка при генерации файлов.';
        this.loadingChange.emit(false);
        this.errorChange.emit(this.errorMessage);
        this.generationComplete.emit(false);
        this.cdr.detectChanges();
      }
    });
  }
}
