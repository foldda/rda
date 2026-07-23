# Recursive Delimited Array (RDA)

[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

> RDA is a text-encoding format that extends CSV with two core capabilities: **inline delimiter definitions** and **recursive nested structures**.

While traditional CSV assumes an externally agreed-upon delimiter (typically a comma) and a flat table, RDA declares its own delimiters directly in its inline header and allows any field to contain either a string value or a nested RDA structure.

## Compared to CSV

RDA retains CSV's lightweight, delimiter-separated design while overcoming two of its major limitations.

### Inline Delimiter Definition

Rather than relying on out-of-band agreements or file extensions to know how a file is formatted, an RDA-encoded string declares its encoding characters (delimiters plus an escape character) in its header, followed by a payload substring that contains the encoded data:

```text
|,\|Steel Bolts,500,12.4|Copper Wire,120,8.1
```

In this example, the leading header (`|,\`) explicitly defines:
1. `|` as the record delimiter
2. `,` as the field delimiter
3. `\` as the escape character

A parser needs only the RDA string itself to interpret the payload according to these explicit definitions.

### Recursive Nested Structures

Standard CSV is inherently two-dimensional. In RDA, any field can contain a fully nested RDA structure, enabling rich hierarchies within a uniform syntax.

For example, to attach a structured `Destination` (street address, suburb, postcode) to a specific record, a third delimiter (`;`) is declared for the next structural level down:

```text
|,;\|Steel Bolts,500,12.4|Copper Wire,120,8.1|Packing Foam,40,3.6,45 Dock Rd;Fremantle;6160
```

Here, the header specifies three positional delimiters—`|` for records, `,` for top-level fields, and `;` for second-level nested fields—followed by the escape character `\`.

Notice that only the final record contains a nested `Destination` value (`45 Dock Rd;Fremantle;6160`); earlier records remain simple and unburdened by empty fields or extra syntax. CSV cannot express this kind of optional depth without resorting to application-specific hacks or string escaping.

## Compared to JSON and XML

Because RDA natively models nested hierarchies, it serves as a lightweight alternative to JSON and XML in data interchange scenarios.

### Compact Payload Size

RDA uses positional delimiters rather than repeating key names (JSON) or opening/closing tags (XML) on every record. As a result, RDA payloads are significantly smaller—especially for uniform or semi-uniform arrays of data.

See the [size comparison demo](https://htmlpreview.github.io/?https://github.com/foldda/rda/blob/main/rda-vs-json-size.html)) for examples.

### Separation of Schema and Data

JSON and XML embed structural metadata directly into every single object. RDA allows you to separate schema definitions from the data payload when appropriate (or include an optional header row, similar to CSV). While this trade-off reduces self-describing verbosity, it mostly eliminates redundancy over the wire.

See the [dynamic schema-version handling demo](https://htmlpreview.github.io/?https://github.com/foldda/rda/blob/main/rda-version-handling.html) for examples.

## Purpose & Next Steps

RDA isn't built to replace CSV, XML, or JSON as data containers. 

>RDA is designed for **loosely coupled, late-binding data exchange**—scenarios where data structures evolve over time across disparate systems, and where hardcoding schemas into compiled code or embedding heavy metadata in every payload is inefficient.

While RDA defines the foundational wire format, higher-level capabilities—such as schema evolution, late binding, dynamic object mapping, and composable components—are handled by the surrounding ecosystem.

* Check out **[Charian](https://github.com/foldda/)** for a lightweight reference parser/encoder implementation featuring working code and practical examples to experiment with RDA's benefits in your own projects.
* Explore **[Snappable](https://github.com/foldda/)** to see how modular, late-bound components dynamically work and collaborate inside a generic **component-based computing framework**.

---

RDA is released under the MIT License.

"Recursive Delimited Array" and "RDA" are trademarks of Foldda Pty Ltd.
