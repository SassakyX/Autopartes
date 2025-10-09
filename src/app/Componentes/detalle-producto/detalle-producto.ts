import { CarritoServicio } from './../../servicios/Carrito/carrito';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';

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
  cantidad: number = 1;
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
        this.cantidad = p.stock > 0 ? 1 : 0;
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

  soloNumeros(event: KeyboardEvent) {
    // permite solo números (teclas 0-9)
    const charCode = event.which ? event.which : event.keyCode;
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
    }
  }
  validarCantidad() {
    if (!this.producto) return;

    if (this.producto.stock <= 0) {
      Swal.fire('Sin stock', 'Este producto no tiene unidades disponibles.', 'warning');
      this.cantidad = 0;
      return;
    }

    if (this.cantidad < 1) {
      Swal.fire('Cantidad inválida', 'Debes ingresar una cantidad válida.', 'warning');
      this.cantidad = 1;
    }

    if (this.cantidad > this.producto.stock) {
      Swal.fire('Stock insuficiente', `Solo hay ${this.producto.stock} unidades disponibles.`, 'info');
      this.cantidad = this.producto.stock;
    }
  }

  agregarAlCarrito() {


  if (!this.producto) {
    alert("Error: no se ha cargado el producto.");
    return;
  }

  if (!this.cantidad || this.cantidad < 1) {
    alert("Debes ingresar una cantidad válida.");
    return;
  }
  if (this.cantidad > this.producto.stock) {
    alert(`Solo hay ${this.producto.stock} unidades disponibles.`);
    this.cantidad = this.producto.stock;
    return;
    }

  if (this.producto && this.cantidad > 0) {
    const productocantidad = {
      ...this.producto,
      cantidad: this.cantidad
    };
    this.carritoService.agregar(productocantidad);
    alert(`${this.producto.nombre} x${this.cantidad} se agregó al carrito`);
  } else {
    alert("Por favor, ingresa una cantidad válida antes de agregar al carrito.");
  }
}
}


