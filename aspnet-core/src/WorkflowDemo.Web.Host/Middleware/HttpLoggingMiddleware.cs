// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Microsoft.AspNetCore.HttpLogging
{
    /// <summary>
    /// Middleware that logs HTTP requests and HTTP responses.
    /// </summary>
    internal sealed class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const int DefaultRequestFieldsMinusHeaders = 7;
        private const int DefaultResponseFieldsMinusHeaders = 2;
        private const string Redacted = "[Redacted]";

        /// <summary>
        /// Initializes <see cref="HttpLoggingMiddleware" />.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the <see cref="HttpLoggingMiddleware" />.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>HttpResponseLog.cs
        public Task Invoke(HttpContext context)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                // Logger isn't enabled.
                return _next(context);
            }

            return InvokeInternal(context);
        }

        private async Task InvokeInternal(HttpContext context)
        {
            var request = context.Request;

            await _next(context);
        }
    }
}