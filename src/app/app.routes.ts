import { Routes } from '@angular/router';
import { Navbar } from './Componentes/navbar/navbar';
import { Indice } from './Componentes/indice/indice';
import { Repuestos } from './Componentes/repuestos/repuestos';
import { Registro } from './Componentes/registro/registro/registro';
import { Login } from './Componentes/login/login';
import { VerificarCod } from './Componentes/verificar-cod/verificar-cod';
import { Admin } from './Componentes/admin/admin';
import { AdminProductos } from './Componentes/admin-productos/admin-productos';


export const routes: Routes = [
 {path : '', component: Indice},
 {path : 'repuestos', component: Repuestos },
 {path : 'registro', component: Registro},
 {path: 'login', component: Login},
 {path: 'verificarcod', component: VerificarCod},
{ path: 'admin', component: Admin},
{ path: 'admin-productos', component: AdminProductos}
];
