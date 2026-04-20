import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, firstValueFrom, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthenticationModel } from './../models/authentication/authentication.model';
import { AuthenticationResponseModel } from './../models/authentication/authentication.response.model';

@Injectable({
	providedIn: 'root',
})
export class AuthenticationService {
	baseUrl = environment.apiUrl;

	private currentUserSubject: BehaviorSubject<AuthenticationResponseModel>;
	public currentUser: Observable<AuthenticationResponseModel>;

	/**
	 * Constructor
	 * @param {HttpClient} _httpClient
	 */

	constructor(private _httpClient: HttpClient) {
		this.currentUserSubject = new BehaviorSubject<AuthenticationResponseModel>(
			JSON.parse(localStorage.getItem('BCDUser'))
		);
		this.currentUser = this.currentUserSubject.asObservable();
	}

	public get currentUserValue(): AuthenticationResponseModel {
		return this.currentUserSubject.value;
	}

	async login(authenticationModel: AuthenticationModel): Promise<AuthenticationResponseModel> {
		return await firstValueFrom(
			this._httpClient
				.post<AuthenticationResponseModel>(`${this.baseUrl}Authentication/login`, authenticationModel)
				.pipe(
					map((bCDUser) => {
						localStorage.setItem('BCDUser', JSON.stringify(bCDUser));
						this.currentUserSubject.next(bCDUser);
						return bCDUser;
					})
				)
		);
	}

	/**
	 * Authetication Service
	 * @param {}
	 * @service log out user
	 * @returns {}
	 */
	async logout() {
		// remove user from local storage to log user out
		localStorage.removeItem('eduArkMasterUser');
		this.currentUserSubject.next(null);
		return of({ success: false });
	}
}
