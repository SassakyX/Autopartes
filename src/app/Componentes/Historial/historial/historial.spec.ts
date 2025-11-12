import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HistorialC } from './historial';

describe('Historial', () => {
  let component: HistorialC;
  let fixture: ComponentFixture<HistorialC>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HistorialC]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HistorialC);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
