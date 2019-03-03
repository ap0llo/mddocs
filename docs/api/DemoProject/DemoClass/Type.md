# DemoClass Class

**Namespace:** [DemoProject](../Namespace.md)

**Assembly:** DemoProject

A class meant to demonstrate how the generated documentation looks like.

```csharp
public class DemoClass
```

**Inheritance:** Object → DemoClass

## Remarks

The main purpose of this class is to showcase the generated documentation.

For that purpose, the class aims to include as many code constructs relevant for the generated documentation as possible.

For every type, MdDoc will create a separate markdown page split into multiple sections.

The type page starts with the "definition" section that provides basic info about the type. This includes the type's namespace and assembly as well as the inheritance hierarchy, implemented interfaces and applied attributes. The type info will be followed by the summary provided in the xml documentation comments.

If there are any remarks for the type, a "Remarks" section is added (the section you are currently reading)

All of a types constructors will be listed in a table in the "Constructors" section. The table contains a row for every constructor displaying the constructors signature and summary. As there is a separate page generated that provides more detailed info about the constructor, a link to that page is inserted.

Similar tables are generated for a type's public fields, events, properties, indexers, methods and operator overloads

Links to other members are supported (using the xml tag `see`), for example a link to [IDemoInterface](../IDemoInterface/Type.md). References to types outside the assembly are written to the output             but cannot be linked to, e.g. a reference to String

The last section is the "See Also" section with links provided in the xml documentation using the `seealso` tag

Similar pages are also generated for interfaces (see [IDemoInterface](../IDemoInterface/Type.md)), structs (see [DemoStruct](../DemoStruct/Type.md))             and enums (see  [DemoEnum](../DemoEnum/Type.md))

## Constructors

| Name                              | Description                                                           |
| --------------------------------- | --------------------------------------------------------------------- |
| [DemoClass()](Constructors.md)    | Initializes a new instance of DemoClass                               |
| [DemoClass(int)](Constructors.md) | Initializes a new instance of DemoClass with the specified parameters |

## Fields

| Name                       | Description                   |
| -------------------------- | ----------------------------- |
| [Field1](Fields/Field1.md) | An example of a public field. |
| [Field2](Fields/Field2.md) | An example of a public field. |

## Events

| Name                       | Description                   |
| -------------------------- | ----------------------------- |
| [Event1](Events/Event1.md) | An example of a public event. |
| [Event2](Events/Event2.md) | An example of a public event. |

## Properties

| Name                                 | Description                                                            |
| ------------------------------------ | ---------------------------------------------------------------------- |
| [Property1](Properties/Property1.md) | An example of a read\-only property.                                   |
| [Property2](Properties/Property2.md) | An example of a read\/write property annotated with a custom attribute |
| [Property3](Properties/Property3.md) | An example of an obsolete property.                                    |

## Indexers

| Name                                 | Description                                       |
| ------------------------------------ | ------------------------------------------------- |
| [Item\[int, int\]](Indexers/Item.md) | An example of an indexer with two parameters.     |
| [Item\[int\]](Indexers/Item.md)      | An example of an indexer with a single parameter. |
| [Item\[object\]](Indexers/Item.md)   | An example of an obsolete indexer.                |

## Methods

| Name                                  | Description                                                    |
| ------------------------------------- | -------------------------------------------------------------- |
| [Method1()](Methods/Method1.md)       | Example of an overloaded method without parameters             |
| [Method1(string)](Methods/Method1.md) | Example of an overloaded method accepting one parameter.       |
| [Method2()](Methods/Method2.md)       | Example of an non\-overloaded methods with a custom attribute. |
| [Method3\<T\>(T)](Methods/Method3.md) | Example of a generic method.                                   |
| [Method4()](Methods/Method4.md)       | Example of an obsolete method.                                 |

## Operators

| Name                                                          | Description                                       |
| ------------------------------------------------------------- | ------------------------------------------------- |
| [Addition(DemoClass, DemoClass)](Operators/Addition.md)       | Example of an overload of the binary + operator.  |
| [Subtraction(DemoClass, DemoClass)](Operators/Subtraction.md) | Example of an overload of the binary \- operator. |

## Example

Using the `example` tag, examples on how to use a type can be included in the documentation:

```
// create a new instance of DemoClass
var instance = new DemoClass();
```

## See Also

- [IDemoInterface](../IDemoInterface/Type.md)
- [By providing text in the `seealso` element, the link text can be changed](../DemoStruct/Type.md)
- [DemoEnum](../DemoEnum/Type.md)
- No link can be generated if the referenced type is defined in another assembly (`System.String` in this case)
