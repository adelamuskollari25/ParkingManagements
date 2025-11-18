import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LotsFormDialog } from './lots-form-dialog';

describe('LotsFormDialog', () => {
  let component: LotsFormDialog;
  let fixture: ComponentFixture<LotsFormDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LotsFormDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LotsFormDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
