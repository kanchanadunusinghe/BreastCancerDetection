import { Gender } from '../../enums/gender';

export class PatientListModel {
	id!: number;

	nhsNumber!: string;
	firstName!: string;
	lastName!: string;

	email!: string;
	mobileNumber!: string;

	dateOfBirth!: string;
	gender!: string;
	sex!: Gender;

	postCode!: string;
	createdAt!: string;

	mammographyScanCount!: number;

	// Computed property
	get fullName(): string {
		return `${this.firstName} ${this.lastName}`.trim();
	}

	get age(): number {
		const dob = new Date(this.dateOfBirth);
		const today = new Date();

		let age = today.getFullYear() - dob.getFullYear();
		const hasBirthdayPassed =
			today.getMonth() > dob.getMonth() || (today.getMonth() === dob.getMonth() && today.getDate() >= dob.getDate());

		if (!hasBirthdayPassed) {
			age--;
		}

		return age;
	}

	status!: string;
}
