
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-admin',
  imports: [CommonModule, RouterLink, FormsModule],
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

