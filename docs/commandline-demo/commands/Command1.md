# `Command1` Command

**Application:** [DemoProject](../commandline.md)  
**Version:** 1.2.3

Documentation for the 'command1' subcommand. For every subcommand, a separate page will be generated.  
The command page includes the commands description (provided as 'HelpText' on the \[Verb\] attribute.)

## Usage

```
DemoProject Command1 [<VALUE>]
                     [<VALUE>]
                     [--parameter1 <VALUE>]
                     [--parameter2|-x <VALUE>]
                     --parameter4 <VALUE>
                     [--parameter5 <VALUE>]
                     [-y <VALUE>]
```

## Parameters

| Position                   | Name                                | Short Name                 | Required | Description                                                                                          |
| -------------------------- | ----------------------------------- | -------------------------- | -------- | ---------------------------------------------------------------------------------------------------- |
| [0](#parameter-position-0) |                                     |                            | No       | Unnamed parameters (declared using the \[Value\] attribute) are identified by index instead of name. |
| [1](#parameter-position-1) |                                     |                            | No       | If a parameter has a default value, it will be included in the documentation, too                    |
|                            | [parameter1](#parameter1-parameter) |                            | No       | The description of the named parameter1 (Declared using the \[Option\] attribute).                   |
|                            | [parameter2](#parameter2-parameter) | [x](#parameter2-parameter) | No       | For named parameters "short" (a single character like 'x') and "long" names are supported.           |
|                            | [parameter4](#parameter4-parameter) |                            | Yes      | This is an example of a mandatory parameter                                                          |
|                            | [parameter5](#parameter5-parameter) |                            | No       | Is a parameter is an enum, the list of accepted values will be included in the documentation.        |
|                            |                                     | [y](#y-parameter)          | No       | Parameters without a long name are supported as well.                                                |

### Parameter (Position 0)

Unnamed parameters (declared using the \[Value\] attribute) are identified by index instead of name.

|                |        |
| -------------- | ------ |
| Position:      | 0      |
| Required:      | No     |
| Default value: | *None* |
___

### Parameter (Position 1)

If a parameter has a default value, it will be included in the documentation, too

|                |                |
| -------------- | -------------- |
| Position:      | 1              |
| Required:      | No             |
| Default value: | MyDefaultValue |
___

### `parameter1` Parameter

The description of the named parameter1 (Declared using the \[Option\] attribute).

|                |                   |
| -------------- | ----------------- |
| Name:          | parameter1        |
| Position:      | *Named parameter* |
| Required:      | No                |
| Default value: | *None*            |
___

### `parameter2` Parameter

For named parameters "short" (a single character like 'x') and "long" names are supported.

|                |                   |
| -------------- | ----------------- |
| Name:          | parameter2        |
| Short name:    | x                 |
| Position:      | *Named parameter* |
| Required:      | No                |
| Default value: | *None*            |
___

### `parameter4` Parameter

This is an example of a mandatory parameter

|                |                   |
| -------------- | ----------------- |
| Name:          | parameter4        |
| Position:      | *Named parameter* |
| Required:      | Yes               |
| Default value: | *None*            |
___

### `parameter5` Parameter

Is a parameter is an enum, the list of accepted values will be included in the documentation.

|                  |                                                    |
| ---------------- | -------------------------------------------------- |
| Name:            | parameter5                                         |
| Position:        | *Named parameter*                                  |
| Required:        | No                                                 |
| Accepted values: | `AcceptedValue1`, `AcceptedValue2`, `AnotherValue` |
| Default value:   | *None*                                             |
___

### `y` Parameter

Parameters without a long name are supported as well.

|                |                   |
| -------------- | ----------------- |
| Name:          | y                 |
| Position:      | *Named parameter* |
| Required:      | No                |
| Default value: | *None*            |
___

*Documentation generated by [MdDocs](https://github.com/ap0llo/mddocs)*
