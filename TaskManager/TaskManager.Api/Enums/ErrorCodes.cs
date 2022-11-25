namespace TaskManager.Api.Enums
{
    public enum ErrorCodes
    {
        NoError = 0,
        UnknownError = 555000,
        InvalidModel = 555002,
        EntityNotFound = 555004,
        ErrorWhileSaving = 555011,
        InternalServerException = 555017,
        ItemWasDeleted = 555023,
        ConflictInTrasaction = 555024,
        InvalidFileType = 555025
    }
}
