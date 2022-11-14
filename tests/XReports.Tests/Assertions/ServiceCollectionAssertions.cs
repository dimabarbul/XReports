using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace XReports.Tests.Assertions
{
    public class ServiceCollectionAssertions : GenericCollectionAssertions<ServiceDescriptor>
    {
        public ServiceCollectionAssertions(IEnumerable<ServiceDescriptor> actualValue)
            : base(actualValue)
        {
        }

        protected override string Identifier => "service collection";

        public AndConstraint<ServiceCollectionAssertions> ContainDescriptor<TServiceType, TImplementationType>(ServiceLifetime lifetime)
            where TImplementationType : TServiceType
        {
            ServiceDescriptor serviceDescriptor = this.Subject.FirstOrDefault(sd => sd.ServiceType == typeof(TServiceType));
            serviceDescriptor.Should().NotBeNull("{context} should contain service descriptor for service type {0}", typeof(TServiceType));

            serviceDescriptor.Lifetime.Should().Be(lifetime);

            // When using factory, service descriptor doesn't have implementation type set.
            if (serviceDescriptor.ImplementationFactory == null)
            {
                serviceDescriptor.ImplementationType.Should().Be<TImplementationType>();
            }

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }

        public AndConstraint<ServiceCollectionAssertions> NotContainDescriptor<TServiceType>()
        {
            this.Subject.Should().NotContain(sd => sd.ServiceType == typeof(TServiceType));

            return new AndConstraint<ServiceCollectionAssertions>(this);
        }
    }
}
