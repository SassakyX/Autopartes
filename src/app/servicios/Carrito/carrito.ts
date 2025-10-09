import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Producto } from '../Productos/productos';


@Injectable({
  providedIn: 'root'
})
export class CarritoServicio {

  private items: Producto[] = [];
  private itemsSubject = new BehaviorSubject<Producto[]>([]);

  constructor() {
    const guardado = localStorage.getItem('carrito');
    if (guardado) {
      this.items = JSON.parse(guardado);
      this.itemsSubject.next(this.items);
    }
  }
  items$ = this.itemsSubject.asObservable();
  private guardar() {
    localStorage.setItem('carrito', JSON.stringify(this.items));
  }

  agregar(producto: any) {
    const existente = this.items.find(p => p.idProducto === producto.idProducto);

    if (existente)
    {
      existente.cantidad += producto.cantidad;
    }
    else
    {
      this.items.push({...producto, cantidad: producto.cantidad || 1 })
    }
    this.guardar();
    this.itemsSubject.next(this.items);
  }

  eliminar(index: number) {
    this.items.splice(index, 1);
    this.itemsSubject.next(this.items);
    this.guardar();
  }

  limpiar() {
    this.items = [];
    this.itemsSubject.next(this.items);
    this.guardar();
  }

  obtenerItems(): Producto[] {
    return this.items;
  }
  getTotal(): number {
  return this.items.reduce(
    (total, item) => total + (item.precioVena * item.cantidad),
    0
  );
  }
  actualizarCarrito(itemsActualizados: any[]) {
  this.items = itemsActualizados;
  this.guardar();
  this.itemsSubject.next(this.items);
}
}
