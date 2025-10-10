import { VerificarCod } from './../verificar-cod/verificar-cod';
import { routes } from './../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  credenciales = {
    User: '',
    Contrasenia: '',
  };
  mensaje = '';

  constructor(private auth: AuthService, private router:Router) {}

  iniciarSesion() {
      this.auth.login(this.credenciales).subscribe({
        next: (res) => {
          if (res?.token) {
            // login completo (sin 2FA)
            localStorage.setItem('token', res.token);
            this.router.navigate(['/']);
          } else if (res?.requiereCodigo) {
            // login con 2FA pendiente
            localStorage.setItem('tempUser', this.credenciales.User);

              Swal.fire({
              icon: 'success',
              title: 'Código de verificacion enviado',
              text: 'Revisa tu correo para iniciar sesión',
              confirmButtonColor: '#28a745',
              timer: 3500,
              timerProgressBar: true,
              showConfirmButton: false
              }).then(() => {
                this.router.navigate(['/verificarcod']);
              });
          }
        },
        error: (err) => {
          console.error(err);
          this.mensaje = err.error?.mensaje || 'Credenciales inválidas';
        }
      });
    }
  }
