using CIS.Testing.Attributes;

namespace CIS.Infrastructure.Caching.Tests;

public class GlobalCacheStringTest : BaseGlobalCacheTest
{
    [Theory]
    [InlineAutoMoqData("redis")]
    [InlineAutoMoqData("inmemory")]
    public void String_AddAndGet_ShouldReturnSameValue(string provider, string key, string value)
    {
        var cache = getCache(provider);
        cache.Set(key, value);
        Assert.Equal(value, cache.GetString(key));
    }

    [Theory]
    [InlineAutoMoqData("redis")]
    [InlineAutoMoqData("inmemory")]
    public void String_Update_ShouldReturnNewValue(string provider, string key, string value, string update)
    {
        var cache = getCache(provider);
        cache.Set(key, value);
        cache.Set(key, update);
        Assert.Equal(update, cache.GetString(key));
    }

    [Theory]
    [InlineData("redis")]
    [InlineData("inmemory")]
    public void String_NullValue_ShouldThrowException(string provider)
    {
        var cache = getCache(provider);
        Assert.Throws<Core.Exceptions.CisArgumentNullException>(() =>
        {
            cache.Set("t1", new HashItem("", ""));
        });
    }

    [Theory]
    [InlineData("redis")]
    [InlineData("inmemory")]
    public void String_Exists_ShouldPass(string provider)
    {
        var cache = getCache(provider);
        cache.Set("t1", "v1");

        Assert.True(cache.Exists("t1"));
        Assert.False(cache.Exists("t2"));
    }

    [Theory]
    [InlineData("redis")]
    [InlineData("inmemory")]
    public void String_Remove_ShouldPass(string provider)
    {
        var cache = getCache(provider);
        cache.Set("t1", "v1");
        cache.Remove("t1");
        cache.Remove("t2");
        Assert.False(cache.Exists("t1"));
    }
}
