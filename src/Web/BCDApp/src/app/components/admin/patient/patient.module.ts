import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientRoutingModule } from './patient-routing.module';
import { PatientComponent } from './patient.component';
import { PatientBreastCancerDitectionComponent } from './patient-breast-cancer-ditection/patient-breast-cancer-ditection.component';

@NgModule({
	declarations: [PatientComponent, PatientDetailComponent, PatientBreastCancerDitectionComponent],
	imports: [
		CommonModule,
		PatientRoutingModule,
		TableModule,
		ConfirmDialogModule,
		DialogModule,
		InputTextModule,
		DropdownModule,
		FormsModule,
		ReactiveFormsModule,
		InputSwitchModule,
		TabViewModule,
		InputTextareaModule,
		CalendarModule,
	],
	providers: [ConfirmationService],
})
export class PatientModule {}
