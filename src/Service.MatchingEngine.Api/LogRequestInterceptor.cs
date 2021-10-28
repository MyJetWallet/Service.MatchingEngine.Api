using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

namespace Service.MatchingEngine.Api
{
    public class LogRequestInterceptor : Interceptor
    {
        private readonly ILogger<LogRequestInterceptor> _logger;

        public LogRequestInterceptor(ILogger<LogRequestInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var response = await base.UnaryServerHandler(request, context, continuation);

                _logger.LogInformation("Call [{path}]. Request: {request}. Response: {response}", 
                    context.Method, 
                    JsonConvert.SerializeObject(request),
                    JsonConvert.SerializeObject(response));
                
                return response;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Call EXCEPTION [{path}]. Request: {request}. Exception: {text}", 
                    context.Method, 
                    JsonConvert.SerializeObject(request),
                    e.Message);
                
                throw;
            }
        }
    }
}