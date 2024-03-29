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

    public NotFoundException(string entity, string identifier)
        : base($"{entity} with identifier {identifier} was not found.")
    {
    }
}