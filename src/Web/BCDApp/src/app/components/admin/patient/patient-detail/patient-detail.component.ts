import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { PatinetMasterDataModel } from 'src/app/core/models/patient/patient.master.model';
import { PatientService } from 'src/app/core/service/patient.service';

@Component({
	selector: 'app-patient-detail',
	templateUrl: './patient-detail.component.html',
	styleUrls: ['./patient-detail.component.scss'],
})
export class PatientDetailComponent implements OnInit {
	patientId: number = 0;
	patientDetailForm!: UntypedFormGroup;

	patinetMasterDataModel: PatinetMasterDataModel = {
		listOfGenders: [],
		listOfActiveStatus: [],
	};

	exrayAttachment: File | null = null;
	activeIndex: number = 0;

	constructor(
		private readonly _patientService: PatientService,
		private readonly _toastrService: ToastrService,
		private readonly _spinnerService: NgxSpinnerService,
		private readonly _router: Router,
		private readonly _formBuilder: UntypedFormBuilder,
		private readonly _activatedRoute: ActivatedRoute
	) {}

	ngOnInit(): void {
		this._activatedRoute.params.subscribe(async (params) => {
			this.patientId = +params['id'] || 0;

			await this.getPatientMasterData();
		});

		this._activatedRoute.queryParams.subscribe((params) => {
			const tab = params['tab'];

			if (tab === 'breast-cancer-detector') {
				this.activeIndex = 1;
			}
		});
	}

	async getPatientMasterData(): Promise<void> {
		try {
			this._spinnerService.show();
			this.patinetMasterDataModel = await this._patientService.getPatientMasterData();
			await this.createPatientDetailForm();
		} catch (error) {
			this._toastrService.error('Failed to load master data', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}

	async createPatientDetailForm(): Promise<void> {
		try {
			this.patientDetailForm = this._formBuilder.group({
				id: [0],
				nhsNumber: [''],
				firstName: [''],
				lastName: [''],
				email: [''],
				mobileNumber: [''],
				dateOfBirth: [''],
				gender: [1],
				postCode: [''],
			});

			if (this.patientId > 0) {
				await this.getPatientDetailById();

				this.patientDetailForm.get('nhsNumber')?.disable();
			}
		} catch (error) {}
	}

	async getPatientDetailById(): Promise<void> {
		try {
			this._spinnerService.show();
			const patientDetail = await this._patientService.getPatientById(this.patientId);
			this.patientDetailForm.patchValue(patientDetail);
			this.patientDetailForm.get('dateOfBirth')?.setValue(new Date(patientDetail.dateOfBirth));
		} catch (error) {
			this._toastrService.error('Error fetching patient details');
		} finally {
			this._spinnerService.hide();
		}
	}

	async savePatient(): Promise<void> {
		if (this.patientDetailForm.invalid) {
			this._toastrService.error('Please fill all required fields', 'Validation Error');
			return;
		}

		try {
			this._spinnerService.show();
			const patientRequest = this.patientDetailForm.getRawValue();
			const response = await this._patientService.savePatient(patientRequest);

			if (response.success) {
				this._toastrService.success(response.message, 'Success');
				this._router.navigate(['/admin/patient']);
			} else {
				this._toastrService.error(response.message, 'Error');
			}
		} catch (error) {
			this._toastrService.error('Failed to save patient details', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}

	private readonly MAX_FILE_SIZE = 10 * 1024 * 1024;
	onFileSelected(event: Event): void {
		const input = event.target as HTMLInputElement;

		if (!input.files || input.files.length === 0) return;

		if (input.files.length > 1) {
			this._toastrService.error('Only one image can be selected.', 'Error');
			input.value = '';
			return;
		}

		const file = input.files[0];

		if (!file.type.startsWith('image/')) {
			this._toastrService.error('Only image files are allowed.', 'Error');
			input.value = '';
			return;
		}

		if (file.size > this.MAX_FILE_SIZE) {
			this._toastrService.error('Image must be less than 10MB.', 'Error');
			input.value = '';
			return;
		}

		this.exrayAttachment = file;

		input.value = '';
	}

	onDragOver(event: DragEvent) {
		event.preventDefault();
	}

	onFileDrop(event: DragEvent) {
		event.preventDefault();
		if (event.dataTransfer?.files) {
			this.addFiles(event.dataTransfer.files);
		}
	}

	addFiles(files: FileList) {
		Array.from(files).forEach((file) => {
			if (file.size <= 10 * 1024 * 1024) {
				//this.supportTicketAttachments.push(file);
			}
		});
	}

	async breastCancerPredict(): Promise<void> {
		if (!this.exrayAttachment) {
			this._toastrService.error('Please select an image for prediction.', 'Validation Error');
			return;
		}

		try {
			this._spinnerService.show();
			const formData = new FormData();
			formData.append('ExrayImage', this.exrayAttachment);
			formData.append('PatientId', this.patientId.toString());
			const response = await this._patientService.breastCancerPredict(formData);

			if (response.success) {
				this._toastrService.success(`Prediction Result: ${response.data}`, 'Success');
			} else {
				this._toastrService.error(response.message, 'Error');
			}
		} catch (error) {
			this._toastrService.error('Failed to perform prediction', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}
}
