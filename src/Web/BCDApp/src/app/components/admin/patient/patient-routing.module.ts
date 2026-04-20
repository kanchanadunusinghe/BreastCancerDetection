import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PatientDetailComponent } from './patient-detail/patient-detail.component';
import { PatientComponent } from './patient.component';

const routes: Routes = [
	{
		path: '',
		redirectTo: 'patient',
		pathMatch: 'full',
	},
	{
		path: '',
		component: PatientComponent,
	},
	{
		path: 'patient-detail',
		component: PatientDetailComponent,
	},
	{
		path: 'patient-detail/:id',
		component: PatientDetailComponent,
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class PatientRoutingModule {}
