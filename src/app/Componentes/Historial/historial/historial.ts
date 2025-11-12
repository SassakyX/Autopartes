import { Component, OnInit } from '@angular/core';
import { PedidoDTO, Historial } from '../../../servicios/Historial/historial';
import { DatePipe, NgClass, NgFor, NgIf, CurrencyPipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-historial',
  standalone : true,
  imports: [NgIf, NgFor, NgClass, DatePipe, DecimalPipe],
  templateUrl: './historial.html',
  styleUrl: './historial.css'
})
export class HistorialC implements OnInit {


 pedidos: PedidoDTO[] = [];
  loading = true;

  constructor(private historialservicio: Historial) {}

  ngOnInit(): void {
    const Usuariostring = localStorage.getItem('usuario');
    if (!Usuariostring) {
      console.error('No hay usuario logueado');
      this.loading = false;
      return;
    }
    const usuario = JSON.parse(Usuariostring);
    const idUsuario = usuario.idUsuario;

    if (!idUsuario) {
    console.error('No se encontró idUsuario en el objeto usuario');
    this.loading = false;
    return;
    }


    this.historialservicio.obtenerPorUsuario(idUsuario).subscribe({
      next: (data) => {
        this.pedidos = data.sort((a, b) => new Date(b.fecha).getTime() - new Date(a.fecha).getTime());
        this.loading = false;
      },
      error: (err) => {
        console.error('Error al cargar pedidos:', err);
        this.loading = false;
      }
    });
  }
}
