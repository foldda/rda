# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is an encoding format for storing and transporting structured data in a text string.

Unlike XML and JSON using a schema to restrict the data to the specifics of a certain application, RDA is a **schema-less** format for **generic** data. It means an RDA-encoded string (aka. "RDA container") can be used for storing any data from any application. 

_With the simple yet powerful data transportation enabled by using RDA, programs "talking" to each other has never been so easy._

## A Problem With XML/JSON

> *An RDA container is like a box that has lots of pockets for storing "anything", and an XML or JSON container is like a wallet that offers specific places for coins, notes, and cards.* 

When use XML/JSON format for data exchange between two applications, a developer must firstly design a schema that decides the data types and data structure specifically for the data exchange. If one of applications wants to change its data format and the schema, the change of the schema must be propagated across, and be agreed by, all the connected applications. Changing the schema makes it difficult to manage compatibility, but on the other hand, not being able to change the schema restricts what can be enhanced to the applications.

In comparison, while RDA can also be used for exchanging complex structured data, it is schema-less and is designed to be **application independent**[^1]:

[^1]: Full details of the encoding rules can be found [here](https://foldda.github.io/rda/rda-encoding-rule).

* Instead of using tags or markups, RDA uses delimited encoding for separating and structuring data elements; 
* Instead of using named paths, RDA uses integer-based indexes for addressing data elements inside a container; 
* Instead of having multiple, specific data types, RDA has only two generalized data types[^2]: _RDA_ and _string_, for "composite" and "primitive" data values repectively.
 
[^2]:RDA data types and data structure are [discussed here](https://foldda.github.io/rda/data-type-and-data-structure). 

By using RDA, applications are no longer restricted by a pre-defined data format before they can exchange data. It means the data format can be dynamic and can be changed if required, giving the applications the flexibility to handle data changes while maintaining the connection and the communication.

## Benefits of RDA

> *RDA allows implementing a generic and unified data transport layer which applications can utilize for sending and receiving data. As the applications are "loosely coupled" using such a data transport layer, they are less dependent and easier to maintain if the data format is changed.*
 
One powerful feature of RDA is for implementing cross-language and cross-application object-serialization. For example, you can send a "Person" object as a serialized RDA container from your C# program to many receivers, and in a Python program, you can de-serialize a "User" object using data elements from the received RDA container. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately. 

Another feature of RDA is for maintaining version compatibility between a sender and a receiver. Because RDA's recursive storage allows storing an RDA inside another RDA, multiple versions (or different formats) of the data can be transported "side-by-side" (as child RDAs) in an RDA container, and the receiver can pick its preferred version or format to use. 

Indeed, being able to send multiple copies of _any data_ side-by-side in a container can be interestingly useful: like sending XML data together with its DTD[^3], or sending a digital document paired with its digital signature or public key, or sending a computing "workload" that has some data together with an executable script to a data-processing unit, etc.

[^3]: An XML or JSON document can be converted to a single 'string' data element, and be stored inside an RDA container.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than a XML or JSON container with the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it does not have any reserved keyword or character and allows any charactor to be part of the data content. In contrast, for example, in XML the line-feed character in data has to be encoded as "\&\#xA;", otherwise it will cause corruption.

## About This Project

This project implements an API for cross-application data communication via object-serialization using RDA as the data format. In the API design, RDA encoding and parsing are wrapped in a single class called _Rda_. Class Rda is intuitively modeled as a "container" which provides the following methods - 

* **Setter-Getter()** methods which are for storing and retrieving the container's content using index-based addresses. 
* **ToString()** method which is for serializing the container and its content, i.e. apply RDA-encoding and make it into a string. 
* **Parse()** method which is for de-serializing an RDA-encoded string back to an Rda container object with content.

The idea of generic object-serialization in such design is, rather than serializing a data object, the data object's properties are stored in an Rda container object first and then the Rda object (with its content) is serialized instead. 

## Getting Started

The super-lightweight API has no 3rd party dependency and requires no installation. To use the API, you just need to inlcude the provided source code (available in [C#](https://github.com/foldda/rda/tree/main/src/CSharp), [Java](https://github.com/foldda/rda/blob/main/src/Java/), and [Python](https://github.com/foldda/rda/blob/main/src/Python)) in your project. 

The code snippet below demonstrates how to serialize and de-serialize data values, using the API[^4] - 

[^4]: The example is given in C#. Methods of using the Java API and the Python API are very similar.

```c#
//a sender ... create an RDA container
Rda rdaSending = new Rda();    

//use SetValue() store some data values in the container
rdaSending.SetValue(0, "One");  //store value "One" at index = 0
rdaSending.SetValue(1, "Two");
rdaSending.SetValue(2, "Three");

//use ToString() to serialize the container and its content to an RDA-encoded string
System.Console.WriteLine(rdaSending.ToString());   //print the encoded container string, eg "|\|One|Two|Three"

// ... the encoded RDA string can be saved to a file, or be sent to another app via network ...
// ... and a receiver can ...

//use Parse() to de-serialize an RDA-format encoded string back to an Rda container object 
Rda rdaReceived = Rda.Parse(@"|\|One|Two|Three");   

//use GetValue() to retrieve data values from in an RDA container at an index location    
System.Console.WriteLine(rdaReceived.GetValue(2));   //print "Three", the value stored at index=2 in the container.

```

## More Details 

The [wiki of this project](https://github.com/foldda/rda/wiki) contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - miscellaneous topics and dicsussions.

The unit tests (in [C#](https://github.com/foldda/rda/tree/main/src/CSharp/UnitTests), [Java](https://github.com/foldda/rda/blob/main/src/Java/src/test/java/UniversalDataTransport/UniversalDataFrameworkTests.java), and [Python](https://github.com/foldda/rda/blob/main/src/Python/test_rda.py)) are good places to find further examples of how to use the RDA API.

## Contributing

You can help this project by - 

- Giving us feedback on how you use the API in your project.
- Improving existing code, test cases, and documentation.
- Writing a new RDA encoder/parser (e.g. in a new language) for your project, and share!
- Participating in discussions and giving your insight.

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Authors

* **Michael Chen** - Designing RDA, and the reference parser (in C#) - [sierrathedog](https://github.com/sierrathedog)

* **Samuel Chen** - The Java parser and the Python parser - [samuelfchen](https://github.com/samuelfchen)

You can be a contributor and help this project! Please contact us.

## License 

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 

"Recursive Delimited Array" and "RDA" are trademarks of [Foldda Pty Ltd](https://foldda.com) - an Australia software company.

