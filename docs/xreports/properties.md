# Properties

In examples below variable `cellsProviderBuilder` is a variable of type `IReportSchemaCellsProviderBuilder<TSourceEntity>`, where TSourceEntity is a type of model of the report. This variable is a result of adding column/row to report. For example:

```c#
IVerticalReportSchemaBuilder<string> builder = new VerticalReportSchemaBuilder<string>();
IReportSchemaCellsProviderBuilder<string> cellsProviderBuilder = reportBuilder.AddColumn("Value");
```

## AlignmentProperty

Marks cell to be center-, left- or right-aligned.

Example:
```c#
cellsProviderBuilder.AddProperties(new AlignmentProperty(Alignment.Right));
```

In Html it will be converted to `style="text-align: right"` attribute.

## BoldProperty

Marks cell as highlighted in bold font.

Example:
```c#
cellsProviderBuilder.AddProperties(new BoldProperty());
```

In Html it will be converted to `strong` tag, for example, `<strong>Name</strong>`.

## ColorProperty

Sets cell font and (optionally) background color.

Example:
```c#
// Blue font color.
cellsProviderBuilder.AddProperties(new ColorProperty(Color.Blue));

// Yellow font color on black background.
cellsProviderBuilder.AddProperties(new ColorProperty(Color.Yellow, Color.Black));
```

In Html it will be converted to `color` and `background-color` styles respectively.

## DateTimeFormatProperty and ExcelDateTimeFormatProperty

Sets format of date and time data contained in cell. Cells may contain either DateTimeOffset data type, in which case value is treated as DateTimeOffset, or other type, in which case value will be converted to DateTime. During export to Excel DateTimeOffset cannot be set in Excel as is, because Excel does not have timezone, so value in Excel will be value of its DateTime property.

The format is not parsed and not mutated in any way, it is simply passed to ToString method in Html handler and as number format in Excel handler.

Example:

```c#
// Will be formatted like "January 21, 2021"
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("MMMM d, yyyy"));

// Will be formatted like "Thursday, January 21, 2021" in Html and Excel.
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("dddd, MMMM d, yyyy"));

// If value of the cell is "12:34:56", depending on computer setting
// Html might contain "12:34:56 +00:00", but Excel will contain "12:34:56 K"
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss K"));
```

To stop interpreting character as format specifier prepend the character with backslash or enclose into double quotes.

```c#
// If value of the cell is "12:34:56", Html and Excel will contain "12:34:56 K"
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss \"K\""));
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("HH:mm:ss \\K"));

// For example, "Today is Thursday"
cellsProviderBuilder.AddProperties(new DateTimeFormatProperty("\"Today is \"dddd"));
```

Unfortunately, there is no conversion between Html and Excel formats, so if you need to use different format strings for Excel, you'll need to assign another property:

```c#
cellsProviderBuilder
    // "tt" works for DateTime.ToString, but not in Excel.
    // "AM/PM" is the Excel equivalent to "tt".
    .AddProperties(new ExcelDateTimeFormatProperty(
        "HH:mm:ss tt",
        "HH:mm:ss AM/PM"));
```

ExcelDateTimeFormatProperty is a subclass of DateTimeFormatProperty, so handlers for DateTimeFormatProperty will also handle ExcelDateTimeFormatProperty. The trick here is that there is handler for ExcelDateTimeFormatProperty which has lower priority (so, executed earlier) than Excel handler for DateTimeFormatProperty.

## DecimalPrecisionProperty

Sets precision of decimal data contained in cell. Property handler for Html uses culture from thread's CurrentCulture.

```c#
// 1.2 → "1.20"
// 1.2345 → "1.23"
cellsProviderBuilder.AddProperties(new DecimalPrecisionProperty(2));

// 1.2 → "1.2"
// 1.2345 → "1.23"
cellsProviderBuilder.AddProperties(new DecimalPrecisionProperty(2, false));
```

## MaxLengthProperty

Specifies maximum length of text in cell.

```c#
// "Lorem ipsum" → "Lorem ips…"
// "Test text." → "Test text."
cellsProviderBuilder.AddProperties(new MaxLengthProperty(10));

// "Lorem ipsum" → "Lorem i..."
// "Test text." → "Test text."
cellsProviderBuilder.AddProperties(new MaxLengthProperty(10, "..."));
```

The property limits number of 16-bit characters, so it won't work correctly for strings containing multi-character symbols.

## PercentFormatProperty

Sets precision and postfix text for percent value. Value will automatically be multiplied by 100, so value 1 is 100%. Property handler for Html uses culture from thread's CurrentCulture.

```c#
// 0.49555 → "49.56%"
// 0.5 → "50.00%"
cellsProviderBuilder.AddProperties(new PercentFormatProperty(2));

// 0.49555 → "50"
// 0.5 → "50"
cellsProviderBuilder.AddProperties(new PercentFormatProperty(0, string.Empty));

// 0.49555 → "49.6 (%)"
// 0.49 → "49.0 (%)"
cellsProviderBuilder.AddProperties(new PercentFormatProperty(1, " (%)"));

// 0.49555 → "49.56%"
// 0.5 → "50%"
cellsProviderBuilder.AddProperties(new PercentFormatProperty(2, preserveTrailingZeros: false));

// 0.49 → "49 (%)"
cellsProviderBuilder.AddProperties(new PercentFormatProperty(1, " (%)", false));
```

If format contains percent sign ("%"), then Excel will contain original value and will multiply the value by 100 during displaying. But if the format does not contain percent sign, Excel won't be able to display value correctly, so the value itself will is multiplied during export to Excel.

## SameColumnFormatProperty

Makes EpplusWriter format whole column the same as first cell in the column. Improves performance of export to Excel.
