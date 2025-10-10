import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GatoServicio {
private apiUrl = 'https://api.thecatapi.com/v1/images/search';

  constructor(private http: HttpClient) {}

  getGatoRandom(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
