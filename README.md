# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array (RDA) is a plain text data format for encoding structured data as text strings, similar to XML and JSON. 

While XML and JSON rely on using a schema that is fixed to a data model for the encoding, RDA uses a generic space (an expandable, multi-dimensional array) for encoding any data object - regardless of its attributes or structure[^1]. Because of this “one-size-fits-all” approach, RDA encoding is easier to use, faster to parse and encode, and more space-efficient, compared to XML and JSON.

But most significantly, as explained below, the schema-less RDA is potentially a key technology in facilitating effective communication and collaboration through loosely-coupled devices and programs, particularly when the data models in these devices and programs are uncertain or incompatible .

[^1]: RDA's encoding space is logically an infinitely expandable multi-dimensional array, where the number of dimensions and the size of each dimension of the multi-dimensional array of an RDA-encoded string can be expanded as required, and in RDA, a data object's attributes values are simply stored in the space as strings i.e. no specific data types. 

## The Problem With Cross-System Data Exchange 

Data exchange between two separated systems can be difficult to implement and maintain, as these systems can have incompatible data models due to their separate development cycles and different business requirements, and if they are from different vendors, it would be difficult to coordinate in making changes. Dedicated custom-developed pipelines are commonly required for connecting the communicating parties, using either an 'agreed' format (i.e. a schema) for the data exchange or having programmed logic in the pipelines to do the data conversion.

Also, technically speaking, using fixed data models the data-exchange pipelines makes the connected programs “tightly coupled” - meaning they are inflexible to changes because changing the data model in one program will affect all the other connected programs - there will be a very high cost to make data model change and also maintain data exchange compatibility.

<div align='center'>
<img src='img/Pre-Charian-data-transport.png' width='550' align='center'>
</div>

Therefore, using these dedicated pipelines for cross-system data exchange is expensive and inflexible. In an analogy, it is like sending parcels to people through ad hoc transport and delivery arrangements rather than using the Post Office. 

<div align='center'>
<img src='img/Pre-Post-office-system.png' width='470' align='center'>
</div>

Postal services are cost-effective for people posting goods because they use shared logistics and freight system helps cut down the cost. These servises are also convenient because they cater to most clients' requirements, such as posting parcels of different shapes and sizes.

<div align='center'>
<img src='img/Post-office-system.png' width='550' align='center'>
</div>

So for data exchange between isolated independent systems, we could do something similar to the Post Office's postal service, to cut down the costs and improve flexibility.  

## Universal Data Exchange

Universal Data Exchange, or UDX, is a "flattened" data communication layer for independent programs to exchange data conveniently and cost-effectively. By providing shared, generic data transport and delivery services, rather than individually building ad-hoc dedicated data-exchange solutions, UDX simplifies cross-program data communications and eliminates "tight coupling" between the communicating programs.

<div align='center'>
<img src='img/Charian-data-transport.png' width='550'>
</div>

In a Post Office, using standardized packaging is a key for it to effectively provide the postal services. More specifically, the use of **plain boxes** allows for packing loose items of different sizes and shapes so it's easier to cater to different parcel-posting requirements. Standardized packaging also simplifies parcel handling and allows modularized, more effective transportation using general courier companies. Similarly, the key in implementing UDX is to have a generic data container for packaging (and regulating) various data items, so arbitrary, 'irregular' data can be handled uniformly using general data transport protocols and methods. We need to find such "boxes", that is, to have a standardized, _universal_ data container, for UDX.

Popular data formats, such as XML, JSON, and CSV, are not suitable for encoding the UDX container, because these formats assume certain data models (by structure and type) to the data, meaning a container encoded in these formats is always for a certain kind of data, not _any data_. That's where RDA, a "one-size-fits-all" data format, comes into play. RDA encoding allows converting data objects with arbitrarily complex structures to a text string, and strings are supported by most computer systems and programming languages for manipulation and transportation. So using RDA-encoding, any data can be stored as text and be exchanged via text-based networks or messaging protocols, such as HTTP/RPC, TCP/IP, and FTP. 

## Schema-less Encoding

RDA use an expandable multi-dimensional array for encoding data items. The following example shows a single-dimension RDA-encoded string containing a list of data elements: "One", "Two", and "Three". 

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

An RDA-encoded string starts with a "header" section that contains the string's encoding chars, followed by a "payload" section containing the encoded data elements. In the second example above, the header section is the substring _"|,\\"_ and the payload section is the substring _"Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63"_. In the header section, the ending char of the substring, char '\\' in the example, defines the "escape char", and the other chars before the escape char are the "delimiter chars" - more specifically, delimiter chars for separating data elements at different dimensions in the array - in the example, char '|' is the 1st-dimension delimiter, and char ',' is the 2nd-dimension delimiter[^2], and so on.

[^2]: A more detailed explanation of RDA encoding rule is in [this repo's wiki](https://github.com/foldda/RDA/wiki).

As delimiters can be flexibly defined and added to the header section, the dimensions of an RDA's encoding space can be flexibly extended when required.

## RDA's Unique Features

> XML/JSON's schema-bound encoding space is like a wallet, with specific places for cards, notes, and coins, whilst RDA's space is like an infinitely expandable shelf that can store anything.

RDA sets itself apart from the other text data encoding formats with these unique encoding features - 

First, the storage locations in the space are addressed by integer indexes, rather than by names or string paths. This allows a client to access an RDA's content with no meta-data knowledge.

Second, the number of dimensions of the space, and the length of the array at each dimension, can be (practically unlimited) expanded as required. So from a client's encoding needs, the space's storge is infinite and always available.

Third, as a sub-dimension in an RDA space is also a multi-dimensional array, it is also an RDA storage itself, meaning **an RDA's encoding space is recursive** i.e. an RDA can be stored inside another RDA. This is why RDA can be used to store arbitrarily complex data objects.

## Charian - An Easy RDA Encoding and Parsing API

Charian is an RDA encoding and parsing API implemented in C#, Java, and Python. Charian is available as [a separate GitHub repo](https://github.com/foldda/charian).

Charian API intuitively uses the postal service metaphor while hiding the underlying encoding and parsing mechanics, that is, it uses an RDA-encoded string as a "container" object for storing data, and accessing the stored data items via integer-based indexing. For example, in C#, this is how a client app may send and receive data by firstly encoding the data as an RDA string into a file, then retrieving the data by reading and parsing the RDA string from the file.

```csharp
    using Charian;

    class RdaDemo1
    {
        public void Main(string[] args)
        {
            //a file is used as the physical media/channel for the data transport
            string PATH = "C:\\Temp\\file1.txt";

            //as sender ...
            SendSomeData(PATH);

            //as receiver ...
            ReceiveSomeData(PATH);
        }

        void SendSomeData(string filePath)
        {

            Rda rda1 = new Rda();    //create an Rda object which provides a storage space

            //placing some data items into the storage space (all as strings)
            rda1.SetValue(0, "One");  //storing a string value at index = 0
            rda1.SetValue(1, "Two");  //storing a decimal value at index = 1
            rda1.SetValue(2, "Three");  //storing a date value

            string encodedRdaString = rda1.ToString();     // => "|\|One|Two|Three"

            File.WriteAllText(filePath, encodedRdaString);  //output to a physical media
        }

        void ReceiveSomeData(string filePath)
        {
            string encodedRdaString = File.ReadAllText(filePath);  //input from a physical media

            Rda rda1 = Rda.Parse(encodedRdaString);    //decode the RDA string and restore an Rda "box" object

            //"unpacking" the data items from the box's content
            string a = rda1.GetValue(0);  //retrieve the stored value ("One") from location index = 0
            string b = rda1.GetValue(1);
            string c = rda1.GetValue(2);
        }
    }
```

Please take a look at [the Charian repo](https://github.com/foldda/charian) to see more detailed explanations and examples of using the API.

## Snappable - An RDA's Commercial Use Example

Snappable is an open-source component-based computing framework that allows assembling non-proprietary apps using mixed, portable components from different vendors. Snappable components are designed to be independent,  meaning they have minimal assumed knowledge when connecting and collaborating with each other, that includes when exchanging data, it cannot assume the data has a specific data model. In fact, RDA is created for this design requirement and is a primary data type used throughout the Enflow framework, eg. for it to interact with its hosted components, and for its components to exchange data between each other.

In Snappable, a compatible component is required to convert its "native data" to and from RDA, possibly by using Charian, so the data (carried within an RDA) can flow through the system. For example, the HL7FileReader component, available at the "Enflow Portable Components" repo, implements the conversion from HL7 to RDA, and the HL7FileWriter component does the opposite conversion, and these two components can be connected and used in an app that requires HL7 data file reading and writing.

[This demo video](https://www.youtube.com/watch?v=Uek9aW1qToU) shows how Snappable components can be assembled to form an app dynamically without programming or compilation.

Snappable's framework API and many of its ready-to-use portable components are available in [this GitHub repo](https://github.com/foldda/snappable).

## RDA's Other Potential Uses

> *RDA allows the implementation of a generic and unified data transport layer that applications can utilize for sending and receiving any data. As the applications are "loosely coupled" using such a data transport layer, they are less dependent and are easier to maintain if the data format is changed.*
 
Benefited from its "loose coupling" feature, RDA is most suitable for implementing systems integration and applications collabration, even for cross-language and cross-platform. For example, you can send a "Person" object as a serialized RDA container from your C# program to many receivers, and in one of the receivers, say it's a Python program, you can de-serialize the the received RDA container and construct a "User" object out of it, using properties of the original Person object. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately - for example, the Python's User object may only has a 'full-name' property but it can be derived from the Java Person object's 'first-name' and 'last-name' properties. 

Because RDA's recursive storage allows storing an RDA inside another RDA, multiple versions (or different formats) of the data can be transported "side-by-side" (as child RDAs) in a single RDA container. Allowing transporting mixed contents in a single RDA object can be useful for maintaining version compatibility, because a receiver can pick its preferred version or format to use. XML/JSON data can be stored inside an RDA container with no conversion required because, after all, XML/JSON documents are just strings.

Indeed, being able to send multiple copies of _any data_ side-by-side in a container can spark many inspiring ideas: like sending XML data together with its DTD, or sending a digital document paired with its digital signature or public key, or, in distributed computing, constructing a computing "workload" that contains some data and an executable script for a generic data-processing unit to execute.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than an XML or JSON container for encoding the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it has no reserved keyword or character meaning it allows any character to be part of the data content. In contrast, for example, in XML the line-feed character in data has to be encoded as "\&\#xA;", otherwise it will cause corruption.

## More Details 

The [wiki of this project](https://github.com/foldda/rda/wiki) contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - miscellaneous topics and dicsussions.

## Legal 

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 

"Recursive Delimited Array" and "RDA" are trademarks of [Foldda Pty Ltd](https://foldda.com).

