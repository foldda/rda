# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is a text encoding format for storing structured data in a string, similar to XML and JSON. 

But unlike XML and JSON, which require pre-defining a schema that has places and defined attributes for every data elements for a targeted data structure, RDA's "storage space" is a plain "one-size-fits-all" multi-dimensional array[^1], and all data elements of a data object are placed as a simplified data types in the array.

[^1]: The number of dimensions and the size of each dimemsion of the multi-dimensional array of an RDA encoded string can be expanded as rquired, 

As the result, RDA encoding and decoding are much simpler (read "fast", "efficient", "compact"...), and as explained below, more flexible and resiliant to data structure changes during exchanging data between programs.

## Schema-less Encoding

The following example shows a 1-dimension RDA-encoded string that contains a list of data elements: "One", "Two", and "Three". 

```
|\|One|Two|Three
```
The next example is a 2-dimension RDA container that contains the data equivalent to the content of the following table.
```
|,\|Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63
```

| Name | Sex | Age | 
|------|-----|-----|
| Mary | F   | 52  | 
| John | M   | 70  |
| Kate | F   | 63  | 

_As RDA **schema-less** encoding is not fixed to a defined data structure, it can be flexibily adapted depending on the application, and is most suitable for exchanging data between distributed, independent programs, as it allows a program to flexibly adapt to uncontrolled data structure changes (eg. caused by the other party), and remain compatible in the data communication._

## A Problem Schema-based Data Communication With XML/JSON

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

