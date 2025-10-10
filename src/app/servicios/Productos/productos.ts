import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

export interface Producto {
  idProducto: number;
  nombre: string;
  categoria: string;   // viene del backend con Include
  descrpicion: string;
  precioCompra: number;
  precioVena: number;
  cantidad : number;
  stock: number;
  idCategoria?: number;
  imagenBase64?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductosServicio {
  private base = window.location.origin;
  private apiUrl = `${this.base}/api/Producto`;

  constructor(private http: HttpClient) {}

  // Obtener todos los productos
  getTodos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(this.apiUrl);
  }

  //Obtener productos con filtros
  getProductos(filtros: {
    nombre?: string;
    idCategoria?: number | null;
    precioMin?: number;
    precioMax?: number;
    soloStock?: boolean;
  }): Observable<Producto[]> {
    let params = new HttpParams();

    if (filtros.nombre) params = params.set('nombre', filtros.nombre);
    if (filtros.idCategoria) params = params.set('idCategoria', filtros.idCategoria!);
    if (filtros.precioMin) params = params.set('precioMin', filtros.precioMin);
    if (filtros.precioMax) params = params.set('precioMax', filtros.precioMax);
    if (filtros.soloStock) params = params.set('soloStock', filtros.soloStock);

    return this.http.get<Producto[]>(`${this.apiUrl}/filtrar`, { params });
  }
    crear(producto: FormData): Observable<any> {
    return this.http.post(this.apiUrl, producto);
  }

  editar(id: number, producto: FormData): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, producto);
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
  getPorId(id: number): Observable<Producto> {
  return this.http.get<Producto>(`${this.apiUrl}/${id}`);
}
}
