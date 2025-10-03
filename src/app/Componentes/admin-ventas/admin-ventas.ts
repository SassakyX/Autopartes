import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Producto, ProductosServicio } from '../../servicios/Productos/productos';
import Swal from 'sweetalert2';
import { VentasService } from '../../servicios/Ventas/ventas';
import { AuthService } from '../../servicios/AutServicio/autenticacion';

@Component({
  selector: 'app-admin-ventas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-ventas.html',
  styleUrl: './admin-ventas.css'
})

export class AdminVentas implements OnInit {
  ventas: any[] = [];

  constructor(private ventasService: VentasService, public auth: AuthService) {}

  ngOnInit(): void {
    this.cargarVentas();
  }

  cargarVentas() {
    this.ventasService.getVentas().subscribe(res => {
      this.ventas = res.map(v => ({
        ...v,
        estadoAnterior: v.estado
       }));
      });
    }
  cambiarEstado(id: number, nuevoEstado: string, estadoAnterior: string) {
    // condicional para verificar estado
  if (estadoAnterior === 'Finalizado' || estadoAnterior === 'Cancelado') {
    Swal.fire('Acción no permitida', 'Esta venta ya no se puede modificar.', 'error');
    return;
  }

  // Confirmación
  Swal.fire({
    title: '¿Estás seguro?',
    text: `¿Quieres cambiar el estado de esta venta a "${nuevoEstado}"?`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonColor: '#2fd42f',
    cancelButtonColor: '#d33',
    confirmButtonText: 'Sí, cambiar'
  }).then((result) => {
    if (result.isConfirmed) {
      this.ventasService.cambiarEstado(id, nuevoEstado).subscribe({
        next: () => {
          Swal.fire('Éxito', `Estado cambiado a ${nuevoEstado}`, 'success');
          this.cargarVentas(); // refresca y actualiza estado Anterior también
        },
        error: (err) => {
          Swal.fire('Error',err.error?.mensaje || 'No se pudo cambiar el estado', 'error');
          this.cargarVentas();
        }
      });
    } else {
      // revertimos el select si canceló
      const venta = this.ventas.find(v => v.idVenta === id);
      if (venta) venta.estado = estadoAnterior;
    }
  });
}
  cerrarSesion() {
  this.auth.logout();
  }
}

