using PRJ4.Models;
using PRJ4.Repositories;
using PRJ4.DTOs;
using AutoMapper;
using System;
using RestSharp;
using RestSharp.Authenticators;

namespace PRJ4.Services
{
    public interface IEmailService
    {
        public RestResponse SendSimpleMessage( string to, string subject, string text);
    }
}