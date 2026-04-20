import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
	{
		path: '',
		redirectTo: 'patient',
		pathMatch: 'full',
	},
	{
		path: 'patient',
		loadChildren: () => import('./patient/patient.module').then((m) => m.PatientModule),
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AdminRoutingModule {}
