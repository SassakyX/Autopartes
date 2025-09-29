import { Component, OnInit, Pipe } from '@angular/core';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CategoriasServicio,Categorias } from '../../servicios/Categoria/categorias';

@Component({
  selector: 'app-repuestos',
  templateUrl: './repuestos.html',
  styleUrls: ['./repuestos.css'],
  imports: [CommonModule, FormsModule],
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

  constructor(
    private productoservicio: ProductosServicio,
    private categoriasServicio: CategoriasServicio
  ) {}

  ngOnInit(): void {
    // cargar productos iniciales
    this.productoservicio.getTodos().subscribe(data => this.producto = data);

    // cargar categorías
    this.categoriasServicio.getCategorias().subscribe(data => this.categorias = data);
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
}
