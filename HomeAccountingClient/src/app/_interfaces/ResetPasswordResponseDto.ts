export interface ResetPasswordResponseDto{
    isPawwordReseted : boolean;
    resetToken :string;
    errorMessage: string;
}