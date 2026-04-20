export interface AuthenticationResponseModel {
	isSuccess: boolean;
	error?: string;

	displayName?: string;
	token?: string;
	email?: string;
	roles?: string[];
	expiresAt?: string;
	message?: string;
	userId?: number;
}

/* export class AuthenticationResponseDto {
  isSuccess!: boolean;
  error?: string;

  displayName?: string;
  token?: string;
  email?: string;
  roles?: string[];
  expiresAt?: Date;
  message?: string;

  private constructor() {}

  static success(
    displayName: string,
    email: string,
    token: string,
    roles: string[],
    expiresAt: Date,
    message: string
  ): AuthenticationResponseDto {
    const response = new AuthenticationResponseDto();
    response.isSuccess = true;
    response.displayName = displayName;
    response.email = email;
    response.token = token;
    response.roles = roles;
    response.expiresAt = expiresAt;
    response.message = message;

    return response;
  }

  static failure(error: string): AuthenticationResponseDto {
    const response = new AuthenticationResponseDto();
    response.isSuccess = false;
    response.error = error;

    return response;
  }
} */
