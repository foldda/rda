# Recursive Delimited Array 
[![Awesome](https://cdn.jsdelivr.net/gh/sindresorhus/awesome@d7305f38d29fed78fa85652e3a63e154dd8e8829/media/badge.svg)](https://github.com/sindresorhus/awesome#readme)

<img src="docs/image/rda_logo.png" align="right" height="128">

Recursive Delimited Array, or RDA, is an encoding format for storing and transporting structured data in a text string.

Unlike XML and JSON using a schema to restrict the data to the types and structure of a specific application, RDA is a **schema-less** format for **generic** data. An RDA-encoded string (aka. "RDA container") can be used for storing any data from any application. 

## A Problem With XML/JSON

> *An RDA container is like a bag that has unlimited number of pockets for storing "anything", and an XML or JSON container is like a wallet that offers specific places for coins, notes, and cards.* 

When use XML/JSON format for data exchange between two applications, a developer must firstly decide the data types and data structure for the applications, and fix the decided data format in a schema. If one of applications wants to change its data format and the schema, it will be difficult to manage compatibility especially if the applications are maintained by different parties. Imagine if Twitter or Google changes the data format in their REST API, a lot of downstream applications will be affected.

In contrast, while RDA is also capable for storing complex structured data, it is designed to remove anything "application specific"[^1]:

[^1]: Full details of the encoding rules can be found [here](https://foldda.github.io/rda/rda-encoding-rule).

* Instead of using tags or markups, RDA uses delimited encoding for separating and structuring data elements; 
* Instead of using named paths, RDA uses integer-based indexes for addressing data elements inside a container; 
* Instead of having multiple, specific data types, RDA has only two generalized data types[^2]: _RDA_ and _string_, for "composite" and "primitive" data values repectively.
 
[^2]:RDA data types and data structure are [discussed here](https://foldda.github.io/rda/data-type-and-data-structure). 

These make RDA **application independent** and **generic**, meaning any application can freely use an RDA container as the media for exchanging data with each other. It means the data content can be decided later and can evolve when required, and gives the application the flexibility to handle any changes.

## Benefits of RDA

> *RDA allows implementing a generic and unified data transport layer which applications can utilize for sending and receiving data. As the applications are "loosely coupled" using such a data transport layer, they are less dependent and can be flexibly maintained if the data format is changed.*
 
One powerful feature of RDA is for implementing cross-language and cross-application object-serialization. For example, you can send a "Person" object as a serialized RDA container from your C# program to many receivers, and in a Python program, you can de-serialize a "User" object using data elements from the received RDA container. Because there is no schema to be adhered to, the "Person" object and the "User" object can be programmed differently and be maintained separately. 

Another feature of RDA is for maintaining version compatibility between a sender and a receiver. Because RDA's recursive storage allows storing an RDA inside another RDA, you can transfer copies of multiple versions or formats of your data "side-by-side" (as child RDAs) in an RDA container, and the receiver can pick the right version or format to its preference. 

Indeed, being able to send multiple pieces of "anything" side-by-side in a container can have many interesting uses: like sending XML data together with its DTD[^3], or sending a digital document paired with its digital signature or public key, or sending a computing "workload" that has some data together with an executable script to a data-processing unit, etc.

[^3]: An XML or JSON document can be converted to a single 'string' data element, and be stored inside an RDA container.

Also, thanks to its simple and efficient delimiter-based encoding, an RDA container is much more compact than a XML or JSON container with the same content, and it is much easier to parse. RDA encoding is also more robust and resilient to data corruption, as it does not have any reserved keyword or character and allows any charactor to be part of the data content. In contrast, for example, in XML the line-feed character in data has to be encoded as "\&\#xA;", otherwise it will cause corruption.

## Getting Started
> *The super-lightweight API has no 3rd party dependency and requires no installation. It contains only one class and one interface.*

To use the RDA encoding API from this project, all you need is to include the provided API source code (available in [C#](https://github.com/foldda/rda/tree/main/src/CSharp), [Java](https://github.com/foldda/rda/blob/main/src/Java/), and [Python](https://github.com/foldda/rda/blob/main/src/Python)) in your project, and start using the provided class and interface in your program, as explained below - 

#### _Using class Rda_

The _Rda class_ implements the RDA encoding and decoding. It provides - 

* **Setter-Getter** methods are for storing and retrieving the container's content using index-based addresses. 
* **ToString** method is for serializing the container and its content, i.e. apply RDA-encoding and make it into a string. 
* **Parse** method is for de-serializing an RDA-encoded string back to an RDA container object with content.

The Rda class is for serializing data objects and is modeled as a "container". The idea is, since an Rda container (and its content) is serializable, rather than serializing a data object directly, all we need is to store the data object or its properties into the Rda container.

Below is an example of serializing and de-serializing data values using the Rda class and its methods[^4].

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

// ... the encoded RDA string can be saved to a file, or be sent to another app via network ...

//Parse(): de-serialize an RDA-format encoded string back to an RDA container object 
Rda rdaReceived = Rda.Parse(@"|\|One|Two|Three");   

//GetValue(): retrieve a value from in an RDA container at an index location    
System.Console.WriteLine(rdaReceived.GetValue(2));   //print "Three", the value stored at index=2 in the container.

```

#### _Using interface IRda_

A class implements the two methd defined in _IRda interface_ to specify how its properties can be stored in and be restored from an Rda container, for serialization:

* **ToRda()**: produces an Rda container object that contains specific properties of the object. 

* **FromRda(rda)**: restores the object's specific properties from values in a given Rda container object.

Here is an example - 

```c#
public class Person : IRda
{
   //properties of Person class
   public string Name = "John Smith";
   public int Age = 30;

    //define the index locations in the container for storing the properties
    const int IDX_NAME = 0, IDX_AGE = 1;
        
    //store the properties into an RDA
    public Rda ToRda()
    {
       var rda = new Rda();  //create an RDA container
            
       //store the properties to designated places in the container
       rda.SetValue(IDX_NAME, this.Name);
       rda.SetValue(IDX_AGE, this.Age.ToString()); //convert int to string
            
       return rda;     //the result
    }

    //restore the properties from an RDA
    public IRda FromRda(Rda rda)
    {
       try
       {
          this.Name = rda.GetValue(IDX_NAME);
          this.Age = int.Parse(rda.GetValue(IDX_AGE)); //convert string to int
       }
       catch
       {
           //handle error here ..
       }
       return this;
    }
}
```

A more complex object-serialization example using RDA container [can be found here.](https://foldda.github.io/rda/object-serialization-pattern).

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

