import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Sobrenosotros } from './sobrenosotros';

describe('Sobrenosotros', () => {
  let component: Sobrenosotros;
  let fixture: ComponentFixture<Sobrenosotros>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Sobrenosotros]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Sobrenosotros);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
