using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.Common.Assertions
{
    public class ServiceCollectionAssertions : GenericCollectionAssertions<ServiceDescriptor>
    {
        public ServiceCollectionAssertions(IEnumerable<ServiceDescriptor> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "service collection";

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptor<TService>(ServiceLifetime lifetime)
        {
            ServiceDescriptor serviceDescriptor = this.Subject.FirstOrDefault(sd => sd.ServiceType == typeof(TService));
            serviceDescriptor.Should().NotBeNull("{context} should contain service descriptor for service type {0}", typeof(TService));

            serviceDescriptor.Lifetime.Should().Be(lifetime);

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptor<TService, TImplementation>(ServiceLifetime lifetime)
            where TImplementation : TService
        {
            ServiceDescriptor serviceDescriptor = this.Subject.FirstOrDefault(sd => sd.ServiceType == typeof(TService));
            serviceDescriptor.Should().NotBeNull("{context} should contain service descriptor for service type {0}", typeof(TService));

            serviceDescriptor.Lifetime.Should().Be(lifetime);
            serviceDescriptor.ImplementationType.Should().Be<TImplementation>();

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptors<TService>(ServiceLifetime lifetime, params Type[] implementationTypes)
        {
            ServiceDescriptor[] descriptors = this.Subject
                .Where(sd => sd.ServiceType == typeof(TService))
                .ToArray();

            descriptors.Should().OnlyContain(d => d.Lifetime == lifetime);
            descriptors.Select(h => h.ImplementationType)
                .Should().BeEquivalentTo(implementationTypes);

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> NotContainDescriptor<TService>()
        {
            this.Subject.Should().NotContain(sd => sd.ServiceType == typeof(TService));

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }
    }
}
