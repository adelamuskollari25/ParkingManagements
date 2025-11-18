import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TariffView } from './tariff-view';

describe('TariffView', () => {
  let component: TariffView;
  let fixture: ComponentFixture<TariffView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TariffView]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TariffView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
