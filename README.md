# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array (RDA) is a plain-text data format for encoding structured data as text strings. Its delimiter-based encoding is  similar to CSV, which is more robust and simpler to implement than the tag-based, schema-dependent XML and JSON. 

Compared to CSV, which is limited for encoding 2-dimensional data, RDA encoding utilises multiple delimiter chars for storing any complex structured data in a multidimensional-array space.

## RDA Examples

Below is an RDA-encoded string contains of a one-dimension array consists of three data elements "One", "Two", and "Three", sepatated by a delimiter char which is '|'.

```
|\|One|Two|Three
```

An RDA string has two sub-string sections for its functions: a header section (the "header") for dynamically defining the string's encoding chars; and a payload section (the "payload") for containing the encoded data elements. 

In the example, the header is the sub-string "|\\|", which in this case is defines only one delimiter (the first char '|') and the escape-char (the second letter '\\'). The third char '|' (the first repeat of the first char) marks the end of the header and the start of the payload, which is the rest of the string - "One|Two|Three".

If we want to encode and store 2-dimensional data in an RDA string, we need to define a second-dimension delimiter in the header and use it in the encoding, like in this next example. 

```
|,\|Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63
```

In this example, the header "|,\\|" defines the first dimension deleimiter '|' as the previous example, but also defines a second dimension delimiter as char ',', and the data encoded in the string is the content of the following 2-D table.

| Name | Sex | Age | 
|------|-----|-----|
| Mary | F   | 52  | 
| John | M   | 70  |
| Kate | F   | 63  | 

Following this encoding pattern, by defining more delimiters in an RDA string's header, we can encode and store higher-level multidimensional data in an RDA-formatted string[^1].

[^1]: A more detailed explanation of RDA encoding rule is in [this repo's wiki](https://github.com/foldda/rda/wiki).

## Schema-Neutral Data Exchange

Traditional data-exchange pipelines using XML and JSON can cause tight-coupling between the sender and the receiver. This is because the XML/JSON schemas are tied to the data, and if the data format changes the pipelines also need to change, meaning high cost in maintainance. Here we have a generic approach of schema-neutral data exchange, which can be explained with the following analogy. 

Imagine you're moving house: you would first pack household items into boxes, disassemble them if required. Once the boxes are delivered to the new place by a freight company, you could then unpack the boxes, reassemble the items, and re-place them to their designated places. Notice everyone's household content would be different but they can use the same freight company for house moving. The key is the sender and the receiver are responsible for packing and unpacking the contents, the freight company only moves packed boxes.

So for mimicing flexible house-moving scenario in data exchange between a data sender program and a receiver program, we can conceptually divid it  into two layers: the bottom "data transport" layer works like freight companies, that is, it is for moving the data but is insensitive to the data content and data structure changes; and the top "application" layer is responsible for "packing and unpacking" (interpreting and consuming) the data that have been transported. We leave the sender and the receiver programs to implement the application layer, as they are the logical places to handle any data structure changes that need to happen.

For the data transport layer, we want to make it to be data-content and data-structure independent - like a "freight comapany" for data, and the key to this is to have a media that can serve as **plain boxes** storage for storing various data and are also can be handled by conventional data communication methods and protocols, and RDA formatted strings are most suitable for this purpose.

## Charian - Using RDA Strings As Data Container

Charian is an API for easily encoding and parsing RDA format strings, which is available in C#, Python, and Java [from its GitHub repo](https://github.com/foldda/charian).

In the API, an RDA string is modeled as a data container, which has setter and getter methods that can be used to store and retrieve data by a data sender or receiver. Charian API makes the RDA string encoding and decoding easy and transparent to a user, and the intuitive "container" modeling fits perfectly into the concept of building loosely-coupled schema-independent data trasnport pipelines. 

It's worth pointing out that because strings are a generic data format supported in all modern languages and platforms, so data trasnport pipelines based on RDA-encoded strings are well suited for cross-language and cross-platform systems integration.

## SnapFusion - A Practical Example

SnapFusion is a component-based software framework for building applications using pre-existing software components - a bit like the process of manufacturing cars, where different parts - doors, wheels, windows and engine from different suppliers - are assembled into an automobil.

SnapFusion is designed to be generic and vendor-neutral, so it assume all components are independently-built and not necessarily have the same data model, and components connected via SnapFusion to communicate and interact, which is designed to allows to connect to each other and become an application. 

These components are highly  - they can be made by different vendors, from different time, and have different business function. 

between each othercan be used assamble applications using software components. One of the design challenge of SnapFusion is to allows any third-party software components that connect to the framework to commnunicate between each other, and these components can perform any business function, built by any parties, change at anytime, and can be built in the future. So the framework has to be **extremely flexible** to accommandate all these. 

One of the things has to be flexible is the data exchange format between the component and the framework, and between the components themselves. We chose to use RDA and Charian for achivieving these design goals, check out [the SnapFusion GitHub repo](https://github.com/foldda/snap-fusion) to see how these are practically implemented, with plug-n-play demos.

## Universal Data Exchange - The Big Picture

In an analogy, using dedicated pipelines for cross-system data exchange is like sending parcels to people through adhoc transport and delivery arrangements rather than using the Post Office, which is expensive and inflexible. 

<div align='center'>
<img src='img/Pre-Post-office-system.png' width='470' align='center'>
</div>

We are all used to using Postal services for posting parcels because the shared logistics and freight system helps cut down the cost. Postal servises' standardized packaging (i.e. envelops and boxes) is also flexible in meeting various clients' requirements - for posting parcels of different shapes and sizes.

<div align='center'>
<img src='img/Post-office-system.png' width='550' align='center'>
</div>

Similarily, using fixed data models and custom-built pipelines makes the connected programs “tightly coupled” - meaning they are inflexible and expensive not just financially but also in maintanance and operational complexity.

<div align='center'>
<img src='img/Pre-Charian-data-transport.png' width='550' align='center'>
</div>

So for data exchange between isolated independent systems, we could do something similar to the Post Office's postal service, to cut down the costs and improve flexibility.  

<div align='center'>
<img src='img/Charian-data-transport.png' width='550'>
</div>

Because RDA is schema-less, it is ideal for playing the role of the **plain boxes** in the implementation of UDX - it converts and stores complex structured data into simple, easy-to-parse text strings, so the data can be exchanged between individual programs with minimal and low-cost intermedia data transport layer, i.e. via text-based networks or messaging protocols, such as HTTP/RPC, TCP/IP, and FTP. 


## SnapFusion - A Practical UDX Example




can be understood as an extension of the well-known CSV formatencoding by allowing dynamically defining multiple delimiters. The delimitor-based RDA encoding Superior to XML and JSON, the simple, schema-less RDA encoding provides a generic space (an expandable, multi-dimensional array) for _storing_ a data object's properties - which are accessible via indexes[^1]. RDA can be seen as an enhanced CSV encoding that supports encoding multidimensional arrays of data, with additional encoding merits compared to CSV.

Because of its “one-size-fits-all” approach, RDA encoding is easier to use, faster to parse and encode, and more space-efficient. As explained below, RDA enables loosely-coupled communication, which is especially useful in facilitating independently developed devices and programs' collaboration - where data models between these devices and programs are commonly incompatible and constantly changing.  

[^1]: RDA's encoding space is logically an infinitely expandable multi-dimensional array, where the number of dimensions and the size of each dimension of the multi-dimensional array of an RDA-encoded string can be expanded as required, and in RDA, a data object's attributes values are simply stored in the space as strings i.e. no specific data types. 

## The Problem With Cross-System Data Exchange 

Data exchange between two separated systems can be difficult to implement (and maintain) due to their often incompatible data models. Enforced "common schema" and/or dedicated custom-built pipelines are usually required to bridge the gaps if two incompatible systems require integration.



## RDA: Schema-less Encoding

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

Second, both the number of dimensions of the space and the length of the array at each dimension can be dynamically expanded as required. So for a client's encoding needs, the space's storage is practically sufficient and is always available.

Third, as a sub-dimension in an RDA space is also a multi-dimensional array, it is also an RDA storage itself, meaning **an RDA's encoding space is recursive** i.e. an RDA can be stored inside another RDA. This is why RDA can be used to store arbitrarily complex data objects.

## Charian - The Official RDA Encoding/Decoding API

[Charian](https://github.com/foldda/charian) is an open-source RDA encoding and parsing API implemented in C#, Java, and Python.

Charian API intuitively hides the underlying RDA encoding and parsing details from the clients, where the data serialization process via the API is similar to posting a parcel using the post office. That is, a client uses an RDA-encoded string as a "container" object for storing data and accessing the stored data items in an RDA container via integer-based indexing. For example, in C#, this is how a client app may send and receive data by firstly encoding the data as an RDA string into a file, then retrieving the data by reading and parsing the RDA string from the file.

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

## Snappable - A Practical Use of RDA

[Snappable](https://github.com/foldda/snappable) is an open-source component-based computing framework. It is used for defining software component intefaces, so compatible software components that are reusable and interchangeable. By using generic, interchangeable software component, an app developer can assemble "non-proprietary" apps using components made by different vendors from a open market, rather than writing code from scratch or using proprietary components and being locked-in by a specific component vendor. 

In Snappable's design, components are independent, having minimal assumed knowledge when connecting and collaborating to another component, i.e., when two components exchanging data, they don't assume the data has a specific data model. This allows the Snappable to implement very generic "plugs" for plugging in interchangeable components which is the design goal of the framework. In fact, RDA is created for this design requirement and is a primary data type used throughout the Snappable framework.

In Snappable, a compatible component is required to convert its "native data" to and from RDA, possibly by using [Charian](https://github.com/foldda/charian), so the data (carried within an RDA) can flow through the system. For example, in its HL7FileReader component implements the conversion from HL7 to RDA, and the HL7FileWriter component does the opposite conversion, and these two components (both available in the Snappable GitHub repo) can be connected and used in an app that requires HL7 data file reading and writing.

[This demo video](https://www.youtube.com/watch?v=Uek9aW1qToU) visually demonstrates Snappable components in-action. It shows how an app can be assembled "physically" form pre-built interchangeable Snappable components.

The Snappable framework API and many of its ready-to-use portable components are available in [this GitHub repo](https://github.com/foldda/snappable).

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

