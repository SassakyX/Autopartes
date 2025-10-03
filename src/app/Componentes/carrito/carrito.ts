import { AuthService } from './../../servicios/AutServicio/autenticacion';
import { CommonModule } from '@angular/common';
import { Component, importProvidersFrom, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Init } from 'v8';
import { Producto } from '../../servicios/Productos/productos';
import { CarritoServicio } from '../../servicios/Carrito/carrito';
import { VentasService } from '../../servicios/Ventas/ventas';
import Swal, { SweetAlertArrayOptions } from 'sweetalert2';


@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './carrito.html',
  styleUrl: './carrito.css'
})

export class Carrito implements OnInit {
  items: Producto[] = [];

  constructor(private carritoService: CarritoServicio, private ventasService: VentasService, private AuthService: AuthService) {}

  ngOnInit(): void {
    this.carritoService.items$.subscribe(items => this.items = items);
  }

  eliminar(index: number) {
    this.carritoService.eliminar(index);
  }

  limpiar() {
    this.carritoService.limpiar();
  }

getTotal() {
    return this.items.reduce((sum, i) => sum + i.precioVena * (i.cantidad || 1), 0);
  }

    finalizarCompra() {
      const usuario = this.AuthService.getUsuario();
      if (!usuario) {
        Swal.fire('⚠️ Error', 'Debes iniciar sesión para comprar', 'error');
        return;
      }
    const venta = {
      idUsuario: usuario.idUsuario,
      detalles: this.items.map(i => ({
        idProducto: i.idProducto,
        cantidad: i.cantidad || 1,
        precioUnidad: i.precioVena
      }))
    };


    this.ventasService.crearVenta(venta).subscribe({
      next: () => {
        Swal.fire({
          title: "Compra realizada",
          text: "Tu pedido se ha registrado correctamente",
          icon: "success",
          confirmButtonText: "Aceptar"
        });
        this.items = [];
        localStorage.removeItem('carrito');
      },
      error: () => {
        Swal.fire('Error', 'No se pudo completar la compra', 'error');
      }
    });
  }
}
