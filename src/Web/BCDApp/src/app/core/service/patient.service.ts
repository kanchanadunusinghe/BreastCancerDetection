import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResultModel } from '../models/common/paged.result.model';
import { ResponseModel } from '../models/common/result.model';
import { BreastCancerPredictionResultResponseModel } from '../models/machine-learning/breast.cancer.prediction.result.response.model';
import { MammographyScanResultModel } from '../models/patient/mammography.scan.result.model';
import { PatientFilterModel } from '../models/patient/patient.filter.model';
import { PatientListModel } from '../models/patient/patient.list.model';
import { PatinetMasterDataModel } from '../models/patient/patient.master.model';
import { PatientRequestModel } from '../models/patient/patient.request.model';
@Injectable({
	providedIn: 'root',
})
export class PatientService {
	baseUrl = `${environment.apiUrl}Patient`;

	constructor(private _httpClient: HttpClient) {}

	async getPatientById(id: number): Promise<PatientRequestModel> {
		return await firstValueFrom(this._httpClient.get<PatientRequestModel>(`${this.baseUrl}/getPatientById/${id}`));
	}

	async getPatientByFilter(filter: PatientFilterModel): Promise<PagedResultModel<PatientListModel>> {
		let params = new HttpParams()
			.set('Search', filter.search ?? ' ')
			.set('Gender', filter.gender.toString())
			.set('ActiveStatus', filter.activeStatus.toString())
			.set('PageNumber', filter.pageNumber.toString())
			.set('PageSize', filter.pageSize.toString());

		return await firstValueFrom(
			this._httpClient.get<PagedResultModel<PatientListModel>>(`${this.baseUrl}/getPatientsByFilter`, { params })
		);
	}

	async savePatient(patient: PatientRequestModel): Promise<ResponseModel<number>> {
		return await firstValueFrom(this._httpClient.post<ResponseModel<number>>(`${this.baseUrl}/savePatient`, patient));
	}

	async getPatientMasterData(): Promise<PatinetMasterDataModel> {
		return await firstValueFrom(this._httpClient.get<PatinetMasterDataModel>(`${this.baseUrl}/getPatientMasterData`));
	}

	async breastCancerPredict(formData: FormData): Promise<ResponseModel<BreastCancerPredictionResultResponseModel>> {
		return await firstValueFrom(
			this._httpClient.post<ResponseModel<BreastCancerPredictionResultResponseModel>>(
				`${this.baseUrl}/breastCancerPredict`,
				formData
			)
		);
	}

	async mammographyScanCommentUpdate(id: number, comment: string): Promise<ResponseModel<number>> {
		return await firstValueFrom(
			this._httpClient.put<ResponseModel<number>>(`${this.baseUrl}/mammographyScanCommentUpdate`, {
				id,
				comment,
			})
		);
	}

	async getMammographyScansByPatinetId(patientId: number): Promise<MammographyScanResultModel[]> {
		return await firstValueFrom(
			this._httpClient.get<MammographyScanResultModel[]>(`${this.baseUrl}/getMammographyScansByPatinetId/${patientId}`)
		);
	}
}
