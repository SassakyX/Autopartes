import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verificar-cod',
  standalone : true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verificar-cod.html',
  styleUrl: './verificar-cod.css'
})

export class VerificarCod {
    codigo = '';
    mensaje = '';
    error = '';
    constructor(private auth: AuthService, private router: Router) {}
    Verificar()
    {
    const user= localStorage.getItem('tempUser');
    const dni = localStorage.getItem('tempDNI')
    if (!user && !dni) { this.error ="No se detecto el usuario temporal.";
      return;
    }

    const tempUser = localStorage.getItem('tempUser') || undefined;
    const tempDNI = localStorage.getItem('tempDNI') || undefined;
    this.auth.verificarCodigo({User: tempUser, DNI: tempDNI, Codigo: this.codigo }).subscribe({
      next: (res) => {
        this.mensaje = res.mensaje;
        this.error = '';
        // guarda usuario y token
        this.auth.setUsuario(res.usuario);
        localStorage.setItem('token', res.token);
        localStorage.removeItem('tempUser');
        setTimeout(() => this.router.navigate(['/']), 1000);
      },
      error: (err) => {
        console.error(err);
        this.error = err.error?.mensaje || 'Error al verificar el código';
      }
    });
  }
}
