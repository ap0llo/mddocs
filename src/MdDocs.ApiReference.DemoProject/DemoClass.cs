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
    /// Documentation can also contains list and tables. See <see cref="ListExample"/> for a showcase of the different supported list formats.
    /// </remarks>
    /// <example>
    /// Using the <c>example</c> tag, examples on how to use a type can be included in the documentation.
    /// To specify the language inside of a <c>code</c> element, use the <c>language</c> attribute.
    /// <code language="csharp">
    /// // create a new instance of DemoClass
    /// var instance = new DemoClass();
    /// </code>
    /// </example>
    /// <seealso cref="IDemoInterface"/>    
    /// <seealso cref="DemoStruct">By providing text in the <c>seealso</c> element, the link text can be changed</seealso>
    /// <seealso cref="DemoEnum" />
    /// <seealso cref="System.String">No link can be generated if the referenced type is defined in another assembly (<c>System.String</c> in this case)</seealso>
    /// <seealso cref="ListExample"/>
    public class DemoClass
    {
        /// <summary>
        /// An example of a public field.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a field
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <value>The tag <c>value</c> allows specifying the value a field represents</value>
        /// <seealso cref="Field2"/>
        public readonly int Field1;

        /// <summary>
        /// An example of a public field.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a field
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <value>The tag <c>value</c> allows specifying the value a field represents</value>
        /// <seealso cref="Field1"/>
        public readonly string Field2;

        /// <summary>
        /// An example of a public event.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case an event
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <seealso cref="Event2"/>
#pragma warning disable CS0067
        public event EventHandler<EventArgs> Event1;
#pragma warning restore CS0067

        /// <summary>
        /// An example of a public event.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case an event
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <example>
        /// Using the <c>example</c> tag, examples on how to use a member can be included in the documentation:
        /// <code language="csharp">
        /// var instance = new DemoClass();
        /// instance.Event2 += MyEventHandler;
        /// </code>
        /// </example>
        /// <seealso cref="Event1"/>
#pragma warning disable CS0067
        public event EventHandler<EventArgs> Event2;
#pragma warning restore CS0067

        /// <summary>
        /// An example of a read-only property.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a property
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <value>The tag <c>value</c> allows specifying the value a property represents</value>
        /// <exception cref="ArgumentException">Exceptions can be documented using the <c>exception</c> tag.</exception>
        /// <exception cref="InvalidOperationException" />
        /// <seealso cref="Property2"/>
        public string Property1 { get; }

        /// <summary>
        /// An example of a read/write property annotated with a custom attribute
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case, a property
        /// supplementing the information specified in the summary
        /// </remarks>
        /// <value>The tag <c>value</c> allows specifying the value a property represents</value>
        /// <seealso cref="Property1"/>
        [DemoPropertyAnnotation(DemoPropertyFlags.Flag2 | DemoPropertyFlags.Flag3)]
        public string Property2 { get; set; }

        /// <summary>
        /// An example of an obsolete property.
        /// </summary>
        /// <remarks>
        /// If a member is marked as obsolete using <see cref="ObsoleteAttribute"/> a warning is included in the generated documentation.
        /// </remarks>
        [Obsolete]
        public string Property3 => throw new NotImplementedException();

        /// <summary>
        /// An example of an indexer with a single parameter.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case the indexer.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <example>
        /// Using the <c>example</c> tag, examples on how to call a member can be included in the documentation:
        /// <code language="csharp">
        /// var instance = new DemoClass();
        /// var value = instance[42];
        /// </code>
        /// </example>
        /// <param name="index">Description of parameter <paramref name="index"/> provided using the <c>param</c> element.</param>
        /// <value>The tag <c>value</c> allows specifying the value a indexer represents</value>
        /// <exception cref="ArgumentException">Exceptions can be documented using the <c>exception</c> tag.</exception>
        /// <exception cref="InvalidOperationException" />
        /// <seealso cref="this[Int32, Int32]"/>
        public string this[int index] => throw new NotImplementedException();

        /// <summary>
        /// An example of an indexer with two parameters.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case the indexer.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <param name="x">Description of parameter <paramref name="x"/> provided using the <c>param</c> element.</param>
        /// <param name="y">Description of parameter <paramref name="y"/> provided using the <c>param</c> element.</param>
        /// <value>The tag <c>value</c> allows specifying the value a indexer represents</value>
        /// <seealso cref="this[Int32]"/>
        public string this[int x, int y] => throw new NotImplementedException();


        /// <summary>
        /// An example of an obsolete indexer.
        /// </summary>
        /// <remarks>
        /// If a member is marked as obsolete using <see cref="ObsoleteAttribute"/> a warning is included in the generated documentation. 
        /// The documentation also includes the message specified for the obsolete attribute.
        /// </remarks>
        [Obsolete("This indexer is obsolete. Use this[int,int] instead.")]
        public string this[object arg] => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new instance of <see cref="DemoClass"/>
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case the constructor.
        /// supplementing the information specified in the summary
        /// </remarks>
        public DemoClass()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DemoClass"/> with the specified parameters
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case the constructor.
        /// supplementing the information specified in the summary
        /// </remarks>      
        /// <param name="parameter">Description of parameter <paramref name="parameter"/> provided using the <c>param</c> element.</param>
        /// <exception cref="ArgumentException">Exceptions can be documented using the <c>exception</c> tag.</exception>
        /// <exception cref="InvalidOperationException" />
        public DemoClass(int parameter)
        {
        }


        /// <summary>
        /// Example of an overloaded method without parameters
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <returns>The return value of a method can be documented using the <c>returns</c> tag</returns>
        /// <exception cref="ArgumentException">Exceptions can be documented using the <c>exception</c> tag.</exception>
        /// <exception cref="InvalidOperationException" />
        /// <seealso cref="Method1(String)" />
        /// <seealso cref="Method2" />
        public string Method1() => throw new NotImplementedException();

        /// <summary>
        /// Example of an overloaded method accepting one parameter.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <param name="parameter">Description of parameter <paramref name="parameter"/> provided using the <c>param</c> element.</param>
        /// <seealso cref="Method1()" />
        /// <seealso cref="Method2" />
        public void Method1(string parameter) => throw new NotImplementedException();

        /// <summary>
        /// Example of an non-overloaded methods with a custom attribute.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// </remarks>
        /// <seealso cref="DemoClass.Method1()"/>
        /// <seealso cref="DemoClass.Method1(String)"/>
        [Demo]
        public void Method2() => throw new NotImplementedException();

        /// <summary>
        /// Example of a generic method.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For generic methods, the method documentation also includes the type parameters and their description.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">Description of type parameter <typeparamref name="T"/> (specified using the <c>typeparam</c> element)</typeparam>
        /// <param name="parameter">Description of parameters <paramref name="parameter"/></param>
        public void Method3<T>(T parameter) => throw new NotImplementedException();


        /// <summary>
        /// Example of an obsolete method.
        /// </summary>
        /// <remarks>
        /// If a member is marked as obsolete using <see cref="ObsoleteAttribute"/> 
        /// a warning is included in the generated documentation.
        /// </remarks>
        [Obsolete]
        public void Method4() => throw new NotImplementedException();

        /// <summary>
        /// Example of an overload of the binary + operator.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <param name="left">Description of parameter <paramref name="left"/> provided using the <c>param</c> element.</param>
        /// <param name="right">Description of parameter <paramref name="right"/> provided using the <c>param</c> element.</param>
        /// <returns>The return value of an operator can be documented using the <c>returns</c> tag</returns>
        /// <seealso cref="DemoClass.op_Subtraction"/>
        public static DemoClass operator +(DemoClass left, DemoClass right) => throw new NotImplementedException();

        /// <summary>
        /// Example of an overload of the binary - operator.
        /// </summary>
        /// <remarks>
        /// Remarks allow specification of more detailed information about a member, in this case a method.
        /// supplementing the information specified in the summary.
        /// <para>
        /// For overloaded members, there is a separate "Remarks" section for every overload.
        /// </para>
        /// </remarks>
        /// <param name="left">Description of parameter <paramref name="left"/> provided using the <c>param</c> element.</param>
        /// <param name="right">Description of parameter <paramref name="right"/> provided using the <c>param</c> element.</param>
        /// <returns>The return value of an operator can be documented using the <c>returns</c> tag</returns>
        /// <exception cref="ArgumentException">Exceptions can be documented using the <c>exception</c> tag.</exception>
        /// <exception cref="InvalidOperationException" />
        /// <seealso cref="DemoClass.op_Addition"/>
        public static DemoClass operator -(DemoClass left, DemoClass right) => throw new NotImplementedException();

    }
}
