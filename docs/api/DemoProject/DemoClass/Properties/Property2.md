# DemoClass.Property2 Property

**Declaring Type:** [DemoClass](../Type.md)

An example of a read\/write property annotated with a custom attribute

```csharp
[DemoPropertyAnnotation(DemoPropertyFlags.Flag2 | DemoPropertyFlags.Flag3)]
public string Property2 { get; set; }
```

## Property Value

String

The tag `value` allows specifying the value a property represents

## Remarks

Remarks allow specification of more detailed information about a member, in this case, a property supplementing the information specified in the summary

## See Also

- [Property1](Property1.md)