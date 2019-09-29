# DemoClass Class

**Namespace:** [DemoProject](../index.md)

**Assembly:** DemoProject

A class meant to demonstrate how the generated documentation looks like.

```csharp
public class DemoClass
```

**Inheritance:** object → DemoClass

## Remarks

The main purpose of this class is to showcase the generated documentation.

For that purpose, the class aims to include as many code constructs relevant for the generated documentation as possible.

For every type, MdDoc will create a separate markdown page split into multiple sections.

The type page starts with the "definition" section that provides basic info about the type. This includes the type's namespace and assembly as well as the inheritance hierarchy, implemented interfaces and applied attributes. The type info will be followed by the summary provided in the xml documentation comments.

If there are any remarks for the type, a "Remarks" section is added (the section you are currently reading)

All of a types constructors will be listed in a table in the "Constructors" section. The table contains a row for every constructor displaying the constructors signature and summary. As there is a separate page generated that provides more detailed info about the constructor, a link to that page is inserted.

Similar tables are generated for a type's public fields, events, properties, indexers, methods and operator overloads

Links to other members are supported (using the xml tag `see`), for example a link to [IDemoInterface](../IDemoInterface/index.md). References to types outside the assembly are written to the output but cannot be linked to, e.g. a reference to string

For the `see` tag alternative text is supported as well. To specify the link text, insert content inside the `see` element, for example `<see cref="IDemoInterface">Custom text</see>` which is rendered like this: [Custom text](../IDemoInterface/index.md)

The last section is the "See Also" section with links provided in the xml documentation using the `seealso` tag

Similar pages are also generated for interfaces (see [IDemoInterface](../IDemoInterface/index.md)), structs (see [DemoStruct](../DemoStruct/index.md)) and enums (see  [DemoEnum](../DemoEnum/index.md))Documentation can also contains list and tables. See [ListExample](../ListExample/index.md) for a showcase of the different supported list formats.

## Constructors

| Name                                                 | Description                                                           |
| ---------------------------------------------------- | --------------------------------------------------------------------- |
| [DemoClass()](constructors/index.md#democlass)       | Initializes a new instance of DemoClass                               |
| [DemoClass(int)](constructors/index.md#democlassint) | Initializes a new instance of DemoClass with the specified parameters |

## Fields

| Name                       | Description                   |
| -------------------------- | ----------------------------- |
| [Field1](fields/Field1.md) | An example of a public field. |
| [Field2](fields/Field2.md) | An example of a public field. |

## Events

| Name                       | Description                   |
| -------------------------- | ----------------------------- |
| [Event1](events/Event1.md) | An example of a public event. |
| [Event2](events/Event2.md) | An example of a public event. |

## Properties

| Name                                 | Description                                                            |
| ------------------------------------ | ---------------------------------------------------------------------- |
| [Property1](properties/Property1.md) | An example of a read\-only property.                                   |
| [Property2](properties/Property2.md) | An example of a read\/write property annotated with a custom attribute |
| [Property3](properties/Property3.md) | An example of an obsolete property.                                    |

## Indexers

| Name                                             | Description                                       |
| ------------------------------------------------ | ------------------------------------------------- |
| [Item\[int, int\]](indexers/Item.md#itemint-int) | An example of an indexer with two parameters.     |
| [Item\[int\]](indexers/Item.md#itemint)          | An example of an indexer with a single parameter. |
| [Item\[object\]](indexers/Item.md#itemobject)    | An example of an obsolete indexer.                |

## Methods

| Name                                                | Description                                                    |
| --------------------------------------------------- | -------------------------------------------------------------- |
| [Method1()](methods/Method1.md#method1)             | Example of an overloaded method without parameters             |
| [Method1(string)](methods/Method1.md#method1string) | Example of an overloaded method accepting one parameter.       |
| [Method2()](methods/Method2.md)                     | Example of an non\-overloaded methods with a custom attribute. |
| [Method3\<T\>(T)](methods/Method3.md)               | Example of a generic method.                                   |
| [Method4()](methods/Method4.md)                     | Example of an obsolete method.                                 |

## Operators

| Name                                                          | Description                                       |
| ------------------------------------------------------------- | ------------------------------------------------- |
| [Addition(DemoClass, DemoClass)](operators/Addition.md)       | Example of an overload of the binary + operator.  |
| [Subtraction(DemoClass, DemoClass)](operators/Subtraction.md) | Example of an overload of the binary \- operator. |

## Example

Using the `example` tag, examples on how to use a type can be included in the documentation. To specify the language inside of a `code` element, use the `language` attribute.

```csharp
// create a new instance of DemoClass
var instance = new DemoClass();
```

## See Also

- [IDemoInterface](../IDemoInterface/index.md)
- [By providing text in the `seealso` element, the link text can be changed](../DemoStruct/index.md)
- [DemoEnum](../DemoEnum/index.md)
- No link can be generated if the referenced type is defined in another assembly (`System.String` in this case)
- [ListExample](../ListExample/index.md)

___

*Documentation generated by [MdDocs](https://github.com/ap0llo/mddocs)*
