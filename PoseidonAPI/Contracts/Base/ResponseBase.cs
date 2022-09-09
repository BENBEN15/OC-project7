namespace PoseidonAPI.Contracts.Base
{
    public record ResponseBase(
        bool success,
        int code, 
        string formatedMessage,
        string message
       );
}
