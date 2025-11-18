import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketExit } from './ticket-exit';

describe('TicketExit', () => {
  let component: TicketExit;
  let fixture: ComponentFixture<TicketExit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketExit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketExit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
