import { Component, Input, OnInit } from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { BreastCancerPredictionResultResponseModel } from 'src/app/core/models/machine-learning/breast.cancer.prediction.result.response.model';
import { AuthenticationService } from 'src/app/core/service/authentication.service';
import { MammographyScanResultModel } from '../../../../core/models/patient/mammography.scan.result.model';
import { PatientService } from '../../../../core/service/patient.service';

@Component({
	selector: 'app-patient-breast-cancer-ditection',
	templateUrl: './patient-breast-cancer-ditection.component.html',
	styleUrls: ['./patient-breast-cancer-ditection.component.scss'],
})
export class PatientBreastCancerDitectionComponent implements OnInit {
	@Input() patientId: number = 0;
	exrayAttachment: File | null = null;
	selectedImage: { file: File; preview: string } | null = null;
	isProcessing: boolean = false;
	scanList: MammographyScanResultModel[] = [];
	showResultViewModal: boolean = false;
	constructor(
		private readonly _patientService: PatientService,
		private readonly _toastrService: ToastrService,
		private readonly _spinnerService: NgxSpinnerService,
		private readonly _router: Router,
		private readonly _formBuilder: UntypedFormBuilder,
		private readonly _activatedRoute: ActivatedRoute,
		private readonly _authenticationService: AuthenticationService
	) {}
	async ngOnInit(): Promise<void> {
		await this.getMammographyScansByPatinetId();
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

		const reader = new FileReader();
		reader.onload = () => {
			this.selectedImage = {
				file: file,
				preview: reader.result as string,
			};
		};

		reader.readAsDataURL(file);

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

	removeImage() {
		this.selectedImage = null;

		this.exrayAttachment = null;
	}

	addFiles(files: FileList) {
		Array.from(files).forEach((file) => {
			if (file.size <= 10 * 1024 * 1024) {
				//this.supportTicketAttachments.push(file);
			}
		});
	}
	showResultModal = false;
	predictionResult: BreastCancerPredictionResultResponseModel;
	doctorComment = '';
	async breastCancerPredict(): Promise<void> {
		if (!this.exrayAttachment) {
			this._toastrService.error('Please select an image for prediction.', 'Validation Error');
			return;
		}

		try {
			this._spinnerService.show();
			this.isProcessing = true;
			const formData = new FormData();
			formData.append('ExrayImage', this.exrayAttachment);
			formData.append('PatientId', this.patientId.toString());
			formData.append('LoggedInUserId', this._authenticationService.currentUserValue.userId.toString());
			const response = await this._patientService.breastCancerPredict(formData);

			if (response.success) {
				this._toastrService.success(`Prediction Result: ${response.data}`, 'Success');

				this.predictionResult = response.data;
				this.isProcessing = false;
				this.showResultModal = true;
			} else {
				this._toastrService.error(response.message, 'Error');
			}
		} catch (error) {
			this._toastrService.error('Failed to perform prediction', 'Error');
		} finally {
			this._spinnerService.hide();
		}
	}

	async closeResult() {
		this.showResultModal = false;
		await this.getMammographyScansByPatinetId();
	}

	saveResult() {
		console.log('Save result with comment:', this.doctorComment);
	}

	printReport() {
		window.print();
	}

	sendEmail() {
		console.log('Send email logic here');
	}

	async mammographyScanCommentUpdate() {
		try {
			if (!this.predictionResult) {
				this._toastrService.error('No prediction result to update comment for.', 'Error');
				return;
			}

			const response = await this._patientService.mammographyScanCommentUpdate(
				this.predictionResult.recordId,
				this.doctorComment
			);

			if (response.success) {
				this._toastrService.success(response.message, 'Success');
				await this.getMammographyScansByPatinetId();
			} else {
				this._toastrService.error(response.message, 'Error');
			}
		} catch (error) {}
	}

	async getMammographyScansByPatinetId() {
		try {
			this.scanList = await this._patientService.getMammographyScansByPatinetId(this.patientId);
		} catch (error) {
			this._toastrService.error('Failed to fetch mammography scans', 'Error');
		}
	}

	openScanDetail(scan: any) {
		this.predictionResult = scan;
		this.showResultViewModal = true;
	}

	closeViewModal() {
		this.showResultViewModal = false;
		this.predictionResult = null;
	}
}
