import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketEntry } from './ticket-entry';

describe('TicketEntry', () => {
  let component: TicketEntry;
  let fixture: ComponentFixture<TicketEntry>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketEntry]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketEntry);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
