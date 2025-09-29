import { Component, signal } from '@angular/core';
import { RouterOutlet} from '@angular/router';
import { Navbar } from './Componentes/navbar/navbar';
import { FormsModule } from '@angular/forms';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Navbar, RouterOutlet, FormsModule],
  styleUrls: ['./app.css'],
  template:`
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
  `
})
export class App {
  protected readonly title = signal('Proyecto');
}
