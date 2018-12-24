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
    /// For every type, MdDoc will create a seprate markdown page split into multiple sections.
    /// </para>
    /// <para>
    /// The type page starts with basic info about the type. This includes the type's namespace and assembly
    /// as well as the inheritance hierarchy, implemented interfaces and applied attribues.
    /// The type info will be followed by the summary provided in the xml documentation comments.
    /// </para>
    /// <para>
    /// If there are any remarks for the type, a "Remarks" section is added (the sections you are currently reading)
    /// </para>
    /// <para>
    /// All of a types constructors will be listed in a table in the "Constructors" section.
    /// The table contains a line for every constructor displaying the constructors signature
    /// and summary. As there is a separate page generated that provides more detailed infor about the
    /// constructor, a link to that page is inserted.
    /// </para>
    /// <para>
    /// Similar tables are generated for a type's public fields, events, properties, methods and operator overloads
    /// </para>
    /// <para>
    /// Links to other members are supported (using the xml tag <c>see</c>), for example a link to 
    /// <see cref="IDemoInterface" />. References to types outside the assembly are written to the output
    /// but cannot be linked to, e.g. a reference to <see cref="String"/>
    /// </para>
    /// <para>
    /// The last section is the "See Also" section with links provided inline using the <c>seealso</c> tag
    /// </para>
    /// </remarks>
    /// <seealso cref="IDemoInterface"/>
    /// <seealso cref="System.String">String might also be interesting</seealso>
    /// <seealso cref="DemoStruct">DemoStruct has a similar purpose but is a value type</seealso>
    public class DemoClass
    {
        /// <summary>
        /// A public field
        /// </summary>
        public readonly int Field1;

        /// <summary>
        /// Raised when the operation is finished
        /// </summary>
        public event EventHandler<EventArgs> OperationCompleted;

        /// <summary>
        /// Gets the value of <see cref="Property1"/>
        /// </summary>
        public string Property1 { get; }

        /// <summary>
        /// Gets the value of <see cref="Property2"/>
        /// </summary>
        [DemoPropertyAnnotation(PropertyFlags.Flag2 | PropertyFlags.Flag3)]
        public string Property2 { get; }

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
        /// Does something ;)
        /// </summary>        
        public void DoSomething(string parameter) => throw new NotImplementedException();

        /// <summary>
        /// Does something else
        /// </summary>        
        [Demo]
        public void DoSomethingElse() => throw new NotImplementedException();

        /// <summary>
        /// Combines two instances of <see cref="DemoClass"/>
        /// </summary>
        public static  DemoClass operator+(DemoClass left, DemoClass right)
        {
            throw new NotImplementedException();
        }
    }
}
