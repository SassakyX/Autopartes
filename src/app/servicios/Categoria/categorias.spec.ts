import { TestBed } from '@angular/core/testing';

import { Categorias, CategoriasServicio } from './categorias';

describe('Categorias', () => {
  let service: CategoriasServicio;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategoriasServicio);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
