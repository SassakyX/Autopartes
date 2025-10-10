import { TestBed } from '@angular/core/testing';

import { GatoServicio } from '../gato-servicio';

describe('GatoServicio', () => {
  let service: GatoServicio;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GatoServicio);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
