export interface UserForRegistrationDto {
    userName: string;
    mainCurrencyId: string;
    email: string;
    password: string;
    confirmPassword: string;
    clientURI: string;
}