# DemoPropertyAnnotationAttribute Class

**Namespace:** DemoProject

**Assembly:** DemoProject

An example of a custom attribute that is used to annotate a property with custom flags.

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.All)]
public class DemoPropertyAnnotationAttribute : Attribute
```

**Inheritance:** Object → Attribute → DemoPropertyAnnotationAttribute

**Attributes:** AttributeUsageAttribute

## Remarks

DemoPropertyAnnotationAttribute showcases the generated documentation for a custom attribute class             (in this case a attribute applicable to properties).             The same layout is used for all classes, see [DemoClass](../DemoClass/Type.md) for a more detailed description.

## Constructors

| Name                                                                  | Description                                                   |
| --------------------------------------------------------------------- | ------------------------------------------------------------- |
| [DemoPropertyAnnotationAttribute(DemoPropertyFlags)](Constructors.md) | Initializes a new instance of DemoPropertyAnnotationAttribute |

## See Also

- [Property2](../DemoClass/Properties/Property2.md)
