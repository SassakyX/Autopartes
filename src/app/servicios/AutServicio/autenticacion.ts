import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap} from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private base = window.location.origin;
  private apiUrl = `${this.base}/api/Auto`;;


  private usuarioSubject = new BehaviorSubject<any>(this.getUsuario());
    usuario$ = this.usuarioSubject.asObservable();
  constructor(private http: HttpClient) {}

  private safeGet(key: string): string | null {
    return (typeof window !== 'undefined' && localStorage)
      ? localStorage.getItem(key)
      : null;
  }

  private safeSet(key: string, value: string): void {
    if (typeof window !== 'undefined' && localStorage) {
      localStorage.setItem(key, value);
    }
  }
  private safeRemove(key: string): void {
    if (typeof window !== 'undefined' && localStorage) {
      localStorage.removeItem(key);
    }
  }

  register(usuario: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, usuario);
  }

  login(credenciales: { User: string; Contrasenia: string }) {
    return this.http.post<any>(`${this.apiUrl}/login`, credenciales).pipe(
      tap((res) => {
        this.safeSet('tempUser', credenciales.User);
        localStorage.setItem('rol', res.rol);
      })
    );
  }

  isLoggedIn(): boolean {
    return !!this.safeGet('token');
  }

  getUsuario() {
    const data = this.safeGet('usuario');
    return data ? JSON.parse(data) : null;
  }

  setUsuario(usuario: any) {
    this.safeSet('usuario', JSON.stringify(usuario));
    this.usuarioSubject.next(usuario);
  }


verificarCodigo(data: { User: string; Codigo: string }) {
  return this.http.post<any>(`${this.apiUrl}/verificar-codigo`, data);
}


  logout() {
    this.safeRemove('usuario');
    this.safeRemove('token');
    this.safeRemove('tempUser');
    localStorage.removeItem('rol');
    this.usuarioSubject.next(null);
  }
}

