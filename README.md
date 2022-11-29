# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is an encoding format for storing and transporting structured data in a text string.

Unlike XML and JSON using a schema to restrict the data to the types and structure of a specific application, RDA is a **schema-less** format for **generic** data. An RDA-encoded string (aka. "RDA container") can be used for storing any data from any application. 

## A Problem With XML/JSON

> *An RDA container is like an enormous, expandable shelf that provides unlimited space for storing "anything", whilst an XML or JSON container is like a wallet that offers specific places for coins, notes, and cards.* 

When use XML/JSON format for data exchange, a developer must firstly decide the data types and data structure for all connected applications, and carve the decided data format in a schema. If an application changes its data format and the schema, all other applications will need to change their container-parsing and data-handling logic to be compatible. For example, if Twitter or Google changes the data format in their REST API, a lot of downstream applications will be affected.

In contrast, while RDA is also a text-encoded data format[^1] for storing complex structured data, it is designed to be **generic** and **application independent** -

[^1]: Full details of the encoding rules can be found [here](https://foldda.github.io/rda/rda-encoding-rule).

* Instead of using tags or markups, RDA uses delimited encoding for separating and structuring data elements; 
* Instead of using named paths, RDA uses integer-based indexes for addressing data elements inside a container; 
* Instead of having multiple, specific data types, RDA has only two generalized data types[^2]: _RDA_ and _string_, for "composite" and "primitive" data values repectively.
 
[^2]:RDA data types and data structure are [discussed here](https://foldda.github.io/rda/data-type-and-data-structure). 

This means an RDA container can accomandate any data format changes, so applications can freely connect to each other to exchange data. It means the content and the structure can be decided later and can evolve over time, and be flexibly handled by the application.

## Benefits of RDA

> *RDA allows implementing a generic and unified data transport layer which applications can utilize for sending and receiving data. As the applications are "loosely coupled" because of this unified data transport layer, they can be independently and flexibly maintained if the data format is changed.*
 
One powerful feature of RDA is for implementing cross-language and cross-application object-serialization. For example, you can send [a "Person" object as a serialized RDA container](https://foldda.github.io/rda/2022/10/03/obj-serialization-pattern.html) from your C# program to many receivers, and in a Python program, you can de-serialize a "User" object using data elements from the received RDA container. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately. 

Another feature of RDA is for maintaining version compatibility between a sender and a receiver. Because RDA's recursive storage allows storing an RDA inside another RDA, you can transfer copies of multiple versions or formats of your data "side-by-side" (as child RDAs) in an RDA container, and the receiver can pick the right version or format to its preference. 

Indeed, being able to send multiple pieces of "anything" side-by-side in a container can have many interesting uses: like sending XML data together with its DTD[^3], or sending an encrypted document together with the associated digital signature and public key, or sending a computing "workload" that has some data together with an executable script to a data-processing unit, etc.

[^3]: An XML or JSON document can be converted to a single 'string' data element, and be stored inside an RDA container.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than a XML or JSON container with the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it does not have any reserved keyword or character. For example, it allows the data to contain native line-breaks as part of the value, whilst in XML/JSON line-breaks must be replaced with a reserved string (eg "\&\#xA;") or they will be ignored by the parser.

## Getting Started

This project provides the RDA-encoding API in three languages: [C#](https://github.com/foldda/rda/tree/main/src/CSharp), [Java](https://github.com/foldda/rda/blob/main/src/Java/), and [Python](https://github.com/foldda/rda/blob/main/src/Python). The API has no 3rd party dependency and requires no installation. You just need to include the API's super-lightweight source code, which contains only one class and one interface, in your project, and start using them as part of your program, as below. 

#### _Using class Rda_

The _Rda class_ implements the RDA encoding and decoding and is modeled as a "container" object. The idea is to store your data object and its properties in the container, and then serialize the container and its conent into a string. The Rda class provides - 

* **Setter-Getter** methods are for accessing the container's content using index-based addresses. 
* **ToString** method is for serialization, i.e. apply RDA-encoding to the container's content and making it into a string. 
* **Parse** method is for de-serializing an RDA-encoded string back to an RDA container object with content.

The following code[^4] shows an example of serializing and de-serializing data values using these methods.

[^4]: Methods of using the Java API and the Python API are very similar.

```c#

//create an RDA container
Rda rdaSending = new Rda();    

//SetValue(): store data values into the container
rdaSending.SetValue(0, "One");  //store value "One" at index = 0
rdaSending.SetValue(1, "Two");
rdaSending.SetValue(2, "Three");

//ToString(): serialize the container and its content to an RDA-encoded string
System.Console.WriteLine(rdaSending.ToString());   //print the encoded container string, eg "|\|One|Two|Three"

// ... the encoded RDA string can be saved to a file, or be sent across the network ...

//Parse(): de-serialize an RDA-format encoded string back to an RDA container object 
Rda rdaReceived = Rda.Parse(@"|\|One|Two|Three");   

//GetValue(): retrieve a value from in an RDA container at an index location    
System.Console.WriteLine(rdaReceived.GetValue(2));   //print "Three", the value stored at index=2 in the container.

```

#### _Using interface IRda_

The _IRda interface_ defines two methods for applications to implement arbitrarily complex object-serialization using RDA:

* **ToRda()**: produces an RDA container that contains specific properties of the object, for serialization. 

* **FromRda(rda)**: restores the object's specific properties from values in a given RDA container, for de-serialization.

An example of complex object-serialization using RDA container [can be found here.](https://foldda.github.io/rda/object-serialization-pattern).

#### _Test Cases_

The unit tests [[C#](https://github.com/foldda/rda/tree/main/src/CSharp/UnitTests), [Java](https://github.com/foldda/rda/blob/main/src/Java/src/test/java/UniversalDataTransport/UniversalDataFrameworkTests.java), [Python](https://github.com/foldda/rda/blob/main/src/Python/test_rda.py)] are good places to find further examples of how to use the RDA API.

## More Details 

The wiki of this project contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - misc. topic dicsussions.

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

