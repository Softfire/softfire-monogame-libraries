![Logo of the project]()

# Softfire Core Library - Bitwise
> Filename: Softfire.MonoGame.CORE.dll  
> Requires: .Net 4.7+, MonoGame 3.7+  
> Sources: [.Net](https://www.microsoft.com/en-us/download/details.aspx?id=55170), [MonoGame](http://www.monogame.net)

This portion contains a breakdown of bitwise operations.

## Table of Contents

- [Main](README.md)

## Bitwise Operators - Basic

```C#
A = 0011 1100
B = 0000 1101
C = 0000 0000
```

<center>

| Operator | Description                                                                 | Example                                                                                |
| :------: | --------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
|     &    | AND: Returns the bits that match in both variables.                         | <center> C = (A \| B) <br> A \| 0011 1100 <br> B \| 0000 1101 <br> C \| 0000 1100  </center> |
|    \|    | OR: Returns the bits that exist in each variable.                           | <center> C = (A & B) <br> A \| 0011 1100 <br> B \| 0000 1101 <br> C \| 0011 1101  </center>  |
|     ^    | XOR: Returns the unique bits from each variable.                            | <center> C = (A ^ B) <br> A \| 0011 1100 <br> B \| 0000 1101 <br> C \| 0011 0001  </center>  |
|     ~    | Negate: Returns the flipped bits of the variable.                           | <center> C = ~A <br> A \| 0011 1100 <br> C \| 1100 0011  </center>                           |
|    <<    | Shift Left: Returns the bits after they've been shifted to the left by x.   | <center> C = A << 1 <br> A \| 0011 1100 <br> C \| 0111 1000  </center>                       |
|    >>    | Shift Right: Returns the bits after they've been shifted to the right by x. | <center> C = A >> 1 <br> A \| 0011 1100 <br> C \| 0001 1110  </center>                       |
|    &=    | AND with Assignment: Equivalent to A = (A & B).                             | <center> A &= B <br> A \| 0011 1100 <br> B \| 0000 1101 <br> A \| 0000 1100  </center>       |
|   \|=    | OR with Assignment: Equivalent to A = (A \| B).                             | <center> A \|= B <br> A \| 0011 1100 <br> B \| 0000 1101 <br> A \| 0011 1101  </center>      |
|    ^=    | XOR with Assignment: Equivalent to A = (A ^ B).                             | <center> A ^= B <br> A \| 0011 1100 <br> B \| 0000 1101 <br> A \| 0011 0001  </center>       |
|   <<=    | Shift Left with Assignment: Equivalent to A = (A << 1).                     | <center> A <<= 1 <br> A \| 0011 1100 <br> A \| 0111 1000  </center>                          |
|   >>=    | Shift Right with Assignment: Equivalent to A = (A >> 1).                    | <center> A >>= 1 <br> A \| 0011 1100 <br> A \| 0111 1000  </center>                          |

</center>

## Bitwise Operators - Enum Flags

Enum flags are enum entries with unique bit assignments.  
A Enum that can be used as flags is described below.

```C#
[Flags]
public enum Colors : byte
{
    Blue   = 1  // (0000 0001) or (1 << 0)
    Green  = 2  // (0000 0010) or (1 << 1)
    Red    = 4  // (0000 0100) or (1 << 2)
    Yellow = 8  // (0000 1000) or (1 << 3)
    All    = 15 // (0000 1111) or (Blue | Green | Red | Yellow)
}

public Colors ColorFlags { get; set; } = (Colors.Blue | Colors.Red | Color.Green);
```

>ColorFlags is currently 0000 0111 as Colors.Yellow is not flagged.

<center>

| Operation                      | Description                                   | Example                                                                        |
| :----------------------------: | --------------------------------------------- | ------------------------------------------------------------------------------ |
| Flags & EnumEntry == EnumEntry | Contains: Check to see if the flag(s) exists. | if ((ColorFlags & Colors.Blue) == Colors.Blue) <br> Result: true               |
| Flags \|= EnumEntry            | Add: Adds the flag.                           | ColorFlags \|= Colors.Yelow <br> ColorsFlags = 0000 1111                       |
| Flags & ~EnumEntry             | Removes: Removes the flag.                    | ColorFlags &= ~Colors.Green <br> ColorFlags = 0000 1101                        |

</center>

## Document Informaton

Version: `1.0`  
Author: `Softfire`  
Updated: `Mar. 7, 2019` 