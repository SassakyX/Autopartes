import { Component } from '@angular/core';
import { AuthService } from '../../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  constructor(private auth: AuthService) {}

  registrar() {
    if (this.usuario.contrasenia !== this.usuario.confirmarContrasenia) {
      this.mensaje = 'Las contraseñas no coinciden';
      return;
    }

      const usuarioEnviar = {
      Nombre_apellido: this.usuario.nombre_apellido,
      DNI: this.usuario.dni,
      Direccion: this.usuario.direccion,
      Correo: this.usuario.correo,
      User: this.usuario.user,
      Contrasenia: this.usuario.contrasenia
      };

    this.auth.register(usuarioEnviar).subscribe({
      next: (res) => {
        this.mensaje = res.mensaje;
        console.log('Registro exitoso:', res);
      },
        error: (err) => {
        console.error("Error completo:", err);
        console.log("Respuesta del back:", err.error);
        if (err.error?.errors) {
          // Convertir el objeto { campo: [mensajes] } en un array plano
          const errores = Object.values(err.error.errors).flat();
          this.mensaje = errores.join(' | ');
        } else if (err.error?.mensaje) {
          this.mensaje = err.error.mensaje;
        } else {
          this.mensaje = 'Ocurrió un error inesperado';
        }
      }
    });
  }
}
