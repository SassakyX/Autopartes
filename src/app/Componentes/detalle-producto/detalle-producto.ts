import { AuthService } from './../../servicios/AutServicio/autenticacion';
import { CarritoServicio } from './../../servicios/Carrito/carrito';
import { Component, ElementRef, HostListener, inject, ViewChild } from '@angular/core';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { ResenaServicio, ReseñaDto} from '../../servicios/Resenas/resenas';

@Component({
  selector: 'app-detalle-producto',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './detalle-producto.html',
  styleUrl: './detalle-producto.css'
})
  export class DetalleProducto {
  producto: Producto | null = null;

  cantidad: number = 1;
  resenas: any[] = [];
  promedioCalificaciones: number = 0;
  calificacionSeleccionada: number = 0;
  comentario: string = '';
  haCompradoProducto: boolean = false;
  usuarioId: number = 0;


  panelAbierto: boolean = false;
  panelOffset: number = 0;
  startX: number = 0;
  dragging: boolean = false;
  screenWidth: number = window.innerWidth;

  @ViewChild('panel') panel!: ElementRef<HTMLDivElement>;
  @ViewChild('imagen') imagen!: ElementRef<HTMLImageElement>;



  constructor(
    private route: ActivatedRoute,
    private productosService: ProductosServicio,
    private carritoService: CarritoServicio,
    private resenaService: ResenaServicio,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const usuario = this.authService.getUsuario();
    if (usuario) {
    this.usuarioId = usuario.idUsuario; // aquí se asigna
    }


    if (id) {
      this.productosService.getPorId(id).subscribe(p => {
        this.producto = p;
        this.cantidad = p.stock > 0 ? 1 : 0;
        this.cargarResenas(p.idProducto);
        if (this.usuarioId) {
        this.verificarCompra(this.usuarioId, p.idProducto);
        }
      });
      }
  }

  verificarCompra(usuarioId: number, productoId: number) {
    console.log("Verificando compra:", { usuarioId, productoId });
    this.resenaService.haCompradoProducto(usuarioId, productoId).subscribe({
      next: (res) => {
        console.log("¿Ha comprado?", res);
        this.haCompradoProducto = res;
      },
      error: (err) => {
        console.error('Error al verificar compra', err);
      }
    });
  }
  //Mouse
 @HostListener('mousedown', ['$event'])
  onMouseDown(e: MouseEvent) {
    const target = e.target as HTMLElement;
    if (target.tagName === 'TEXTAREA' || target.tagName === 'INPUT') return;
    // solo activa si haces clic en el borde derecho
    if (e.clientX > this.screenWidth - 80 || this.panelAbierto) {
      this.dragging = true;
      this.startX = e.clientX;
      e.preventDefault();
    }
  }
  //Dedo
  @HostListener('touchstart', ['$event'])
  onTouchStart(e: TouchEvent) {
  const target = e.target as HTMLElement;
  if (target.tagName === 'TEXTAREA' || target.tagName === 'INPUT') return;
  const touch = e.touches[0];
  if (touch.clientX > this.screenWidth - 80 || this.panelAbierto) {
    this.dragging = true;
    this.startX = touch.clientX;
    e.preventDefault();
  }
  }

  @HostListener('mousemove', ['$event'])
  onMouseMoveGlobal(e: MouseEvent) {
    if (!this.dragging) return;
    const delta = this.startX - e.clientX; // cuánto se movió
    const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;

    if (!this.panelAbierto && delta > 0) {
      this.panelOffset = Math.min(delta, maxOffset);
    } else if (this.panelAbierto && delta < 0) {
      this.panelOffset = maxOffset + delta;
      if (this.panelOffset < 0) this.panelOffset = 0;
    }

    this.updatePanelTransform();
  }

  @HostListener('touchmove', ['$event'])
  onTouchMove(e: TouchEvent) {
    if (!this.dragging) return;
    const touch = e.touches[0];
    const delta = this.startX - touch.clientX;
    const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;

    if (!this.panelAbierto && delta > 0) {
      this.panelOffset = Math.min(delta, maxOffset);
    } else if (this.panelAbierto && delta < 0) {
      this.panelOffset = maxOffset + delta;
      if (this.panelOffset < 0) this.panelOffset = 0;
    }

    this.updatePanelTransform();
  }

  @HostListener('mouseup' , ['$event'])
  onMouseUp(e: MouseEvent) {
    const target = e.target as HTMLElement;
    if (target.tagName === 'TEXTAREA' || target.tagName === 'INPUT') return;

    if (!this.dragging) return;
    this.dragging = false;
    const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;
    const porcentaje = this.panelOffset / maxOffset;

    this.panelAbierto = porcentaje > 0.4;
    this.panelOffset = this.panelAbierto ? maxOffset : 0;
    this.updatePanelTransform();
  }

    @HostListener('touchend', ['$event'])
    onTouchEnd(e: TouchEvent) {
    const target = e.target as HTMLElement;
    if (target.tagName === 'TEXTAREA' || target.tagName === 'INPUT') return;

    if (!this.dragging) return;
    this.dragging = false;

    const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;
    const porcentaje = this.panelOffset / maxOffset;

    this.panelAbierto = porcentaje > 0.4;
    this.panelOffset = this.panelAbierto ? maxOffset : 0;
    this.updatePanelTransform();
  }

  updatePanelTransform() {
  const panel = this.panel.nativeElement;
  const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;

  // calculamos el desplazamiento real (sin exagerar el movimiento)
  const translateX = this.panelAbierto
    ? maxOffset - this.panelOffset // cuando está abierto, el offset disminuye
    : maxOffset - this.panelOffset; // cuando está cerrado, aumenta

  // aplicamos el translateX en sentido correcto
  panel.style.transform = `translateX(${translateX}px)`;
  }

  togglePanel() {
    this.panelAbierto = !this.panelAbierto;
    const maxOffset = this.screenWidth > 992 ? 500 : this.screenWidth;
    this.panelOffset = this.panelAbierto ? maxOffset : 0;
    this.updatePanelTransform();
  }


  cargarResenas(idProducto: number) {

    this.resenaService.obtenerReseñas(idProducto).subscribe({
      next: (res) => {
        this.resenas = res;
        this.calcularPromedio();
      },
      error: (err) => console.error('Error al obtener reseñas', err)
    });
  }

  seleccionarCalificacion(valor: number) {
  this.calificacionSeleccionada = valor;
  }

  enviarResena() {
    if (!this.haCompradoProducto) {
      Swal.fire('No permitido', 'Solo los clientes que compraron este producto pueden calificarlo.', 'warning');
      return;
    }

    if (this.calificacionSeleccionada === 0) {
      Swal.fire('Selecciona una calificación', '', 'info');
      return;
    }

    if (!this.producto) return;

    const dto: ReseñaDto = {
      productoId: this.producto.idProducto,
      usuarioId: this.usuarioId,
      estrellas: this.calificacionSeleccionada,
      comentario: this.comentario
    };

    this.resenaService.crearReseña(dto).subscribe({
      next: (res) => {
        this.resenas.push(res);
        this.calcularPromedio();
        this.calificacionSeleccionada = 0;
        this.comentario = '';
        Swal.fire('¡Gracias!', 'Tu reseña ha sido publicada.', 'success');
        this.cargarResenas(this.producto!.idProducto);
      },
      error: (err) => {
        Swal.fire('Error', err.error || 'No se pudo guardar la reseña', 'error');
      }
    });
  }

  calcularPromedio() {
    if (this.resenas.length === 0) {
      this.promedioCalificaciones = 0;
      return;
    }
    const total = this.resenas.reduce((acc, r) => acc + r.estrellas, 0);
    this.promedioCalificaciones = total / this.resenas.length;
  }


  onMouseMoveImagen(event: MouseEvent) {
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
    Swal.fire({
    title: "Producto Agregado!",
    text: `${this.producto.nombre} x${this.cantidad} se agregó al carrito.`,
    icon: "success",
    showConfirmButton: false,
    timer:1800,
    timerProgressBar : true,
    position : "top-end",
    toast : true,
    });
  } else {
    Swal.fire({
    icon: "error",
    title: "Error en la agregar producto al carrito",
    text: "Elige una cantidad valida",
    });
  }
}

}


