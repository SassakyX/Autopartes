import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface Historials {
  idProducto: number;
  productoNombre: string;
  cantidad: number;
  precioUnidad: number;
  subtotal: number;
}
export interface PedidoDTO {
  idVenta: number;
  fecha: string;
  total: number;
  estado: string;
  idUsuario: number;
  usuarioNombre: string;
  usuarioCorreo: string;
  detalles: Historials[];
}

@Injectable({
  providedIn: 'root'
})

export class Historial {
  private base = window.location.origin;
  private apiUrl = `${this.base}/api/Pedidos`;
  //private apiUrl = 'https://sassakyxx-001-site1.jtempurl.com/api/Ventas';

  constructor(private http: HttpClient) {}

  obtenerPorUsuario(idUsuario: number): Observable<PedidoDTO[]> {
    return this.http.get<PedidoDTO[]>(`${this.apiUrl}/usuario/${idUsuario}`);
  }
}
