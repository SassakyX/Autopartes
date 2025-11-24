import { Component, signal } from '@angular/core';
import { RouterOutlet} from '@angular/router';
import { Navbar } from './Componentes/navbar/navbar';
import { FormsModule } from '@angular/forms';
import { Footer } from './Footer/footer/footer';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Navbar, RouterOutlet, FormsModule, Footer],
  styleUrls: ['./app.css'],
  template:`
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
    <app-footer></app-footer>
  `
})
export class App {
  protected readonly title = signal('Proyecto');
}
