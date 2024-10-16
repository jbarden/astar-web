﻿using System.Net;

namespace AStar.Web.UI.MockMessageHandlers;

public class MockInternalServerErrorHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
}
