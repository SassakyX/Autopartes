import { TestBed } from '@angular/core/testing';

import { ProductosServicio } from './productos';

describe('Productos', () => {
  let service: ProductosServicio;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductosServicio);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
