using System;

namespace DemoProject
{
    /// <summary>
    /// A class meant to demonstrate how the generated documentation looks like.
    /// </summary>
    /// <remarks>
    /// The main purpose of this class is to showcase the generated documentation.
    /// <para>
    /// For that purpose, the class aims to include as many code constructs relevant
    /// for the generated documentation as possible.
    /// </para>    
    /// <para>   
    /// For every type, MdDoc will create a separate markdown page split into multiple sections.
    /// </para>
    /// <para>
    /// The type page starts with the "definition" section that provides basic info about the type.
    /// This includes the type's namespace and assembly as well as the inheritance hierarchy,
    /// implemented interfaces and applied attributes.
    /// The type info will be followed by the summary provided in the xml documentation comments.
    /// </para>
    /// <para>
    /// If there are any remarks for the type, a "Remarks" section is added (the section you are currently reading)
    /// </para>
    /// <para>
    /// All of a types constructors will be listed in a table in the "Constructors" section.
    /// The table contains a row for every constructor displaying the constructors signature
    /// and summary. As there is a separate page generated that provides more detailed info about the
    /// constructor, a link to that page is inserted.
    /// </para>
    /// <para>
    /// Similar tables are generated for a type's public fields, events, properties, indexers, methods and operator overloads
    /// </para>
    /// <para>
    /// Links to other members are supported (using the xml tag <c>see</c>), for example a link to 
    /// <see cref="IDemoInterface" />. References to types outside the assembly are written to the output
    /// but cannot be linked to, e.g. a reference to <see cref="String"/>
    /// </para>
    /// <para>
    /// The last section is the "See Also" section with links provided in the xml documentation using the <c>seealso</c> tag
    /// </para>
    /// <para>
    /// Similar pages are also generated for interfaces (see <see cref="IDemoInterface"/>), structs (see <see cref="DemoStruct"/>)
    /// and enums (see  <see cref="DemoEnum"/>)
    /// </para>
    /// </remarks>
    /// <seealso cref="IDemoInterface"/>
    /// <seealso cref="IDemoInterface"/>
    /// <seealso cref="DemoStruct">By providing text in the <c>seealso</c> element, the link text can be changed</seealso>   
    /// <seealso cref="System.String">No link can be generated if the referenced type is defined in another assembly (<c>System.String</c> in this case)</seealso>
    public class DemoClass
    {
        /// <summary>
        /// An example of a public field.
        /// </summary>
        /// <seealso cref="Field2"/>
        public readonly int Field1;

        /// <summary>
        /// An example of a public field.
        /// </summary>
        /// <seealso cref="Field1"/>
        public readonly string Field2;

        /// <summary>
        /// An example of a public event.
        /// </summary>
        /// <seealso cref="Event2"/>
        public event EventHandler<EventArgs> Event1;

        /// <summary>
        /// An example of a public event
        /// </summary>
        /// <seealso cref="Event1"/>
        public event EventHandler<EventArgs> Event2;

        /// <summary>
        /// An example of a read-only property.
        /// </summary>
        /// <seealso cref="Property2"/>
        public string Property1 { get; }

        /// <summary>
        /// An example of a read/write property annotated with a custom attribute
        /// </summary>
        /// <seealso cref="Property1"/>
        [DemoPropertyAnnotation(DemoPropertyFlags.Flag2 | DemoPropertyFlags.Flag3)]
        public string Property2 { get; set; }

        /// <summary>
        /// An example of an indexer with a single parameter.
        /// </summary>
        /// <seealso cref="this[Int32, Int32]"/>
        public string this[int index] => throw new NotImplementedException();

        /// <summary>
        /// An example of an indexer with two parameters.
        /// </summary>
        /// <seeaslo cref="this[Int32]"/>
        public string this[int x, int y] => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new instance of <see cref="DemoClass"/>
        /// </summary>
        public DemoClass()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DemoClass"/> with the specified parameters
        /// </summary>
        public DemoClass(int parameter)
        {
        }


        /// <summary>
        /// Example of an overloaded method without parameters
        /// </summary>
        /// <seealso cref="Method1(String)" />
        /// <seealso cref="Method2" />
        public void Method1() => throw new NotImplementedException();

        /// <summary>
        /// Example of an overloaded method accepting one parameter.
        /// </summary>
        /// <seealso cref="Method1()" />
        /// <seealso cref="Method2" />
        public void Method1(string parameter) => throw new NotImplementedException();

        /// <summary>
        /// Example of an non-overloaded methods with a custom attribute-
        /// </summary>
        /// <seealso cref="DemoClass.Method1()"/>
        /// <seealso cref="DemoClass.Method1(String)"/>
        [Demo]
        public void Method2() => throw new NotImplementedException();

        /// <summary>
        /// Example of an overload of the binary + operator.
        /// </summary>
        /// <seealso cref="DemoClass.op_Subtraction"/>
        public static DemoClass operator +(DemoClass left, DemoClass right) => throw new NotImplementedException();

        /// <summary>
        /// Example of an overload of the binary - operator.
        /// </summary>
        /// <seealso cref="DemoClass.op_Addition"/>
        public static DemoClass operator -(DemoClass left, DemoClass right) => throw new NotImplementedException();

    }
}
