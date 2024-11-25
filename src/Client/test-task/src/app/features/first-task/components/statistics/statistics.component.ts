import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatisticsService } from '../../../../services/statistics.service';
import { LoadingSpinnerComponent } from '../../../../shared/components/loading-spinner/loading-spinner.component';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message.component';
import { Statistics } from '../../../../models/responses/statistics.response';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    LoadingSpinnerComponent,
    ErrorMessageComponent
  ]
})
export class StatisticsComponent implements OnInit {
  loading = false;
  errorMessage: string | null = null;
  statistics: Statistics | null = null;

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit(): void {
    this.loadStatistics();
  }

  loadStatistics(): void {
    this.loading = true;
    this.errorMessage = null;

    this.statisticsService.getStatistics().subscribe({
      next: (data) => {
        this.statistics = data;
        this.loading = false;
      },
      error: (error) => {
        this.errorMessage = 'Ошибка при загрузке статистики';
        this.loading = false;
      }
    });
  }
}
