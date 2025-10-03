import { CarritoServicio } from './../../servicios/Carrito/carrito';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-detalle-producto',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './detalle-producto.html',
  styleUrl: './detalle-producto.css'
})
  export class DetalleProducto {
  producto: Producto | null = null;

  @ViewChild('imagen') imagen!: ElementRef<HTMLImageElement>;

  constructor(
    private route: ActivatedRoute,
    private productosService: ProductosServicio,
    private carritoService: CarritoServicio
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.productosService.getPorId(id).subscribe(p => {
        this.producto = p;
      });
    }
  }

  onMouseMove(event: MouseEvent) {
    if (!this.imagen) return;

    const bounds = this.imagen.nativeElement.getBoundingClientRect();
    const x = ((event.clientX - bounds.left) / bounds.width) * 100;
    const y = ((event.clientY - bounds.top) / bounds.height) * 100;

    this.imagen.nativeElement.style.transformOrigin = `${x}% ${y}%`;
    this.imagen.nativeElement.style.transform = 'scale(1.5)'; // Zoom 2x
  }

  resetZoom() {
    if (!this.imagen) return;

    this.imagen.nativeElement.style.transform = 'scale(1)';
    this.imagen.nativeElement.style.transformOrigin = 'center center';
  }
  agregarAlCarrito() {
  if (this.producto) {
    this.carritoService.agregar(this.producto);
    alert(`${this.producto.nombre} se agregó al carrito`);
  }
}
}


