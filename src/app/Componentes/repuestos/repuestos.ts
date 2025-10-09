import { Component, OnInit, Pipe } from '@angular/core';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoriasServicio,Categorias } from '../../servicios/Categoria/categorias';
import { RouterLink } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-repuestos',
  templateUrl: './repuestos.html',
  styleUrls: ['./repuestos.css'],
  imports: [CommonModule, FormsModule, RouterLink],
  standalone : true
})

export class Repuestos implements OnInit {
  producto: Producto[] = [];
  categorias: Categorias[] = [];

  // Filtros
  filtroNombre = '';
  categoriaSeleccionada: number | null = null;
  precioMin?: number;
  precioMax?: number;
  soloStock = false;
  busqueda$ = new Subject<string>();

  constructor(
    private productoservicio: ProductosServicio,
    private categoriasServicio: CategoriasServicio
  ) {}

  ngOnInit(): void {
    // cargar productos iniciales
    this.productoservicio.getTodos().subscribe(data => this.producto = data);

    // cargar categorías
    this.categoriasServicio.getCategorias().subscribe(data => this.categorias = data);
    this.busqueda$.pipe(debounceTime(300)).subscribe(valor => {
    this.filtroNombre = valor;
    this.aplicarFiltros();
  });
  }
  aplicarFiltros(): void {
    this.productoservicio.getProductos({
      nombre: this.filtroNombre,
      idCategoria: this.categoriaSeleccionada,
      precioMin: this.precioMin,
      precioMax: this.precioMax,
      soloStock: this.soloStock

    }).subscribe({
      next: data => this.producto = data,
      error: err => console.error('Error en aplicar filtros', err)
    });
  }

  limpiarFiltros(): void {
  this.filtroNombre = '';
  this.categoriaSeleccionada = null;
  this.precioMin = undefined;
  this.precioMax = undefined;
  this.soloStock = false;

  // Recarga los productos originales
  this.productoservicio.getTodos().subscribe(data => this.producto = data);
  }
}
