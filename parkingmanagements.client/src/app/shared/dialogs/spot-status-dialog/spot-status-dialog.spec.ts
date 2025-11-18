import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpotStatusDialog } from './spot-status-dialog';

describe('SpotStatusDialog', () => {
  let component: SpotStatusDialog;
  let fixture: ComponentFixture<SpotStatusDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SpotStatusDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SpotStatusDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
