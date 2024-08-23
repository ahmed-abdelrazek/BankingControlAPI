﻿using MediatR;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BankingControlAPI.Features.Authorization.Commands.Tokens
{
    public sealed record RefreshTokenCommand : IRequest<EmptyHttpResult>
    {
        /// <summary>
        /// The <see cref="AccessTokenResponse.RefreshToken"/> from the last "/login" or "/refresh" response used to get a new <see cref="AccessTokenResponse"/>
        /// with an extended expiration.
        /// </summary>
        public required string RefreshToken { get; init; }
    }
}
