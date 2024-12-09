using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using AutoMapper;
using System;
using RestSharp;
using RestSharp.Authenticators;

namespace PRJ4.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _domain;
        public EmailService(string apiKey,string domain)
        {
            _apiKey = apiKey;
            _domain = domain;
        }   

        public RestResponse SendSimpleMessage(string to, string subject, string text)
        {
             // Create RestClient with Base URL and Authenticator
            var client = new RestClient(new RestClientOptions
            {
                BaseUrl = new Uri($"https://api.mailgun.net/v3/"),
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            });

            var request = new RestRequest();

            request.AddParameter ("domain", "sandboxf55113ec9eef4f6580a316b419167ded.mailgun.org", 
                ParameterType.UrlSegment);
            request.Resource = "sandboxf55113ec9eef4f6580a316b419167ded.mailgun.org/messages";
            request.AddParameter ("from", "Excited User <mailgun@sandboxf55113ec9eef4f6580a316b419167ded.mailgun.org>");
            request.AddParameter("to", to);
            request.AddParameter("subject", subject); 
            request.AddParameter("text", text);

            request.Method = Method.Post;

            return client.Execute(request);

        }

    }
}