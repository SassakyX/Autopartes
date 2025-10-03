import { TestBed } from '@angular/core/testing';
import { CarritoServicio } from './carrito';

describe('Carrito', () => {
  let service: CarritoServicio;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CarritoServicio);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
