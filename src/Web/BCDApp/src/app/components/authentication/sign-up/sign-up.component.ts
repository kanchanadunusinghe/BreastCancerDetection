import { Component } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from './../../../core/service/authentication.service';

@Component({
	selector: 'app-sign-up',
	templateUrl: './sign-up.component.html',
	styleUrls: ['./sign-up.component.scss'],
})
export class SignUpComponent {
	userAuthenticationForm: UntypedFormGroup;
	button = 'Sign In';
	isLoading = false;
	showPassword = false;
	/**
	 * Constructor
	 * @param {UntypedFormBuilder} _untypedFormBuilder
	 * @param {AuthenticationService} _authenticationService
	 * @param {Rotuter} _router
	 * @param {NgxSpinnerService} _spinner
	 */

	constructor(
		private _untypedFormBuilder: UntypedFormBuilder,
		private _authenticationService: AuthenticationService,
		private _router: Router,
		private _spinner: NgxSpinnerService,
		private _toastr: ToastrService
	) {}

	async ngOnInit() {
		await this.createUserAuthenticationForm();
	}

	/**
	 * SignUpComponent
	 * @param {}
	 * @service createUserAuthenticationForm
	 * @returns {Promise<void>}
	 */
	async createUserAuthenticationForm(): Promise<void> {
		this.userAuthenticationForm = this._untypedFormBuilder.group({
			email: ['', [Validators.required]],
			password: ['', [Validators.required]],
		});
	}

	/**
	 * SignUpComponent
	 * @param {}
	 * @service login
	 * @returns {Promise<void>}
	 */
	async login(): Promise<void> {
		try {
			this.button = 'Authenticating...';
			this.isLoading = true;
			let response = await this._authenticationService.login(this.userAuthenticationForm.value);
			if (response.isSuccess) {
				setTimeout(() => {
					this.button = 'Sign In';
					this.isLoading = false;
				}, 5000);
				this._router.navigate(['/admin']);
			} else {
				this._toastr.error(response.message, 'Error');
				this.button = 'Sign In';
				this.isLoading = false;
			}
		} catch (error) {
			this.button = 'Sign In';
			this.isLoading = false;
		}
	}
}
