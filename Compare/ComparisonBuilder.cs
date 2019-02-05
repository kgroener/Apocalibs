using Compare.Contracts;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Compare
{

    public class Instrument
    {
        public DateTime LastUpdated { get; set; }

        public int ID { get; set; }

        public string SerialNumber { get; set; }
    }

    public class Fluidset
    {
        public DateTime LastUpdated { get; set; }

    }

    public class Dummy
    {
        public Dummy()
        {

            var comparisonConstruction = ComparisonBuilder.Build(builder => builder
                .SetupDefaultComparison<int>(s =>
                    s.When((a, b) => a != b, Then.SetError("")))
                .SetupDefaultComparison<double>(s =>
                    s.When(DoublesAreNotEqual, Then.SetError("")))
                .SetupComparisonOf<Instrument>(s =>
                    s.IgnoreProperty(i => i.LastUpdated)
                     .IgnoreProperty(i => i.ID)
                     .SetupPropertyComparison(i => i.SerialNumber, (p) =>
                        p.When((a, b) => a == b, Then.MarkAsEqual)
                         .When((a, b) => a.StartsWith(b) || b.StartsWith(a), Then.SetWarning(""))
                         .Else(Then.SetError(""))))
                .SetupComparisonOf<Fluidset>(s =>
                    s.IgnoreProperty(f => f.LastUpdated)));


            var results = comparisonConstruction.Compare(new Instrument(), new Instrument());

        }

        private bool DoublesAreNotEqual(double a, double b)
        {
            return a.ToString("G5") != b.ToString("G5");
        }

    }

    public sealed class ComparisonBuilder : IComparisonBuilder
    {
        private readonly List<IComparisonConfiguration> _defaultComparisonConfigurations;
        private readonly List<MemberInfo> _ignorableProperties;
        private readonly Dictionary<MemberInfo, List<IComparisonConfiguration>> _propertyComparisonConfigurations;



        private ComparisonBuilder()
        {
            _defaultComparisonConfigurations = new List<IComparisonConfiguration>();
        }

        public static IComparisonConstruction Build(Action<IComparisonBuilder> builder)
        {
            var comparisonBuilder = new ComparisonBuilder();

            builder(comparisonBuilder);

            return comparisonBuilder.Construct();

        }

        private IComparisonConstruction Construct()
        {
            throw new NotImplementedException();
        }

        public IComparisonBuilder SetupComparisonOf<TClass>(Action<IClassComparisonBuilder<TClass>> classComparisonBuilder)
        {
            var classComparison = new ClassComparisonBuilder<TClass>();

            classComparisonBuilder(classComparison);

            _ignorableProperties.AddRange(classComparison.IgnorableProperties);

            foreach (var propertyComparison in classComparison.PropertyComparisons)
            {
                _propertyComparisonConfigurations.Add(propertyComparison.Key, propertyComparison.Value);
            }

            return this;
        }

        public IComparisonBuilder SetupDefaultComparison<T>(Action<IComparisonConfiguration<T>> comparisonConfiguration)
        {
            var configuration = new ComparisonConfiguration<T>();

            comparisonConfiguration(configuration);

            _defaultComparisonConfigurations.Add(configuration);

            return this;
        }
    }
}
