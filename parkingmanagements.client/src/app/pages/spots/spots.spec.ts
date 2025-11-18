import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Spots } from './spots';

describe('Spots', () => {
  let component: Spots;
  let fixture: ComponentFixture<Spots>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Spots]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Spots);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
