import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';


export interface Categorias {
  idCategoria: number;
  nombre: string;
}
@Injectable({
  providedIn: 'root'
})
export class CategoriasServicio {
  private base = window.location.origin;
  private apiUrl = `${this.base}/api/Categoria`;

  constructor(private http: HttpClient) {}

  getCategorias(): Observable<Categorias[]> {
    return this.http.get<Categorias[]>(this.apiUrl);
  }
  crear(nombre: string): Observable<Categorias> {
    return this.http.post<Categorias>(this.apiUrl, { nombre });
  }

  editar(id: number, nombre: string): Observable<Categorias> {
    return this.http.put<Categorias>(`${this.apiUrl}/${id}`, { nombre });
  }

  eliminar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
