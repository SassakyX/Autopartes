import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarCod } from './verificar-cod';

describe('VerificarCod', () => {
  let component: VerificarCod;
  let fixture: ComponentFixture<VerificarCod>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerificarCod]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VerificarCod);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
