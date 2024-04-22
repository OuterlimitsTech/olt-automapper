using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.AutoMapper.Tests.Adapters;
using OLT.DataAdapters.AutoMapper.Tests.Assets.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Xunit;

namespace OLT.DataAdapters.AutoMapper.Tests
{
    public class ExceptionTests 
    {
        private const string DefaultMessage = "Test Error";

        [Fact]
        public void AdapterNotFoundException()
        {
            var ex = new OltAutoMapperException<AdapterObject1, AdapterObject2>(new AutoMapperMappingException(DefaultMessage));
            Assert.Equal($"AutoMapper Exception while using map {nameof(IOltAdapterMap<AdapterObject1, AdapterObject2>)}: {typeof(AdapterObject1).FullName} -> {typeof(AdapterObject2).FullName} {Environment.NewLine}{DefaultMessage}", ex.Message);
        }

        [Fact]
        public void AdapterNotFoundExceptionTyped()
        {
            var ex = new OltAutoMapperException<AdapterObject1, AdapterObject2>(new Exception(DefaultMessage));
            Assert.Equal($"AutoMapper Exception while using map {nameof(IOltAdapterMap<AdapterObject1, AdapterObject2>)}: {typeof(AdapterObject1).FullName} -> {typeof(AdapterObject2).FullName}", ex.Message);
        }

    }
}