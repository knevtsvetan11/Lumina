import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScreeningDialogComponent } from './screening-dialog.component';

describe('ScreeningDialog', () => {
  let component: ScreeningDialogComponent;
  let fixture: ComponentFixture<ScreeningDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScreeningDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScreeningDialogComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
