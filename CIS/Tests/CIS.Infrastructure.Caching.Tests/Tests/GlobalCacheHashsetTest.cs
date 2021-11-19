using CIS.Testing.Attributes;

namespace CIS.Infrastructure.Caching.Tests;

public class GlobalCacheHashsetTest : BaseGlobalCacheTest
{
    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_AddAndExist_ShouldExist(string provider, string key, HashItem item)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });
        Assert.True(cache.Exists(key));
    }

    [Theory]
    [InlineData("inmemory")]
    [InlineData("redis")]
    public void Hashset_InvalidKey_ShouldThrowException(string provider)
    {
        var cache = getCache(provider);
        Assert.Throws<Core.Exceptions.CisArgumentNullException>(() => cache.GetHashset("invalid_key"));
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_Update_ShouldUpdateSet(string provider, string key, HashItem item, HashItem item2)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });
        cache.Set(key, new List<HashItem>() { item, item2 });
        Assert.Equal(2, cache.GetHashset(key).Count);
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_Get_ReturnsAllItems(string provider, string key, HashItem item, HashItem item2)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item, item2 });

        var list = cache.GetHashset(key);
        Assert.Equal(list.Select(t => t.Name).ToArray(), new[] { item.Name, item2.Name });
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_Get_ReturnsSingleItems(string provider, string key, HashItem item, HashItem item2)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item, item2 });
        var value = cache.GetHashset(key, item.Name);
        Assert.Equal(item.Value, value);
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_GetNonExistingName_ShouldThrowException(string provider, string key, HashItem item)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });
        Assert.Throws<Core.Exceptions.CisArgumentNullException>(() => cache.GetHashset(key, "non_existing_name"));
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_SetSingleItem_ShouldOnlyUpdate(string provider, string key, string updatedValue, HashItem item)
    {
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });
            
        var item2 = new HashItem(item.Name, updatedValue);
        cache.Set(key, item2);

        string getValue = cache.GetHashset(key, item.Name);
        Assert.Equal(item2.Value, getValue);
        Assert.Single(cache.GetHashset(key).Where(t => t.Name == item.Name));
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_TryGetHashsetWithWrongKey_ShouldReturnFalse(string provider)
    {
        // arrange
        var cache = getCache(provider);
            
        // act
        bool result = cache.TryGetHashset("wrongkey", "wrongfield", out string v);
            
        // assert
        Assert.False(result);
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_TryGetHashsetWithWrongName_ShouldReturnFalse(string provider, string key, HashItem item)
    {
        // arrange
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });

        // act
        bool result = cache.TryGetHashset(key, "wrongfield", out string v);

        // assert
        Assert.False(result);
    }

    [Theory]
    [InlineAutoMoqData("inmemory")]
    [InlineAutoMoqData("redis")]
    public void Hashset_TryGetHashset_ShouldReturnValue(string provider, string key, HashItem item)
    {
        // arrange
        var cache = getCache(provider);
        cache.Set(key, new List<HashItem>() { item });

        // act
        bool result = cache.TryGetHashset(key, item.Name, out string v);

        // assert
        Assert.True(result);
        Assert.Equal(item.Value, v);
    }
}
