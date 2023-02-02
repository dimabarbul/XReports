# Properties

## AlignmentProperty

Marks cell to be center-, left- or right-aligned.

Example:
```c#
reportBuilder.AddProperties(new AlignmentProperty(Alignment.Right));
```

In Html it will be converted to `style="text-align: right"` attribute.

## BoldProperty

Marks cell as highlighted in bold font.

Example:
```c#
reportBuilder.AddProperties(new BoldProperty());
```

In Html it will be converted to `strong` tag, for example, `<strong>Name</strong>`.

## ColorProperty

Sets cell font and (optionally) background color.

Example:
```c#
// Blue font color.
reportBuilder.AddProperties(new ColorProperty(Color.Blue));

// Yellow font color on black background.
reportBuilder.AddProperties(new ColorProperty(Color.Yellow, Color.Black));
```

In Html it will be converted to `color` and `background-color` styles respectively.

## DateTimeFormatProperty

Sets format of date and time data contained in cell. Supported format specifiers:

Format Specifier | Meaning
-----------------|--------
dd               |  day with leading zero (00 – 31)
d                |  day (1 – 31)
MM               |  month with leading zero (01 – 12)
M                |  month (1 – 12)
MMMM             |  month name (January, February etc.)
yyyy             |  full year (4 digits)
yy               |  short year (2 digits)
HH               |  hour in 24-hour format with leading zero (00 – 23)
H                |  hour in 24-hour format (0 – 23)
hh               |  hour in 12-hour format with leading zero (01 – 12)
h                |  hour in 12-hour format (1 – 12)
mm               |  minute with leading zero (00 – 59)
m                |  minute (0 – 59)
ss               |  second with leading zero (00 – 59)
s                |  second (0 – 59)
tt               |  am/pm

Only format specifiers mentioned above are guaranteed to be correctly formatted during converting to Html and Excel.

All other text will be passed as format to DateTime.ToString or as Excel cell format as is, so if it has special meaning, it will be applied. For example, if you pass "MMM", it will be interpreted as short month name, e.g., Jan, Feb.

Example:
```c#
// Will be formatted like "January 21, 2021"
reportBuilder.AddProperties(new DateTimeFormatProperty("MMMM d, yyyy"));

// Will be formatted like "Thursday, January 21, 2021" in Html and Excel.
// Though dddd is not in the list above, it will work as it has the same meaning
// in c# and in Excel.
reportBuilder.AddProperties(new DateTimeFormatProperty("dddd, MMMM d, yyyy"));

// If value of the cell is "12:34:56", depending on computer setting
// Html might contain "12:34:56 +00:00", but Excel will contain "12:34:56 K"
reportBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss K"));
```

To stop interpreting character as format specifier prepend the character with backslash or enclose into double quotes.

```c#
// If value of the cell is "12:34:56", Html and Excel will contain "12:34:56 K"
reportBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss \"K\""));
reportBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss \\K"));

// For example, "Today is Thursday"
reportBuilder.AddProperties(new DateTimeFormatProperty("\"Today is \"dddd"));
```

## DecimalPrecisionProperty

Sets precision of decimal data contained in cell.

```c#
// Will be formatted with 2 decimal places.
reportBuilder.AddProperties(new DecimalPrecisionProperty(2));
```

## MaxLengthProperty

Specifies maximum length of text in cell. Built-in property handlers trim text length to N-1 character and append ellipsis.

```c#
// Example: "Lorem ips…"
reportBuilder.AddProperties(new MaxLengthProperty(10));
```

## PercentFormatProperty

Sets precision and postfix text for percent value. Value will automatically be multiplied by 100, so value 1 is 100%.

```c#
// 0.49555 → "49.56%"
reportBuilder.AddProperties(new PercentFormatProperty(2));

// 0.49555 → "50"
reportBuilder.AddProperties(new PercentFormatProperty(0, string.Empty));

// 0.49555 → "49.6 (%)"
reportBuilder.AddProperties(new PercentFormatProperty(1, " (%)"));
```

## SameColumnFormatProperty

Makes EpplusWriter format whole column the same as first cell in the column. Improves performance of export to Excel.
