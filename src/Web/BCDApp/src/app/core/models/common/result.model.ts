export class ResultModel {
	id: number;
	succeeded: boolean;
	successMessage: string;
	data: string;
	errors: Array<string>;
}

export interface ResponseModel<T> {
	success: boolean;
	message: string;
	data?: T | null;
	errors: string[];
}
