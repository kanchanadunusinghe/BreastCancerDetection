import { Gender } from '../../enums/gender';

export interface PatientRequestModel {
	id: number;
	nhsNumber: string;

	firstName: string;
	lastName: string;

	email: string;
	mobileNumber: string;

	dateOfBirth: string;
	gender: Gender;

	postCode: string;
}
