﻿using Microsoft.Azure.Databricks.Client.Converters;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Test
{
    public abstract class ApiClientTest
    {

        public static readonly Uri BaseApiUri = new("https://test-server/api/");

        protected static readonly JsonSerializerOptions options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = {
                new JsonStringEnumConverter(),
                new MillisecondEpochDateTimeConverter(),
                new LibraryConverter(),
                new SecretScopeConverter()
            }
        };

        protected static void AssertJsonDeepEquals(string expected, string actual)
        {
            Assert.AreEqual<EquatableJToken>(JToken.Parse(expected), JToken.Parse(actual));
        }

        protected static void RequestContentMatch(HttpRequestMessage request, string expected)
        {
            string actual = request.Content?.ReadAsStringAsync().Result!;
            AssertJsonDeepEquals(expected, actual);
        }

        protected static Predicate<HttpRequestMessage> GetMatcher(string expectedRequest)
        {
            return new Predicate<HttpRequestMessage>(request =>
            {
                RequestContentMatch(request, expectedRequest);
                return true;
            });
        }

        protected Mock<HttpMessageHandler> CreateMockHandler()
        {
            Mock<HttpMessageHandler> handler = new();
            handler
                .SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.NotFound);

            return handler;
        }
    }
}