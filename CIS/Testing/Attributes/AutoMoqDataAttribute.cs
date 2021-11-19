using AutoFixture;
using AutoFixture.Xunit2;

namespace CIS.Testing.Attributes
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoFixture.AutoMoq.AutoMoqCustomization()))
        {
        }
    }
}
