import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VentasService {
  private apiUrl = 'http://sassakyxx-001-site1.jtempurl.com/api/ventas';

  constructor(private http: HttpClient) {}

  crearVenta(venta: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/crear`, venta);
  }

  // Obtener todas las ventas
  getVentas(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtener ventas por usuario
  getVentasPorUsuario(idUsuario: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/usuario/${idUsuario}`);
  }

  // Cambiar estado de una venta
  cambiarEstado(idVenta: number, nuevoEstado: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${idVenta}/estado`, JSON.stringify(nuevoEstado), {
      headers: { 'Content-Type': 'application/json' }
    });
  }
}
