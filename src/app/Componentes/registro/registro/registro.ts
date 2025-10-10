import { routes } from './../../../app.routes';
import { Component } from '@angular/core';
import { AuthService } from '../../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { VerificarCod } from '../../verificar-cod/verificar-cod';
import { tap } from 'node:test/reporters';
import Swal from 'sweetalert2';
import { privateDecrypt } from 'crypto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './registro.html',
  styleUrls: ['./registro.css']
})
export class Registro {
    usuario = {
    nombre_apellido: '',
    dni: null,
    direccion: '',
    correo: '',
    user: '',
    contrasenia: '',
    confirmarContrasenia: ''
}
  mensaje: string = '';

  constructor(private auth: AuthService, private router: Router) {}

  registrar() {
    if (this.usuario.contrasenia !== this.usuario.confirmarContrasenia)  {
      this.mensaje = 'Las contraseñas no coinciden';
      return;
    }

      const usuarioEnviar = {
      Nombre_apellido: this.usuario.nombre_apellido,
      DNI: this.usuario.dni,
      Direccion: this.usuario.direccion,
      Correo: this.usuario.correo,
      User: this.usuario.user,
      Contrasenia: this.usuario.contrasenia,
      Rol: 'cliente'

      };

      this.auth.register(usuarioEnviar).subscribe({
      next: (res) => {
        localStorage.setItem('tempUser', this.usuario.user);

          Swal.fire({
          icon: 'success',
          title: 'Código de verificacion enviado',
          text: res.mensaje,
          confirmButtonColor: '#28a745',
          timer: 3500,
          timerProgressBar: true,
          showConfirmButton: false
        });

       setTimeout(() => {
          this.router.navigate(['/verificarcod']);
        }, 2000);
      },
      error: (err) => {
        console.error('Error completo:', err);
        const mensaje = err.error?.mensaje || 'Ocurrió un error inesperado';
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: mensaje,
          confirmButtonColor: '#d33'
        });
      }
    });
  }
}
