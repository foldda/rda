# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is a text encoding format, similar to XML and JSON, for storing data in text strings. Unlike XML and JSON, RDA does not use a fixed data model (i.e. a schema) for the encoding, rather, it uses a generic space, an expandable multi-dimensional array[^1], for accommodating a targeted data object, regardless of the object's attributes and structure.

[^1]: RDA's encoding space is logically an infinitely expandable multi-dimensional array, where the number of dimensions and the size of each dimension of the multi-dimensional array of an RDA-encoded string can be expanded as required, and in RDA, a data object's attributes values are simply stored in the space as strings i.e. no specific data types. 

RDA's simpler, "one-size-fits-all" approach brings many benefits including being easier to implement, faster parsing and encoding, and more space-efficient. Most significantly, as explained below, RDA is a key technology that enables applications to exchange data _freely_, even when the applications are independent and evolving, with 'incompatible' data models. This potentially allows more communication and collaborative interactions between many otherwise isolated devices and programs.

## RDA's Schema-less Encoding

The following example shows a single-dimension RDA-encoded string containing a list of data elements: "One", "Two", and "Three". 

```
|\|One|Two|Three
```

| One | Two | Three | 
|------|-----|-----|

The next example is a 2-dimensional RDA container that contains the data equivalent to the content of the following table.
```
|,\|Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63
```

| Name | Sex | Age | 
|------|-----|-----|
| Mary | F   | 52  | 
| John | M   | 70  |
| Kate | F   | 63  | 

An RDA-encoded string starts with a "header" section that contains the string's encoding chars, followed by a "payload" section containing the encoded data elements. In the second example above, the header section is the substring _"|,\\"_ and the payload section is the substring _"Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63"_. In the header section, the ending char of the substring, char '\\', defines the "escape char", and the other chars before the escape char are the "delimiter chars" - more specifically, delimiter chars for separating data elements at different dimensions in the array - in the example, char '|' is the 1st-dimension delimiter, and char ',' is the 2nd-dimension delimiter[^2].

[^2]: A more detailed explanation of RDA encoding rule is in this repo's wiki.

RDA encoding allows defining delimiters dynamically in the header section, so the encoding space's dimensions can be flexibly extended when required.

## The Problem To Address

Reliable cross-program data exchange, such as between two systems from different vendors, or an IoT device and its control console, are often more difficult to implement and maintain, as these programs may have incompatible data models due to their separate development cycles and evolving business requirements. Normally it requires building custom, dedicated pipelines to connect the communicating parties, using either an 'agreed' format (i.e. a schema) for the data exchange or having programmed logic in the pipelines to do the data conversion.

<div align='center'>
<img src='img/Pre-Charian-data-transport.png' width='550' align='center'>
</div>

Building these dedicated pipelines is resource-consuming and inefficient because, in an analogy, it is like sending parcels to people through ad hoc transport and delivery arrangements instead of using the generic postal services from the Post Office. Also, technical speaking, programs depend on schema-based data exchange pipelines are “tightly coupled” by the fixed data models used in building the pipelines, making them inflexible to changes. If one of the programs has evolved and the data model needs to be changed, the logic connecting the programs need to be updated to maintain compatibility, and the situation can be more complex if multiple parties need to be kept compatible with the changed data model as it would require more development and testing.

<div align='center'>
<img src='img/Pre-Post-office-system.png' width='470' align='center'>
</div>

Postal services are convenient and cost-effective for posting goods to people, as they can easily cater to all kinds of requirements, such as parcels of different shapes and sizes, and use of the shared logistics and freight system helps cut down the cost.

<div align='center'>
<img src='img/Post-office-system.png' width='550' align='center'>
</div>

## Towards Universal Data Exchange

Using the same “post-office-like” approach, Universal Data Exchange, or UDX, is an envisioned data communication method for all programs to use for exchanging data conveniently and cost-effective - that is, by sharing a common, generic data transport and delivery services, rather than individually building ad-hoc dedicated data-exchange solutions. 

> With UDX, data transport implementation can be simplified and shared, and there is no "tight coupling" between the communicating programs.

<div align='center'>
<img src='img/Charian-data-transport.png' width='550'>
</div>

Using the Post Office analogy, one of the keys for the Post Office to cater for different parcel-posting requirements from all its clients is to use standardized packaging. Packing loose items into **boxes** simplifies parcel handling and allows modularized, more effective transportation by general courier companies. Similarly, a key in UDX's design is to use a generic data container for packaging (and regulating) various data items (e.g. properties of a data object), so arbitrary, 'irregular' data can be handled uniformly using general data transport protocols and methods. For implementing UDX, we need to find such "boxes", that is, to have a standardized, _universal_ data container.

Popular data formats, such as XML, JSON, and CSV, are not suitable for encoding the UDX container. That’s because each data instance in one of these formats assumes a certain data model (by structure and type), meaning a container encoded in these formats won't be the “generic and universal” that we want for accommodating _any data_. That's where RDA, a "one-size-fits-all" data format, comes to play. RDA encoding allows converting data objects with arbitarily complex structure to a text string - a data type supported by most computer systems and programming languages for manipulation and transportation. Using RDA, any data can be stored as text and be exchanged via text-based network or messaging protocols, such as HTTP/RPC, TCP/IP, and FTP. 

## Three Characteristics Of RDA's Encoding Space

> XML/JSON's schema-bound encoding space is like a wallet, with specific places for cards, notes, and coins, whilst RDA's space is like an infinitely expandable shelf that can store anything.

RDA's multi-dimensional schemaless encoding space has the following three importantant characteristics. 

First, the storage locations in the space are addressed by integer indexes, rather than by names or string paths. This allows a client to access RDA's content easily without any prerequisite.

Second, the number of dimensionas of the space, and the length of the array at each dimension, can be (theoritically) infinitely expanded as required. So from a client's perspective, any required storage in the space is always available.

Third, as a sub-dimension can have unlimited dimensions because it can be infinitely expanded, the sub-dimension is a complete RDA storage itself, this means, **an RDA's encoding space is recursive** - an RDA can store other RDAs as its children. This feature can be used for maintaining compability between multiple versions of data, i.e. you can have have mutliple children RDAs stored inside a parent RDA, each child RDA carries a different version of the data, and when the parent RDA is transferred to a recipient client, it can choose a compatible verion to consume. 

## Charian - Programming RDA

RDA stands for "Recursive Delimited Array". It is a delimited encoding format similar to CSV where encoded data elements are separated by delimiter chars except, among other things, RDA allows dynamically defining multiple delimiters for encoding more complex, multidimensional data[^4].

[^4]: According to the RDA encoding rule, the header section starts from the first letter of the string and finishes at the first repeat of the string's starting letter which is called the ‘end-of-section’ marker. Any char can be used as a delimiter or the escape char, the only requirement is they must be different to each other in an RDA string header. By placing the encoding chars in the header at the front of an RDA string allows a parser to be automatically configured when it starts parsing the string. 

Here is an example of an RDA format string containing a 2D (3x3) data table, using two delimiter chars for separating the data elements -

```
|,\|A,B,C|a,b,c|1,2,3
```
The beginning of an RDA string is a substring section known as the "header" which contains the definition of the RDA string’s encoding chars including one or many delimiter chars (“delimiters”) and one escape char. In this example, the header is the substring "|,\\|", and the delimiters are the first two chars '|' and ','. The third char ‘\\’ is the ‘escape’ char, and the last char ‘|’ is the ‘end-of-section’ marker which marks the end of the header section. 

Following the header, the remaining RDA string is the 'payload' section that contains the encoded data. The RDA payload section provides a 'virtual' storage space of a multi-dimensional array where stored data elements are delimited using the delimiters defined in the header, and each data element is accessible via an index address comprised of an array of 0-based integers. In the above example, the top dimension of the array is ored delimited by delimiter '|' and the second dimension is delimited by delimiter ',', and the data element stored in this 2D array at the indexed location [0,1] is the string value "B".

**Compared to XML and JSON**

> The space from XML/JSON is like a wallet, where places are specifically defined for holding cards, notes, and coins; the space from RDA is like an enormous shelf, where you can place anything anywhere in the unlimited space provided.

RDA is specifically designed to avoid targeting a certain data model and having to define and maintain a schema. Such design is reflected by the structure of RDA's encoded storage space, the way of addressing a location in the space, and the supported data types[^5].  

[^5]: First, RDA has multi-dimensional array storage space that is dynamically expandable, that is, the size of each dimension and the number of dimensions can be increased or decreased as required, like an elastic bag. This is in contrast to the ‘fixed’ hierarchical space provided by schema-based encodings, like XML or JSON, which is restricted by a predefined data mode, like a rigid, fixed-shaped box. Second, RDA uses integer-based indexes for addressing the storage locations in its multi-dimensional array storage space, which means, and combination of non-negative integers is a valid address referring to a valid storage location in the space. This is in contrast to XML and JSON, the address for accessing a storage location is a ‘path’ that has to be ‘validated’ against a pre-defined schema. Third, RDA assumes all data (of any type) can be 'expressed as a string' and a value stored at a location (referred to as “a data's value expression”) can only be a string; whilst XML and JSON attempt to define and include every possible data types and a data values stored at a location must conform with what has been defined in the schema.

Inheritively from RDA's schemaless design, the encoding is simpler, more space-efficient, and configuration-free compared to XML and JSON. But perhaps the most interesting and unique property of RDA is the **recursiveness** of the storage space: the multi-dimensional array structure is homogenous, and there can be only one 'unified' data type, so a sub-dimension in the space is itself a multi-dimensional space that has the same structure as its containing (parent dimension) space, and can be used in the same way. The recursiveness of the multi-dimensional space allows an arbitrarily complex data structure to be (recursively) decomposed into sub-components and stored in the dimensions and their sub-dimensions from the provided space.

## Charian - RDA's Codec API



> *An RDA container is like a large, expandable pocket with many inner pockets, recursively, which can be used for storing "anything"; an XML or JSON container is like a wallet with specific places for coins, notes, and cards.* 

When using XML/JSON format for data exchange between two applications, a developer must first design a schema that decides the data types and data structure specifically for the data exchange. If one of the applications wants to change its data format and schema, the change must be propagated across, and be agreed by all the connected applications. Changing the schema makes it difficult to manage compatibility, but on the other hand, not being able to change the schema restricts what can be enhanced to the applications.

In comparison, while RDA can also be used for exchanging complex structured data, it is schema-less and is designed to be **application independent**[^1]:

[^1]: Full details of the encoding rules can be found [here](https://github.com/foldda/RDA/wiki/1.-Overview#the-rda-encoding-rule).

* Instead of using tags or markups, RDA uses delimited encoding for separating and structuring data elements; 
* Instead of using named paths, RDA uses integer-based indexes for addressing data elements inside a container; 
* Instead of having multiple, specific data types, RDA has only two generalized data types[^2]: _RDA_ and _string_, for "composite" and "primitive" data values respectively.
 
[^2]: RDA data types and data structure are [discussed here](https://foldda.github.io/rda/data-type-and-data-structure). 

By using RDA, applications are no longer restricted by a pre-defined data format before they can exchange data. It means the data format can be dynamic and can be changed if required, giving the applications the flexibility to handle data changes while maintaining the connection and communication.

## Benefits of RDA

> *RDA allows implementing a generic and unified data transport layer which applications can utilize for sending and receiving data. As the applications are "loosely coupled" using such a data transport layer, they are less dependent and easier to maintain if the data format is changed.*
 
One powerful feature of RDA is for implementing cross-language and cross-application object-serialization. For example, you can send a "Person" object as a serialized RDA container from your C# program to many receivers, and in a Python program, you can de-serialize a "User" object using data elements from the received RDA container. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately. 

Another feature of RDA is for maintaining version compatibility between a sender and a receiver. Because RDA's recursive storage allows storing an RDA inside another RDA, multiple versions (or different formats) of the data can be transported "side-by-side" (as child RDAs) in an RDA container, and the receiver can pick its preferred version or format to use. 

Indeed, being able to send multiple copies of _any data_ side-by-side in a container can be interestingly useful: like sending XML data together with its DTD[^3], or sending a digital document paired with its digital signature or public key, or sending a computing "workload" that has some data together with an executable script to a data-processing unit, etc.

[^3]: An XML or JSON document can be converted to a single 'string' data element, and be stored inside an RDA container.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than a XML or JSON container with the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it does not have any reserved keyword or character and allows any charactor to be part of the data content. In contrast, for example, in XML the line-feed character in data has to be encoded as "\&\#xA;", otherwise it will cause corruption.

## About This Project

This project contains the (forthcoming, under development) technical documentation of the RDA encoding rules. 


## More Details 

The [wiki of this project](https://github.com/foldda/rda/wiki) contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - miscellaneous topics and dicsussions.

## Contributors

* **Michael Chen** - Invented RDA, and developed the reference RDA parser (in C#) - [sierrathedog](https://github.com/sierrathedog)

* **Samuel Chen** - The Java parser and the Python parser - [samuelfchen](https://github.com/samuelfchen)

You can be a contributor and help this project! Please contact us.

## Legal 

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 

"Recursive Delimited Array" and "RDA" are trademarks of [Foldda Pty Ltd](https://foldda.com).

