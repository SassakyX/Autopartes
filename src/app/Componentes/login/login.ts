import { routes } from './../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule,RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  credenciales = {
    User: '',
    Contrasenia: '',


  };
  mensaje = '';

  constructor(private auth: AuthService, private router: Router) {}

  iniciarSesion() {
    this.auth.login(this.credenciales).subscribe({
      next: (res) => {
        this.mensaje = res.mensaje;
        this.router.navigate(['/verificarcod']);
        console.log('Usuario logueado:', "Chismoso uwu");
      },
      error: (err) => {
        if (err.error?.mensaje) {
          this.mensaje = err.error.mensaje;
        } else {
          this.mensaje = 'Error al iniciar sesión';
        }
      }
    });
  }
}
