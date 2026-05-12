import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialPermissionsComponent } from './special-permissions.component';

describe('SpecialPermissionsComponent', () => {
  let component: SpecialPermissionsComponent;
  let fixture: ComponentFixture<SpecialPermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SpecialPermissionsComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SpecialPermissionsComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
