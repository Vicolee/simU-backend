﻿using MediatR;
using SimU_GameService.Domain.Models;

namespace SimU_GameService.Application.Services.Queries;

public record GetChatQuery(Guid ChatId) : IRequest<Chat?>;