# DemoClass.Item Indexer

**Declaring Type:** [DemoClass](../Type.md)

## Overloads

| Signature        | Description                                       |
| ---------------- | ------------------------------------------------- |
| Item\[int, int\] | An example of an indexer with two parameters.     |
| Item\[int\]      | An example of an indexer with a single parameter. |

## Item\[int, int\]

An example of an indexer with two parameters.

```csharp
public string this[int x, int y] { get; }
```

### Parameters

`x`  Int32

Description of parameter `x` provided using the `param` element.

`y`  Int32

Description of parameter `y` provided using the `param` element.

### Indexer Value

String

The tag `value` allows specifying the value a indexer represents

### Remarks

Remarks allow specification of more detailed information about a member, in this case the indexer. supplementing the information specified in the summary.

For overloaded members, there is a separate "Remarks" section for every overload.

## Item\[int\]

An example of an indexer with a single parameter.

```csharp
public string this[int index] { get; }
```

### Parameters

`index`  Int32

Description of parameter `index` provided using the `param` element.

### Indexer Value

String

The tag `value` allows specifying the value a indexer represents

### Remarks

Remarks allow specification of more detailed information about a member, in this case the indexer. supplementing the information specified in the summary.

For overloaded members, there is a separate "Remarks" section for every overload.

### Exceptions

ArgumentException

Exceptions can be documented using the `exception` tag.

InvalidOperationException

### Example

Using the `example` tag, examples on how to call a member can be included in the documentation:

```
var instance = new DemoClass();
var value = instance[42];
```

### See Also

- Item\[int, int\]
