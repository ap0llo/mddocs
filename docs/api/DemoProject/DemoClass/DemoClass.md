# DemoClass Class

**Namespace:** DemoProject

**Assembly:** DemoProject

A class meant to demonstrate how the generated documentation looks like.

```csharp
public class DemoClass
```

**Inheritance:** Object → DemoClass

**Attributes:** DefaultMemberAttribute

## Remarks

The main purpose of this class is to showcase the generated documentation.

For that purpose, the class aims to include as many code constructs relevant for the generated documentation as possible.

For every type, MdDoc will create a seprate markdown page split into multiple sections.

The type page starts with basic info about the type. This includes the type's namespace and assembly as well as the inheritance hierarchy, implemented interfaces and applied attribues. The type info will be followed by the summary provided in the xml documentation comments.

If there are any remarks for the type, a "Remarks" section is added (the sections you are currently reading)

All of a types constructors will be listed in a table in the "Constructors" section. The table contains a line for every constructor displaying the constructors signature and summary. As there is a separate page generated that provides more detailed infor about the constructor, a link to that page is inserted.

Similar tables are generated for a type's public fields, events, properties, methods and operator overloads

Links to other members are supported (using the xml tag `see`), for example a link to [IDemoInterface](../IDemoInterface/IDemoInterface.md). References to types outside the assembly are written to the output             but cannot be linked to, e.g. a reference to String

The last section is the "See Also" section with links provided inline using the `seealso` tag

## Constructors

| Name                                        | Description                                                           |
| ------------------------------------------- | --------------------------------------------------------------------- |
| [DemoClass()](DemoClass-constructors.md)    | Initializes a new instance of DemoClass                               |
| [DemoClass(int)](DemoClass-constructors.md) | Initializes a new instance of DemoClass with the specified parameters |

## Fields

| Name                                 | Description    |
| ------------------------------------ | -------------- |
| [Field1](fields/DemoClass.Field1.md) | A public field |

## Events

| Name                                                         | Description                           |
| ------------------------------------------------------------ | ------------------------------------- |
| [OperationCompleted](events/DemoClass.OperationCompleted.md) | Raised when the operation is finished |

## Properties

| Name                                           | Description                                                      |
| ---------------------------------------------- | ---------------------------------------------------------------- |
| [Property1](properties/DemoClass.Property1.md) | Gets the value of [Property1](properties/DemoClass.Property1.md) |
| [Property2](properties/DemoClass.Property2.md) | Gets the value of [Property2](properties/DemoClass.Property2.md) |

## Indexers

| Name                                           | Description                                       |
| ---------------------------------------------- | ------------------------------------------------- |
| [Item\[int, int\]](indexers/DemoClass.Item.md) | An example of an indexer with two parameters.     |
| [Item\[int\]](indexers/DemoClass.Item.md)      | An example of an indexer with a single parameter. |

## Methods

| Name                                                      | Description         |
| --------------------------------------------------------- | ------------------- |
| [DoSomething(string)](methods/DemoClass.DoSomething.md)   | Does something ;)   |
| [DoSomethingElse()](methods/DemoClass.DoSomethingElse.md) | Does something else |

## Operators

| Name                                                              | Description                         |
| ----------------------------------------------------------------- | ----------------------------------- |
| [Addition(DemoClass, DemoClass)](operators/DemoClass.Addition.md) | Combines two instances of DemoClass |

## See Also

- [IDemoInterface](../IDemoInterface/IDemoInterface.md)
- String might also be interesting
- [DemoStruct has a similar purpose but is a value type](../DemoStruct/DemoStruct.md)
