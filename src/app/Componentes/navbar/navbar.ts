import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../servicios/AutServicio/autenticacion';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-navbar',
  standalone:true,
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.html',
  styleUrls: ['./navbar.css']
})
export class Navbar implements OnInit{
  usuario:any = null;

  constructor(private auth: AuthService) {}

  ngOnInit() {
  this.auth.usuario$.subscribe(u => {
      this.usuario = u;
    console.log("Navbar detectó un usuario: ", u);
  });
}
  esAdmin(): boolean {
  return this.usuario?.rol === 'Admin';

  }
  cerrarSesion() {
  Swal.fire({
    icon: 'info',
    title: 'Sesión cerrada',
    text: 'Has cerrado sesión correctamente',
    confirmButtonColor: '#3085d6',
    timer: 2000,
    timerProgressBar: true,
    showConfirmButton: false
    }).then(() => {
    window.location.href = '/'; // o this.router.navigate(['/']);
    });
    this.auth.logout();
  }
}
