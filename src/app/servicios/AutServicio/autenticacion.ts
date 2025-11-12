import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap} from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private base = window.location.origin;
  //private apiUrl = `https://sassakyxx-001-site1.jtempurl.com/api/Auto`;
  private apiUrl = `${this.base}/api/Auto`;


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

  login(credenciales: { User?:string; DNI?: string; Contrasenia: string }) {
    return this.http.post<any>(`${this.apiUrl}/login`, credenciales).pipe(
      tap((res) => {
        const identificador = credenciales.User || credenciales.DNI;
        this.safeSet('tempUser', identificador!);
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


verificarCodigo(data: { User?: string; DNI?: string; Codigo: string }) {
  //return this.http.post<any>(`${this.apiUrl}/verificar-codigo`, data).pipe(
  return this.http.post<any>(`${this.apiUrl}/verificar-codigo`, data).pipe(
    tap((res) => {
      if (res.usuario) {
        // Guardamos usuario y token reales
        this.safeSet('usuario', JSON.stringify(res.usuario));
        this.safeSet('token', res.token);
        localStorage.setItem('rol', res.usuario.rol);

        // Actualizamos el BehaviorSubject para que Angular lo detecte
        this.usuarioSubject.next(res.usuario);
      }
    })
  );
}


  logout() {
    this.safeRemove('usuario');
    this.safeRemove('token');
    this.safeRemove('tempUser');
    this.safeRemove('tempDNI')
    localStorage.removeItem('rol');
    this.usuarioSubject.next(null);
  }
}

