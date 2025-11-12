import { VerificarCod } from './../verificar-cod/verificar-cod';
import { routes } from './../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  credenciales = {
    User: '',
    Contrasenia: '',
  };
  mensaje = '';
  procesandoLogin: boolean = false;

  constructor(private auth: AuthService, private router:Router) {}
  flashMensaje() {
      const msgElement = document.querySelector('.alert');
      if (!msgElement) return;

      msgElement.classList.add('flash');
      setTimeout(() => msgElement.classList.remove('flash'), 500);
    }
  iniciarSesion() {
      this.procesandoLogin = true;
      this.mensaje = '';

      const input = this.credenciales.User.trim();
      const credencialesFinal = /^\d+$/.test(input)

      ? { DNI: input, Contrasenia: this.credenciales.Contrasenia }
      : { User: input, Contrasenia: this.credenciales.Contrasenia };


      this.auth.login(credencialesFinal).subscribe({
        next: (res) => {
          this.procesandoLogin = false;
          if (res?.token) {
            // login completo (sin 2FA)
            localStorage.setItem('token', res.token);
            this.router.navigate(['/']);
          } else if (res?.requiereCodigo) {
            // login con 2FA pendiente
          if (/^\d+$/.test(input)) {
            // Es DNI
            localStorage.setItem('tempDNI', input);
            localStorage.removeItem('tempUser');
          } else {
            // Es usuario
            localStorage.setItem('tempUser', input);
            localStorage.removeItem('tempDNI');
          }

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
          this.procesandoLogin = false;
          this.mensaje = err.error?.mensaje || 'Credenciales inválidas';
          this.flashMensaje();
        }
      });

    }

  }
