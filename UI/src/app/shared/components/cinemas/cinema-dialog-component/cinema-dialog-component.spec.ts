import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CinemaDialogComponent } from './cinema-dialog-component';

describe('CinemaDialogComponent', () => {
  let component: CinemaDialogComponent;
  let fixture: ComponentFixture<CinemaDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CinemaDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CinemaDialogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
