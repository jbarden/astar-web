﻿using System.Net;
using System.Text.Json;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace AStar.Web.UI.Unit.Tests.MockMessageHandlers;

public static class MockHttpClientBunitHelpers
{
    public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services)
    {
        var mockHttpHandler = new MockHttpMessageHandler();
        var httpClient = mockHttpHandler.ToHttpClient();
        httpClient.BaseAddress = new("http://localhost");
        services.AddSingleton(httpClient);
        return mockHttpHandler;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
    {
        request.Respond(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(content))
            };
            response.Content.Headers.ContentType = new("application/json");
            return response;
        });
        return request;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
    {
        request.Respond(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(contentProvider()))
            };
            response.Content.Headers.ContentType = new("application/json");
            return response;
        });
        return request;
    }
}
