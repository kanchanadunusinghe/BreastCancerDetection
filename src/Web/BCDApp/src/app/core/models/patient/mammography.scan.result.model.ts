import { PatientRequestModel } from './patient.request.model';

export interface MammographyScanResultModel {
	id: number;
	recordId: string;
	comment?: string | null;
	cancerType: string;
	probability: number;
	exrayImageUrl?: string | null;
	createdAt: string;
	createdUserName: string;
	patinet: PatientRequestModel;
}
