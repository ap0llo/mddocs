# DemoClass Class

**Namespace:** DemoProject

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

Links to other members are supported (using the xml tag `see`), for example a link to [IDemoInterface](../IDemoInterface/IDemoInterface.md). References to types outside the assembly are written to the output             but cannot be linked to, e.g. a reference to String

The last section is the "See Also" section with links provided in the xml documentation using the `seealso` tag

Similar pages are also generated for interfaces (see [IDemoInterface](../IDemoInterface/IDemoInterface.md)), structs (see [DemoStruct](../DemoStruct/DemoStruct.md))             and enums (see  [DemoEnum](../DemoEnum/DemoEnum.md))

## Constructors

| Name                                        | Description                                                           |
| ------------------------------------------- | --------------------------------------------------------------------- |
| [DemoClass()](DemoClass-constructors.md)    | Initializes a new instance of DemoClass                               |
| [DemoClass(int)](DemoClass-constructors.md) | Initializes a new instance of DemoClass with the specified parameters |

## Fields

| Name                                 | Description                   |
| ------------------------------------ | ----------------------------- |
| [Field1](fields/DemoClass.Field1.md) | An example of a public field. |
| [Field2](fields/DemoClass.Field2.md) | An example of a public field. |

## Events

| Name                                 | Description                   |
| ------------------------------------ | ----------------------------- |
| [Event1](events/DemoClass.Event1.md) | An example of a public event. |
| [Event2](events/DemoClass.Event2.md) | An example of a public event  |

## Properties

| Name                                           | Description                                                            |
| ---------------------------------------------- | ---------------------------------------------------------------------- |
| [Property1](properties/DemoClass.Property1.md) | An example of a read\-only property.                                   |
| [Property2](properties/DemoClass.Property2.md) | An example of a read\/write property annotated with a custom attribute |

## Indexers

| Name                                           | Description                                       |
| ---------------------------------------------- | ------------------------------------------------- |
| [Item\[int, int\]](indexers/DemoClass.Item.md) | An example of an indexer with two parameters.     |
| [Item\[int\]](indexers/DemoClass.Item.md)      | An example of an indexer with a single parameter. |

## Methods

| Name                                            | Description                                                     |
| ----------------------------------------------- | --------------------------------------------------------------- |
| [Method1()](methods/DemoClass.Method1.md)       | Example of an overloaded method without parameters              |
| [Method1(string)](methods/DemoClass.Method1.md) | Example of an overloaded method accepting one parameter.        |
| [Method2()](methods/DemoClass.Method2.md)       | Example of an non\-overloaded methods with a custom attribute\- |

## Operators

| Name                                                                    | Description                                       |
| ----------------------------------------------------------------------- | ------------------------------------------------- |
| [Addition(DemoClass, DemoClass)](operators/DemoClass.Addition.md)       | Example of an overload of the binary + operator.  |
| [Subtraction(DemoClass, DemoClass)](operators/DemoClass.Subtraction.md) | Example of an overload of the binary \- operator. |

## See Also

- [IDemoInterface](../IDemoInterface/IDemoInterface.md)
- [IDemoInterface](../IDemoInterface/IDemoInterface.md)
- [By providing text in the `seealso` element, the link text can be changed](../DemoStruct/DemoStruct.md)
- No link can be generated if the referenced type is defined in another assembly (`System.String` in this case)
