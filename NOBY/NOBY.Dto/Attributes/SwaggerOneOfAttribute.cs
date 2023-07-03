namespace NOBY.Dto.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SwaggerOneOfAttribute : Attribute
{
    public Type[] PossibleTypes { get; }

    public SwaggerOneOfAttribute(params Type[] possibleTypes)
    {
        PossibleTypes = possibleTypes;
    }
}

public class SwaggerOneOfAttribute<TType> : SwaggerOneOfAttribute
{
    public SwaggerOneOfAttribute() : base(typeof(TType))
    {
    }
}

public class SwaggerOneOfAttribute<T1, T2> : SwaggerOneOfAttribute
{
    public SwaggerOneOfAttribute() : base(typeof(T1), typeof(T2))
    {
    }
}

public class SwaggerOneOfAttribute<T1, T2, T3> : SwaggerOneOfAttribute
{
    public SwaggerOneOfAttribute() : base(typeof(T1), typeof(T2), typeof(T3))
    {
    }
}

public class SwaggerOneOfAttribute<T1, T2, T3, T4> : SwaggerOneOfAttribute
{
    public SwaggerOneOfAttribute() : base(typeof(T1), typeof(T2), typeof(T3), typeof(T4))
    {
    }
}

public class SwaggerOneOfAttribute<T1, T2, T3, T4, T5> : SwaggerOneOfAttribute
{
    public SwaggerOneOfAttribute() : base(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))
    {
    }
}