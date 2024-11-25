import { Routes } from '@angular/router';
import { TaskSelectionComponent } from './features/task-selection/task-selection.component';
import {FirstTaskComponent} from './features/first-task/first-task.component';
import {SecondTaskComponent} from './features/second-task/second-task.component';

export const routes: Routes = [
  { path: 'home', component: TaskSelectionComponent },
  { path: 'first-task', component: FirstTaskComponent },
  {path:'second-task',component: SecondTaskComponent },
  { path: '', redirectTo: 'home', pathMatch: 'full' } // Добавлено перенаправление по умолчанию
];
