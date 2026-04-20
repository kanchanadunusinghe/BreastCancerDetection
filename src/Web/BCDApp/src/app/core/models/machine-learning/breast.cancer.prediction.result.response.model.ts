import { PatientRequestModel } from '../patient/patient.request.model';

export interface BreastCancerPredictionResultResponseModel {
	patient: PatientRequestModel;
	recordId: number;
	comment?: string;
	cancerType: string;
	probability: number;
	exrayImageUrl?: string;
	createdAt: string;
	createdUserName: string;
}
