namespace PoseidonAPI.Contracts.Error
{
    public record IdNotFound(int not_found);
    public record ErrorMessage(string message);
    public record IdNotDeleted(int not_deleted);
}
