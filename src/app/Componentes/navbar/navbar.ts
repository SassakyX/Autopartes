import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../servicios/AutServicio/autenticacion';

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
    this.auth.logout();
  }
}
