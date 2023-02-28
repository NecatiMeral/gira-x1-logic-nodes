namespace Necati_Meral_Yahoo_De.Logic.ComfortOnline;
public static class ComfortOnlineConsts
{
    public const string ComfortOnlineBaseAddress = "https://www.comfort-online.com/";

    public static class ErrorCodes
    {
        public const string Ok = "Ok";
        public const string InitialRequestFailed = "InitialRequestFailed";
        public const string MissingRequestVerificationToken = "MissingRequestVerificationToken";
        public const string InvalidCredentials = "InvalidCredentials";
        public const string LoginFailed = "LoginFailed";
        public const string UnexpectedError = "UnexpectedError: ";
    }
}
