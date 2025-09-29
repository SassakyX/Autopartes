import { Component } from '@angular/core';
import { RouterOutlet} from '@angular/router';
import { CommonModule } from '@angular/common';
import { Unsubscribable } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-admin',
  imports: [CommonModule, RouterLink],
  templateUrl: './admin.html',
  styleUrl: './admin.css'
})
export class Admin {
  constructor(public auth: AuthService, private router : Router) {}

  cerrarSesion() {
  this.auth.logout();
  alert("Sesión cerrada correctamente");
  this.router.navigate(['/']);
  }
}
