import { Categorias,CategoriasServicio } from './../../servicios/Categoria/categorias';
import { CommonModule } from '@angular/common';
import { ProductosServicio } from './../../servicios/Productos/productos';
import { Component, OnInit } from '@angular/core';
import { Producto } from '../../servicios/Productos/productos';
import { FormsModule, NgModel } from '@angular/forms';
import { RouterModule } from "@angular/router";
import Swal from 'sweetalert2';
import { response } from 'express';

@Component({
  selector: 'app-admin-productos',
  standalone : true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './admin-productos.html',
  styleUrls: ['./admin-productos.css']
})


export class AdminProductos implements OnInit{
  productos: Producto[] = [];
  seleccionado: Producto | null = null;
  archivo: File | null = null;
  categorias: Categorias[] = [];


  nuevo: {
    nombre: string;
    descrpicion: string;
    precioCompra: number;
    precioVena: number;
    stock: number;
    idCategoria: number;
  } = {
    nombre: '',
    descrpicion: '',
    precioCompra: 0,
    precioVena: 0,
    stock: 0,
    idCategoria: 1
  };

constructor(private ProductosService : ProductosServicio, private CategoriasService: CategoriasServicio) {}

  ngOnInit() {
    this.cargarProductos();
    this.cargarCategorias();
  }
  cargarCategorias() {
    this.CategoriasService.getCategorias().subscribe(cats => this.categorias = cats);
  }
  cargarProductos() {
    this.ProductosService.getTodos().subscribe(res => this.productos = res);
  }

  onFileChange(event: any) {
    this.archivo = event.target.files[0];
  }




  eliminar(id: number) {
    Swal.fire({
      title: '¿Estás seguro?',
      text: 'Esta acción eliminará el producto definitivamente',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, eliminar'
    }).then(result => {
      if (result.isConfirmed) {
        this.ProductosService.eliminar(id).subscribe(() => {
          this.cargarProductos();
          Swal.fire('Eliminado', 'El producto ha sido eliminado.', 'success');
        });
      }
    });
  }


  //Ventana emergente para agregar
  abrirModalCrear() {
    const optionsHtml = this.categorias
    .map(c => `<option value="${c.idCategoria}">${c.nombre}</option>`)
    .join('');
  Swal.fire({
    title: 'Nuevo Producto',
    html: `
      <h4 class="textoHolder">Nombre del Producto</h4>
      <input id="nombre" class="swal2-input" placeholder="Nombre">
      <h4 class="textoHolder">Descripción</h4>
      <input id="descrpicion" class="swal2-input" placeholder="Descripción">
      <h4 class="textoHolder">Precio Entrada</h4>
      <input id="precioCompra" type="number" class="swal2-input" placeholder="Precio Compra">
      <h4 class="textoHolder">Precio Venta</h4>
      <input id="precioVena" type="number" class="swal2-input" placeholder="Precio Venta">
      <h4 class="textoHolder">Stock</h4>
      <input id="stock" type="number" class="swal2-input" placeholder="Stock">
      <h4 class="textoHolder">Categoria</h4>
      <select id="idCategoria" class="swal2-select">${optionsHtml}</select>
      <h4 class="textoHolder">Imagen referencial</h4>
      <input id="imagen" type="file" class="swal2-file">
    `,

    focusConfirm: false,
    showCancelButton: true,
    confirmButtonText: 'Crear',
    cancelButtonText: 'Cancelar',
    preConfirm: () => {

      const nombre = (document.getElementById('nombre') as HTMLInputElement).value;
      const descrpicion = (document.getElementById('descrpicion') as HTMLInputElement).value;
      const precioCompra = (document.getElementById('precioCompra') as HTMLInputElement).value;
      const precioVena = (document.getElementById('precioVena') as HTMLInputElement).value;
      const stock = (document.getElementById('stock') as HTMLInputElement).value;
      const idCategoria = Number((document.getElementById('idCategoria') as HTMLSelectElement).value);
      const imagenInput = (document.getElementById('imagen') as HTMLInputElement);

      if (!nombre || !descrpicion || Number(precioVena) <= 0 || Number(stock) < 0) {
      Swal.showValidationMessage('Completa todos los campos correctamente.');
      return false;
      } return {
        nombre,
        descrpicion,
        precioCompra: Number(precioCompra),
        precioVena: Number(precioVena),
        stock: Number(stock),
        idCategoria,
        archivo: imagenInput.files && imagenInput.files[0] ? imagenInput.files[0] : null
      };
    }
    }).then(result => {

    if (result.isConfirmed) {
      const formData = new FormData();
      formData.append('Nombre', result.value.nombre);
      formData.append('Descrpicion', result.value.descrpicion);
      formData.append('PrecioCompra', result.value.precioCompra.toString());
      formData.append('PrecioVena', result.value.precioVena.toString());
      formData.append('Stock', result.value.stock.toString());
      formData.append('IdCategoria',result.value.idCategoria.toString());
      if (result.value.archivo) formData.append('Imagen', result.value.archivo);
      Swal.fire({
        title: 'Guardando...',
        text: 'Por favor espera',
        didOpen: () => Swal.showLoading(),
        allowOutsideClick: false
      });

      // Llamada al backend
      this.ProductosService.crear(formData).subscribe({
        next: (res: any) => {
          Swal.fire('Creado', res.mensaje || 'El producto fue agregado exitosamente', 'success');
          this.cargarProductos();
        },
        error: (err) => {
          const msg = err.error?.mensaje || 'Ocurrió un error al crear el producto.';
          Swal.fire('Error', msg, 'error');
        }
      });
    }
  });
}
  // la ventana emergente de editar ps xd
  abrirModalEditar(p: Producto) {
    const optionsHtml = this.categorias
    .map(c => `<option value="${c.idCategoria}" ${c.idCategoria === p.idCategoria ? 'selected' : ''}>${c.nombre}</option>`)
    .join('');
  Swal.fire({
    title: 'Editar Producto',
    html: `
      <h4 class="textoHolder">Nombre del Producto</h4>
      <input id="nombre" class="swal2-input" placeholder="Nombre" value="${p.nombre}">
      <h4 class="textoHolder">Descripción</h4>
      <input id="descrpicion" class="swal2-input" placeholder="Descripción" value="${p.descrpicion}">
      <h4 class="textoHolder">Precio Entrada</h4>
      <input id="precioCompra" type="number" class="swal2-input" placeholder="Precio Compra" value="${p.precioCompra}">
      <h4 class="textoHolder">Precio Venta</h4>
      <input id="precioVena" type="number" class="swal2-input" placeholder="Precio Venta" value="${p.precioVena}">
      <h4 class="textoHolder">Stock</h4>
      <input id="stock" type="number" class="swal2-input" placeholder="Stock" value="${p.stock}">
      <h4 class="textoHolder">Categoria</h4>
      <select id="idCategoria" class="swal2-select">${optionsHtml}</select>
      <h4 class="textoHolder">Selecciona una imagen</h4>
      <input id="imagen" type="file" class="swal2-file">
    `,
    focusConfirm: false,
    showCancelButton: true,
    confirmButtonText: 'Guardar',
    cancelButtonText: 'Cancelar',
    preConfirm: () => {
      const nombre = (document.getElementById('nombre') as HTMLInputElement).value;
      const descrpicion = (document.getElementById('descrpicion') as HTMLInputElement).value;
      const precioCompra = (document.getElementById('precioCompra') as HTMLInputElement).value;
      const precioVena = (document.getElementById('precioVena') as HTMLInputElement).value;
      const stock = (document.getElementById('stock') as HTMLInputElement).value;
      const idCategoria = Number((document.getElementById('idCategoria') as HTMLSelectElement).value);
      const imagenInput = (document.getElementById('imagen') as HTMLInputElement);

      if (!nombre || !descrpicion) {
        Swal.showValidationMessage('Completa al menos nombre y descripción');
        return false;
      }

      return {
        nombre,
        descrpicion,
        precioCompra: Number(precioCompra),
        precioVena: Number(precioVena),
        stock: Number(stock),
        idCategoria,
        archivo: imagenInput.files && imagenInput.files[0] ? imagenInput.files[0] : null
      };
    }
  }).then(result => {
    if (result.isConfirmed) {
      const formData = new FormData();
      formData.append('Nombre', result.value.nombre);
      formData.append('Descrpicion', result.value.descrpicion);
      formData.append('PrecioCompra', result.value.precioCompra.toString());
      formData.append('PrecioVena', result.value.precioVena.toString());
      formData.append('Stock', result.value.stock.toString());
      formData.append('IdCategoria', result.value.idCategoria.toString());


      // Mostramos loading mientras se guarda
      Swal.fire({
        title: 'Guardando...',
        text: 'Por favor espera',
        didOpen: () => Swal.showLoading(),
        allowOutsideClick: false
      });

      this.ProductosService.editar(p.idProducto, formData).subscribe({
        next: (res: any) => {
          Swal.fire('Actualizado', res.mensaje || 'El producto fue editado exitosamente', 'success');
          this.cargarProductos();
        },
        error: (err) => {
          const msg = err.error?.mensaje || 'Ocurrió un error al actualizar el producto.';
          Swal.fire('Error', msg, 'error');
        }
      });
    }
  });
}
  abrirModalCrearCategoria() {
  Swal.fire({
    title: 'Nueva Categoría',
    input: 'text',
    inputPlaceholder: 'Nombre categoría',
    showCancelButton: true,
    confirmButtonText: 'Crear',
    cancelButtonText: 'Cancelar',
    preConfirm: (nombre) => {
      if (!nombre) {
        Swal.showValidationMessage('Escribe un nombre');
        return false;
      }
      return nombre;
    }
  }).then(result => {
    if (result.isConfirmed) {
      this.CategoriasService.crear(result.value).subscribe(nuevacat => {
        this.categorias.push(nuevacat);
        this.cargarCategorias();
        Swal.fire('Creada', 'Categoría agregada exitosamente', 'success');
      });
    }
  });
  }
  abrirModalEditarCategoria(c: Categorias) {
    Swal.fire({
      title: 'Editar Categoría',
      input: 'text',
      inputValue: c.nombre,
      showCancelButton: true,
      confirmButtonText: 'Guardar',
      cancelButtonText: 'Cancelar',
      preConfirm: (nombre) => {
        if (!nombre) {
          Swal.showValidationMessage('Escribe un nombre');
          return false;
        }
        return nombre;
      }
    }).then(result => {
      if (result.isConfirmed) {
        this.CategoriasService.editar(c.idCategoria, result.value).subscribe(editada => {
        const idx = this.categorias.findIndex(cat => cat.idCategoria === c.idCategoria);
        if (idx > -1) this.categorias[idx] = editada;
        this.categorias = [...this.categorias];
        Swal.fire('Actualizada', 'Categoría editada exitosamente', 'success');
        this.cargarCategorias();
        });
      }
    }
  );
  }

  eliminarCategoria(id: number) {
    Swal.fire({
      title: '¿Eliminar categoría?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then(result => {
      if (result.isConfirmed) {
        this.CategoriasService.eliminar(id).subscribe(() => {
        //Refrescamos sin reiniciar la pagina al igual que editar y crear
        this.categorias = this.categorias.filter(c => c.idCategoria !== id);
        Swal.fire('Eliminada', 'La categoría fue eliminada', 'success');
        });
      }
    });
  }
  }
