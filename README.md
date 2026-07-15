# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array (RDA) is a plain-text data format for encoding structured data as text strings. RDA uses a simple, delimiter-based encoding, similar to CSV, but supports encoding more complex data structure compared to CSV.

An RDA-encoded string (an "RDA string") has two substring sections - a "header" and a "payload". The header substring contains definition of the string's encoding chars definition[^1], and the payload substring contains the encoded data elements. It allows a parser program to configure itself dynamically when reading the header, and subsequently parse the string's payload content.

[^1]: A more detailed explanation of RDA encoding rule is in [this repo's wiki](https://github.com/foldda/rda/wiki).

In the example below, the header (substring "|\\|") defines a delimiter (the first char '|') which is used to encode the payload (substring "One|Two|Three"), where three data elements ("One","Two", and "Three") are separated using the delimiter. 

```
|\|One|Two|Three
```

RDA encoding supports encoding multi-dimensional data, which is done by expanding the header and defining more delimiters. In the example below, two delimiters ('|' and ',') are defined in the RDA string's header, and the string's payload contains an encoded 2-D data table: the rows are separated by the "first-dimension" delimiter '|', and the columns in each row are separated by the "second-dimension" deleimiter ','. 

```
|,\|Name,Sex,Age|Mary,F,52|John,M,70|Kate,F,63
```

| Name | Sex | Age | 
|------|-----|-----|
| Mary | F   | 52  | 
| John | M   | 70  |
| Kate | F   | 63  | 

## Data Exchange Late-Binding 

> In programming, late-binding allows a prpogram to adapt to changing environments, handle unknown object types, and avoid strict type-dependent links.

When two programs exchange data in XML or JSON format, the data exchange implementation is bound to an XML/JSON schema. This can be a problem if the exact format for the data cannot be certain innitially, or can have varians, or may change (as they do) over time. 

Data exchange late binding, like late-binding in programming, offers flexibility, version independence, and dynamic extensibility to the sender and the receiver, that is, the data format can be determined later and the varians be dealt with accordingly by either or both the sender and the receiver, and **RDA is designed for implementing data exchange late-binding**. Let's explain this with an analogy. 

Imagine you're moving house: you would first pack household items into boxes, _disassemble_ them if required, and once the boxes are delivered to the new place, perhaps by a freight company, you would unpack the boxes, _reassemble_ the items, and re-place them to their designated places. Note in this process, the sender, the receiver, and through the process no party needs to agree the exact shape and the size of each household item - everything is wrapped in generic boxes until the time the receiver unwrap the packaging and "consumes" the content inside the boxes. 

> Data exchange late-binding involves a sender disassembling a complex data object into properties and values, and a receiver reassembling the properties and values back into consumable data. In between, it requires a data container storage for carrying the disassembled properties and values - a storage that does not restrict what can be carried, like a "plain box". 

As discussed earlier, XML/JSON based data exchange are schema-dependent that restricts on data format, and the schema-less CSV encoding is too primitive for carrying complex structured data. In contrast, the schema-less RDA encoding does not have these limitation and restriction and is suitable for data exchange late-binding.

## Charian - More Than An RDA-Encoding API

Charian is a simple RDA encoding and parsing API, that serves as an easy-to-use tool for late-binding data exchange. Specifically, the API assists  programs late-binding data exchange through providing a class that is modeled as a generic schema-less container to be used for "packing and unpacking data" for the data exchange. This is explained below using the API's C# implementation[^2] as an example.

[^2]: Charian is current available in C#, Python, and Java [from its GitHub repo](https://github.com/foldda/charian), and potentially more languages will be supported in the future. 

### The Rda Class (C#)

In the API, the Rda class has setter and getter methods for programs storing and retrieving data[^3]. An Rda class object can be serialized into, and be de-serialized from, an RDA-encoded string:

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
[^3]: You may have noticed the API's Rda class supports storing only two data type values: the first is type "string", the second is type "Rda" (via recurrsion). The recurrsion takes advantage of an RDA string's interesting property for havinv a "**recursive storage structure**", that is, you can store an Rda object inside another Rda object. That's because the RDA's multi-dimensional encoding space can be (almost) unlimited expanded into new deminsions (through introducing additional dilimiters to the encoding process), and any one sub-dimension is also a multi-dimensional array itself and offers the same storaging property and capacity as its containing parent-dimension. Reflecting this in the API is that an Rda object (having a multi-dimensional space) can be stored inside another Rda object (as one of its sub-dimensions).

The Rda is modeled as a data container for ebing used in late-binding style data exchange. In the following example illustrates how a program utilizes an Rda object to transfer some random data via an RDA-encoded string that is temporarilly stored in a file. Note how the container object can automatically expand to accomandate extra data elements without having any restrictions on the data that can be transported.

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

        Rda rda1 = new Rda();    //an Rda object is created as a "generic data container"

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

Charian can also easily serializing complex data objects into RDA strings. More details and code examples are available at [its GitHub repo](https://github.com/foldda/charian)

## Snappable - Data Exchange Late-Binding In Practice

[Snappable](https://github.com/foldda/snappable) is an open-source component-based computing framework that allows assembling software applications using reusable and interchangeable software components. One of Snappable's design requirements is to allow third-parties' software components to connect and work together. This is an ideal case for data exchange late binding, because these components won't necessary have any prior knowledge of the data model used by each other. In fact, RDA encoding and data exchange late-binding concept are created for this design requirement, and RDA is a primary data type used throughout the Snappable framework for data exchange.

[This demo video](https://www.youtube.com/watch?v=Uek9aW1qToU) visually demonstrates Snappable components in-action. It shows how an app can be assembled "physically" form pre-built interchangeable Snappable components.

In a Snappable application, components can "plug" themselves to the framework and exchange data between each other, because Snappable has defined API intefaces that implements data exchange late binding, so even components made by different companies can immediately engage and collabrate - without having to be bonded by a "fixed/agreed data model"[^4].

[^4]: In Snappable, a compatible component implementing the late binding is required to convert its "native data model" to and from RDA, possibly by using [Charian](https://github.com/foldda/charian), so the data (carried within an RDA) can flow through the system. For example, there is a  HL7FileReader component implements the conversion from HL7 to RDA, and the HL7FileWriter component does the opposite conversion, and these two components (both available in the Snappable GitHub repo) can be connected and used in an app that requires HL7 data file reading and writing.

The Snappable framework API and many of its ready-to-use portable components are available in [this GitHub repo](https://github.com/foldda/snappable).

## The Bigger Picture

Today, most cross-system data exchange are using dedicated custom-built data pipelines based on XML/JSON. Fixed data models (XML/JSON schemas) used in these pipelines make the connected programs “tightly coupled” - meaning they are inflexible and expensive not just financially but also in maintanance and operational complexity.

<div align='left'>
<img src='img/Pre-Charian-data-transport.png' width='550' align='center'>
</div>

This is like sending parcels to people through adhoc transport and delivery arrangements rather than using the Post Office, which is expensive and inflexible. 

<div align='left'>
<img src='img/Pre-Post-office-system.png' width='470' align='center'>
</div>

We are all used to using Postal services for posting parcels because the shared logistics and freight system helps cut down the cost. Postal servises' standardized packaging (i.e. envelops and boxes) is also flexible in meeting various clients' requirements - for posting parcels of different shapes and sizes.

<div align='left'>
<img src='img/Post-office-system.png' width='550' align='center'>
</div>

So for data exchange between isolated independent systems, we could do something similar to the Post Office's postal service, to cut down the costs and improve flexibility.  

<div align='left'>
<img src='img/Charian-data-transport.png' width='550'>
</div>

By using the schema-less RDA and through late-binding, data can be exchanged between individual programs with generic and low-cost intermedia data transport layer, i.e. via text-based networks or messaging protocols, such as HTTP/RPC, TCP/IP, and FTP. As in the moving house analogy, these low-cost generic data links are like using cheap "freight companies", that is, to be based on simple and low-cost data transports, because sender and receiver programs can use generic APIs (eg file-system, RDBMS, FTP, MSMQ APIs) for cross-system data transfer, in contrast to building higher cost dedicated pipelines using XML/JSON encoding, where sender and receiver programs must maintain inflexible data-handling logic for the data exchange.

## More Details 

The [wiki of this project](https://github.com/foldda/rda/wiki) contains more details about RDA, including - 

- [RDA overview.](https://github.com/foldda/rda/wiki#1-introduction) - explains the background and philosophy of this project.
- [Using the API.](https://github.com/foldda/rda/wiki#2-using-the-api) - contains more technical details, with a practical example. 
- [FAQ.](https://github.com/foldda/rda/wiki#4-faq) - miscellaneous topics and dicsussions.

## Legal 

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 

"Recursive Delimited Array" and "RDA" are trademarks of [Foldda Pty Ltd](https://foldda.com).

