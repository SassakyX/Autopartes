import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Producto } from '../Productos/productos';


@Injectable({
  providedIn: 'root'
})
export class CarritoServicio {

 private items: Producto[] = [];
  private itemsSubject = new BehaviorSubject<Producto[]>([]);

  items$ = this.itemsSubject.asObservable();

  agregar(producto: Producto) {
    this.items.push(producto);
    this.itemsSubject.next(this.items);
  }

  eliminar(index: number) {
    this.items.splice(index, 1);
    this.itemsSubject.next(this.items);
  }

  limpiar() {
    this.items = [];
    this.itemsSubject.next(this.items);
  }

  obtenerItems(): Producto[] {
    return this.items;
  }
}
