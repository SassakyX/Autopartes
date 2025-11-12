import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


export interface ReseñaDto {
  productoId: number;
  usuarioId: number;
  estrellas: number;
  comentario?: string;
}
@Injectable({
  providedIn: 'root'

})
export class ResenaServicio {
  private base = window.location.origin;
 //private apiUrl = `https://sassakyxx-001-site1.jtempurl.com/api/Resenas`;
  private apiUrl = `${this.base}/api/Resenas`;

  constructor(private http: HttpClient) { }

  crearReseña(dto: ReseñaDto): Observable<any> {
    return this.http.post(this.apiUrl, dto);
  }

  obtenerReseñas(productoId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${productoId}`);
  }

  haCompradoProducto(usuarioId: number, productoId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/haComprado?usuarioId=${usuarioId}&productoId=${productoId}`);
  }
}
