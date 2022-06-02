export interface UserForRegistrationDto {
    userName: string;
    mainCurrencyCode: string;
    email: string;
    password: string;
    confirmPassword: string;
    clientURI: string;
}