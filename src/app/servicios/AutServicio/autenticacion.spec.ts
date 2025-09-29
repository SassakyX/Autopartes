import { AuthService } from './autenticacion';
import { TestBed } from '@angular/core/testing';



describe('Autenticacion', () => {
  let service: AuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
