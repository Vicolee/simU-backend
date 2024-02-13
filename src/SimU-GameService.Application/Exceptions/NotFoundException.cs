namespace SimU_GameService.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entity) : base($"{entity} was not found.")
    {
    }

    public NotFoundException(string entity, Guid id)
        : base($"{entity} with ID {id} was not found.")
    {
    }

    public NotFoundException(string entity, string worldCode)
        : base($"{entity} with join code {worldCode} was not found.")
    {
    }
}