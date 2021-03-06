﻿using AutoMapper;
using BTB.Application.Common.Mappings;

namespace Application.UnitTests.Mappings
{
    public class MappingTestsFixture
    {
        public IConfigurationProvider ConfigurationProvider { get; }
        public IMapper Mapper { get; }

        public MappingTestsFixture()
        {
            ConfigurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            Mapper = ConfigurationProvider.CreateMapper();
        }
    }
}