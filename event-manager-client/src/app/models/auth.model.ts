export interface LoginRequest {
  email: string;
  passWord: string;
}

export interface AuthResponse {
  token: string;
  email: string;
}