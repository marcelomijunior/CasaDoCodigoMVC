﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CasaDoCodigo.IdentityServer4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("ApiScope1"),
                new ApiScope("CasaDoCodigo.RelatorioWebApi")
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            string casaDoCodigoMvcUrl = configuration["CasaDoCodigoMvcUrl"];

            return new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "Client",
                    ClientName = "Client Credentials Name",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "CasaDoCodigo.MVC",
                    ClientName = "Casa do Código MVC",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,

                    RedirectUris = { $"{casaDoCodigoMvcUrl}/signin-oidc" },
                    FrontChannelLogoutUri = $"{casaDoCodigoMvcUrl}/signout-oidc",
                    PostLogoutRedirectUris = { $"{casaDoCodigoMvcUrl}/signout-callback-oidc" },

                    AllowedScopes = { "openid", "profile", "email", "CasaDoCodigo.RelatorioWebApi" },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse
                },
            };
        }
    }
}