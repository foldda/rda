# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is a text encoding format, similar to XML and JSON, for storing data in text strings. 

Unlike XML and JSON, RDA does not use a fixed data model (i.e. a schema) for the encoding. In RDA's encoding, it uses a generic space, which is an expandable multi-dimensional array[^1], for any data object - regardless of the object's attributes and structure.

[^1]: RDA's encoding space is logically an infinitely expandable multi-dimensional array, where the number of dimensions and the size of each dimension of the multi-dimensional array of an RDA-encoded string can be expanded as required, and in RDA, a data object's attributes values are simply stored in the space as strings i.e. no specific data types. 

Compared to XML and JSON, RDA is easier to implement, faster to parse and encode, and more space-efficient - thanks to its simpler, "one-size-fits-all" approach. Most significantly, as explained below, RDA empowers more communication and collaborative interactions between many otherwise isolated devices and programs, because it allows applications to exchange data easily and more flexibly, even when they have independently evolving data models. 

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

## The Problem: Schema-Dependent Pipelines

Reliable cross-program data exchange, such as between two systems from different vendors, or an IoT device and its control console, are often difficult to implement and maintain, as these programs normally have incompatible data models due to their separate development cycles and evolving business requirements. Normally it requires building custom, dedicated pipelines to connect the communicating parties, using either an 'agreed' format (i.e. a schema) for the data exchange or having programmed logic in the pipelines to do the data conversion.

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

## Universal Data Exchange

Universal Data Exchange, or UDX, is a "flattened" data communication layer for independent programs to exchange data conveniently and cost-effectively. By providing shared, generic data transport and delivery services, rather than individually building ad-hoc dedicated data-exchange solutions, it's like the postal service for data exchange. With UDX, cross-program data communication is much simplified, and there is less or no "tight coupling" between the communicating programs.

<div align='center'>
<img src='img/Charian-data-transport.png' width='550'>
</div>

Continue using the Post Office analogy, using standardized packaging is the key for it to cater to different parcel-posting requirements. Packing loose items into **plain boxes** simplifies parcel handling and allows modularized, more effective transportation by general courier companies. Similarly, a key in UDX's design is to use a generic data container for packaging (and regulating) various data items (e.g. properties of a data object), so arbitrary, 'irregular' data can be handled uniformly using general data transport protocols and methods. For implementing UDX, we need to find such "boxes", that is, to have a standardized, _universal_ data container.

Popular data formats, such as XML, JSON, and CSV, are not suitable for encoding the UDX container, because these formats assume certain data models (by structure and type) to the data, meaning a container encoded in these formats is always for a certain kind of data, not _any data_. That's where RDA, a "one-size-fits-all" data format, comes to play. RDA encoding allows converting data objects with arbitrarily complex structures to a text string - a data type supported by most computer systems and programming languages for manipulation and transportation. Using RDA, any data can be stored as text and be exchanged via text-based networks or messaging protocols, such as HTTP/RPC, TCP/IP, and FTP. 

## RDA's Noval Properties

> XML/JSON's schema-bound encoding space is like a wallet, with specific places for cards, notes, and coins, whilst RDA's space is like an infinitely expandable shelf that can store anything.

RDA's unique encoding features depend on its three unique properties - 

First, the storage locations in the space are addressed by integer indexes, rather than by names or string paths. This allows a client to access RDA's content easily without any prerequisite.

Second, the number of dimensionas of the space, and the length of the array at each dimension, can be (theoritically) infinitely expanded as required. So from a client's perspective, any required storage in the space is always available.

Third, as a sub-dimension can have unlimited dimensions because it can be infinitely expanded, it is a complete RDA storage itself, meaning **an RDA's encoding space is recursive** - an RDA can be stored inside another RDA as its child. This feature can be used for maintaining compatibility between multiple versions of data, i.e. you can have multiple children RDAs stored inside a parent RDA, each child RDA carries a different version of the data, and when the parent RDA is transferred to a recipient client, it can choose a compatible version to consume. 

## Charian - Programming RDA

Charian is a GitHub repo from Foldda that hosts RDA encoding and parsing API's in a number of programming languages, including C#, Java, and Python.

These API leverage RDA's unique properties, and intuitively use the postal service metaphor to hide the underlying encoding mechanics, i.e. using RDA as a "box" for storing data, and accessing the stored data items via integer-based indexing. 

For example, in C#, this is how a client app may send and recive data by firstly encoding the data as an RDA string into a file, then retrieve the data by reading and parsing the RDA string from the file.

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

## Enflow - A Practical Use

Enflow is a component-based computing system that allows an app be assembled using components potentially from any vendor. In its design, it needs to allow components with no prior knowledge to connect and collaborate with each other, including exchanging data that does not have a fixed data model. RDA is created for this design requirement and is used in the system's framework API as the primary data object for Enflow to interact with its hosted components, and for components to exchange data between each other.

In Enflow, a compatible component is required to convert its "native data" to and from RDA, by possibly using Charian, so its data can flow through the system. For example, the HL7FileReader component, available at the "Enflow Portable Components" repo, implements the conversion from HL7 to RDA, and the HL7FileWriter component does the opposite conversion, and these two components can be connected and used in an app that required HL7 data file reading and writing.

A demo of Enflow can be found in this video.

## Summary

> *RDA allows implementing a generic and unified data transport layer which applications can utilize for sending and receiving data. As the applications are "loosely coupled" using such a data transport layer, they are less dependent and easier to maintain if the data format is changed.*
 
One powerful feature of RDA is for implementing cross-language and cross-application object-serialization. For example, you can send a "Person" object as a serialized RDA container from your C# program to many receivers, and in a Python program, you can de-serialize a "User" object using data elements from the received RDA container. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately. 

Another feature of RDA is for maintaining version compatibility between a sender and a receiver. Because RDA's recursive storage allows storing an RDA inside another RDA, multiple versions (or different formats) of the data can be transported "side-by-side" (as child RDAs) in an RDA container, and the receiver can pick its preferred version or format to use. 

Indeed, being able to send multiple copies of _any data_ side-by-side in a container can be interestingly useful: like sending XML data together with its DTD[^3], or sending a digital document paired with its digital signature or public key, or implementing distributed computing by sending a computing "workload" to a data-processing unit where the wordload contains some data together with an executable script.

[^3]: If you wish, an XML or JSON document can be stored as a 'string' value inside an RDA container.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than a XML or JSON container with the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it does not have any reserved keyword or character and allows any charactor to be part of the data content. In contrast, for example, in XML the line-feed character in data has to be encoded as "\&\#xA;", otherwise it will cause corruption.

## More Details 

The [wiki of this project](https://github.com/foldda/rda/wiki) contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - miscellaneous topics and dicsussions.

## Legal 

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 

"Recursive Delimited Array" and "RDA" are trademarks of [Foldda Pty Ltd](https://foldda.com).

