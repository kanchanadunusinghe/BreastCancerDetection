import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { LazyLoadEvent } from 'primeng/api';
import { Gender } from '../../../core/enums/gender';
import { DropDownModel } from '../../../core/models/common/drop.down.model';
import { PatientFilterModel } from '../../../core/models/patient/patient.filter.model';
import { PatientListModel } from '../../../core/models/patient/patient.list.model';
import { PatinetMasterDataModel } from '../../../core/models/patient/patient.master.model';
import { PatientService } from '../../../core/service/patient.service';

@Component({
	selector: 'app-patient',
	templateUrl: './patient.component.html',
	styleUrls: ['./patient.component.scss'],
})
export class PatientComponent implements OnInit {
	patientFilterModel: PatientFilterModel = {
		search: '',
		gender: 0,
		activeStatus: 0,
		pageNumber: 0,
		pageSize: 10,
	};

	patinetMasterDataModel: PatinetMasterDataModel = {
		listOfGenders: [],
		listOfActiveStatus: [],
	};

	listOfPatients: PatientListModel[] = [];
	totalRecordCount: number = 0;
	Gender = Gender;
	constructor(
		private readonly _patientService: PatientService,
		private readonly _toastrService: ToastrService,
		private readonly _spinnerService: NgxSpinnerService,
		private readonly _router: Router
	) {}

	async ngOnInit(): Promise<void> {
		await this.getPatientMasterData();
	}

	async getPatientMasterData(): Promise<void> {
		try {
			this._spinnerService.show();
			this.patinetMasterDataModel = await this._patientService.getPatientMasterData();

			let defualtFilter: DropDownModel = {
				id: 0,
				name: '_All_',
			};

			this.patinetMasterDataModel.listOfGenders.unshift(defualtFilter);
			this.patinetMasterDataModel.listOfActiveStatus.unshift(defualtFilter);
		} catch (error) {
			this._toastrService.error('Failed to load master data', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}

	async loadPatients(event: LazyLoadEvent): Promise<void> {
		this.patientFilterModel.pageNumber = (event.first ?? 0) / (event.rows ?? 10);
		this.patientFilterModel.pageSize = event.rows ?? 10;

		await this.getPatientsByFilter();
	}

	async getPatientsByFilter(): Promise<void> {
		try {
			this._spinnerService.show();
			const response = await this._patientService.getPatientByFilter(this.patientFilterModel);
			this.listOfPatients = response.data;
			this.totalRecordCount = response.totalCount;
		} catch (error) {
			this._toastrService.error('Failed to load patients', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}

	async routePatientDetailsModule(id: number): Promise<void> {
		if (id > 0) {
			this._router.navigate(['/admin/patient/patient-detail', id]);
		} else {
			this._router.navigate(['/admin/patient/patient-detail']);
		}
	}

	async routeBreastCancerDetectorModule(id: number): Promise<void> {
		this._router.navigate(['/admin/patient/patient-detail', id], {
			queryParams: { tab: 'breast-cancer-detector' },
		});
	}
}
