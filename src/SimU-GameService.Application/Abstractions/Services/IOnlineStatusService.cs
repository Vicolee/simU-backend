namespace SimU_GameService.Application.Abstractions.Services;
public interface IOnlineStatusService
{
    void CheckOnlineStatus();
    void SetOnlineStatus(Guid userId);
}
