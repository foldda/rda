# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array (RDA) is a plain-text data format for encoding structured data as text strings. It's a delimiter-based encoding, similar to CSV, which is more robust and simpler to implement compared to the tag-based, schema-dependent XML and JSON. 

An RDA-encoded string, or simply "RDA string", consists a "header" and a "payload" substring sections. The header substring contains definition of all the string's encoding chars[^1], and the payload substring contains the encoded data elements. It is intended that a parser program can parse the string's data content in the payload without separate configuration, as it is sufficient for the parsing by just reading the header.

[^1]: A more detailed explanation of RDA encoding rule is in [this repo's wiki](https://github.com/foldda/rda/wiki).

In the example below, the header, substring "|\\|", defines a delimiter (the first char '|') and the payload, substring "One|Two|Three", contains three data elements ("One","Two", and "Three") encoded using the delimiter. 

```
|\|One|Two|Three
```

The encoding definition in an RDA string's header can be dynamically expanded to include more delimiters, which can be used to encode higher-dimension data when it's required. This is in contrast to CSV, where it can only encode 2-dimensional data using a fixed, predefined delimiter. 

In the next example, the RDA string's header contains definition a second delimiter char ',' which is an extention from the previous example, and the data inside the payload is a 2-D data table, where the first-dimension rows are separated by delimiter '|', and the second-dimension columns in each row are separated by deleimiter ',' - 

```
|,\|Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63
```

| Name | Sex | Age | 
|------|-----|-----|
| Mary | F   | 52  | 
| John | M   | 70  |
| Kate | F   | 63  | 

## Data Exchange Late-Binding 

> In programming, late-binding allows a prpogram to adapt to changing environments, handle unknown object types, and avoid strict type- dependent links.

When two programs exhange data between each other using XML or JSON encoding, they must first agree to a data format (i.e. an XML/JSON schema) for the data-exchange. This causes inflexibility because it limits the possibility that the data can be uncertain and can change (as they do). If one program decides to change its data format, it becomes a costly excercise for all the other programs that want to maintain the established data exchange connections.

Using RDA format encoding in data exchange allows "late binding" in data handling. That is, it does not require the programs to pre-agree on a data format, so it's more flexible in handling the data exchange. Let's explain this with an analogy. 

Imagine you're moving house: you would first pack household items into boxes, disassemble them if required, and once the boxes are delivered to the new place, perhaps by a freight company, you would unpack the boxes, reassemble the items, and re-place them to their designated places. Note in this process, the sender, the receiver, and the freight company don't need to agree the exact shape and the size of each household item - everything is wrapped in generic box containers until the time the receiver unwrap the packaging and "consumes" the box's content. 

When using RDA encoding in the data exchange, because RDA strings are schema-less, they are like the generic plain boxes for moving the data around, without the parties having to agree on a specific data object type/format to be used in the data exchange. **RDA encoding allows data exchange late-binding.**

Similar programming late-binding and the moving house analogy, late binding in data exchange can offers significant advantages in flexibility, version independence, and dynamic extensibility. It also allows using versatile, simple, and low-cost data transport: because the data encoded in RDA format is physically a text string, any protocol or mechanism (eg. a file system or a database or via FTP, MSMQ etc) that handles string data type can be used to be the "freight company". This is in contrast to a program must build dedicated data exchange logic or even dedicated pipelines for data exchange, if using XML/JSON encoding for specific data formats.

## Charian - A RDA-Based Serialization API

Charian is an easy-to-use API for transparently encoding and parsing RDA formated strings. 

### The Rda Class

If we think an RDA string is the container used in late-binding data transportation, a program would only care about "packing" its data into the container before the transportation, and "unpacking" its data after the transportation, rather than care about how the end RDA string would be formatted. So in the API, it hides the RDA encoding details and models an RDA string a data container object that has setter and getter methods for storing data into and retrieve data from it. 

```csharp
class Rda
{
    //methods for storing data element values inside an indexed space that an RDA string provides
    public void SetValue(string value, int[] address);  /* save a string value at the index-addressed location */
    public string GetValue(int[] address);        /* retrieve a string value from the index-addressed location */
    public void SetRda(Rda rda, int[] address);      /* save an Rda object at the addressed location */
    public Rda GetRda(int[] address);      /* retrieve an Rda object from the addressed location */

    //serialize this Rda object to an RDA-encoded string, for transportation
    public override string ToString();
}
```

You may have noticed the API's Rda class supports storing only two data type values: the first is type "string", the second is type "Rda" (via recurrsion). The recurrsion takes advantage of an RDA string's interesting property for being a "**recursive storage**", that is, you can store an Rda object inside another Rda object. That's because the RDA's multi-dimensional encoding space can be (almost) unlimited expanded through introducing additional dilimiters to the encoding process, and a sub-dimension (multi-dimensional) array itself offers the same storaging property and capacity as its containing upper-dimension multi-dimensional array.

### The IRda Interface

Data objects implement this interface to turn itself to/from an Rda object, so to be transported/exchanged. 

```csharp
interface IRda
{
    /* "packing": return an Rda object contains this data object's elements/properties values  */
    Rda ToRda();

    /* "unpacking": restore this data object's elements/properties values from an Rda object */
    IRda FromRda(Rda rda);  
    //... ...
}
```

It's worth pointing out strings are a generic data format supported in all modern languages and platforms, so data packed in RDA-encoded strings are well suited for cross-language and cross-platform systems data exchange.

Charian is available in C#, Python, and Java [from its GitHub repo](https://github.com/foldda/charian). 

## SnapFusion - A Practical Example

SnapFusion is a component-based software framework for building applications using pre-existing software components - a bit like the process of manufacturing cars, where different parts - doors, wheels, windows and engine from different suppliers - are assembled into an automobil.

SnapFusion is designed to be generic and vendor-neutral, so it assume all components are independently-built and not necessarily have the same data model, and components connected via SnapFusion to communicate and interact, which is designed to allows to connect to each other and become an application. 

These components are highly  - they can be made by different vendors, from different time, and have different business function. 

between each othercan be used assamble applications using software components. One of the design challenge of SnapFusion is to allows any third-party software components that connect to the framework to commnunicate between each other, and these components can perform any business function, built by any parties, change at anytime, and can be built in the future. So the framework has to be **extremely flexible** to accommandate all these. 

One of the things has to be flexible is the data exchange format between the component and the framework, and between the components themselves. We chose to use RDA and Charian for achivieving these design goals, check out [the SnapFusion GitHub repo](https://github.com/foldda/snap-fusion) to see how these are practically implemented, with plug-n-play demos.

## A Low-Cost Generic Data Exchange Framework - The Big Picture

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

