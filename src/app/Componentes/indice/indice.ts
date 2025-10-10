import { Component, inject, OnInit } from '@angular/core';
import { GatoServicio } from '../../servicios/Gato/gato-servicio';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-indice',
  standalone:true,
  imports: [CommonModule],
  templateUrl: './indice.html',
  styleUrls: ['./indice.css']
})
export class Indice implements OnInit{
 gatos: string[] = [];

  //  para los gatos w
  private gatosService = inject(GatoServicio);

  ngOnInit(): void {
    this.cargarVariosGatos(8);
  }

 cargarVariosGatos(cantidad: number) {
    this.gatos = [];
    for (let i = 0; i < cantidad; i++) {
      this.gatosService.getGatoRandom().subscribe(res => {
        this.gatos.push(res[0].url);
      });
    }
  }

  cambiarUno(index: number) {
    // reemplaza solo el gato de esa posición
    this.gatosService.getGatoRandom().subscribe(res => {
      this.gatos[index] = res[0].url;
    });
  }
}
