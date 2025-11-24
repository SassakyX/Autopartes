import { Component, inject, OnInit } from '@angular/core';
import { GatoServicio } from '../../servicios/Gato/gato-servicio';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-indice',
  standalone:true,
  imports: [CommonModule, RouterLink, RouterModule],
  templateUrl: './indice.html',
  styleUrls: ['./indice.css']
})
export class Indice implements OnInit{
  ngOnInit(): void {
  }
  constructor (private router : Router){}

  irARepuestos(idcategoria: number) {
  this.router.navigate(['/repuestos'], { queryParams: { categoria: idcategoria } });
  }
}
