import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, ValidationErrors } from '@angular/forms';
import { TenantService } from './../service/tenant.service';

@Injectable({ providedIn: 'root' })
export class TenantDomainValidator implements AsyncValidator {
	constructor(private _tenantService: TenantService) {}

	async validate(control: AbstractControl): Promise<ValidationErrors | null> {
		console.log(control.value);

		var response = await this._tenantService.validateTenantDomain(control.value);

		if (!response.succeeded) {
			console.log(response);

			return { isExist: true };
		} else {
			return null;
		}
	}
}
