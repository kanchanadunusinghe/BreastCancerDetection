import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientBreastCancerDitectionComponent } from './patient-breast-cancer-ditection.component';

describe('PatientBreastCancerDitectionComponent', () => {
  let component: PatientBreastCancerDitectionComponent;
  let fixture: ComponentFixture<PatientBreastCancerDitectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PatientBreastCancerDitectionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PatientBreastCancerDitectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
