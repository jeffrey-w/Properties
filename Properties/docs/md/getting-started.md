# Getting Started

Let's begin by defining a regular POCO class.

```csharp
public class Poco
{
    public int Property1 { get; set; }
    public int Property2 { get; set; }
}
```

This is all well and good. However, what happens as we add more and more
properties? The interface begins to grow unwieldy. At the same time, as we add
more and more properties, some of them (perhaps a majority) must be declared
nullable since only a subset (perhaps a small one) exhibit a value for it.

```csharp
public class Poco
{
    public int Property1 { get; set; }
    public int Property2 { get; set; }
    public int? OptionalProperty1 { get; set; }
    public int? OptionalProperty2 { get; set; }
    // ...
    public int? OptionalPropertyN { get; set;}
}
```

This is essentially the same issue that arises from sparse database relations.
We could continue to maintain the large, incoherent API, but maybe there's a
better way. What if we could collapse the largely optional portion of the API
into a small, coherent interface, and only maintain a core set of properties
that are shared by all objects? That is the problem that the Properties library
solves.

[Properties](xref:Properties.Property) are modelled as associations between
strings and [Values](xref:Properties.Value) constituting a uniquely identified
piece of information. Properties are incident on instances of the
[IPropertyDictionary](xref:Properties.IPropertyDictionary\`1) interface, which
may be thought of as sets of `Property` instances, or partial functions from
string to `Value`. Indeed, the interface exposes properties and operations that
conform to both interpretations. Let's see how we can implement the
`IPropertyDictionary` interface to mitigate the issues caused by our bloated
`Poco` class.

```csharp
public class PropertyDictionary : IPropertyDictionary<PropertyDictionary>
{
    public Value this[string name] => 
        _properties
            .TryGet(name, out var property) 
        ? property.Value 
        : Value.Empty;
    
    public int Property1 { get; set; }
    public int Property2 { get; set; }
    
    public IEnumerable<Property> Properties => _properties.Values;
    
    private readonly Dictionary<string, Property> _properties = [];
    
    // Other method implementations ...
}
```

Great, we've preserved our core API (i.e., `Property1` and `Property2`), while
allowing other attributes to be captured by the `IPropertyDictionary` interface.
The question remains, how do we attach other properties to objects.

## Property Attachment and Immutability

The `IPropertyDictionary` interface declares two methods for specifying exactly
which attributes an instance exhibits.

- [WithProperty](xref:Properties.IPropertyDictionary\`1.WithProperty(System.String,Properties.Value))
  &mdash; includes a new [Property](xref:Properties.Property) on a copy of the
  target, given a string identifier and a [Value](xref:Properties.Value) to
  associate with it.
- [WithoutProperty](xref:Properties.IPropertyDictionary\`1.WithoutProperty(System.String))
  &mdash; excludes an existing `Property` on a copy of the target, given a
  string identifier.

Notice that each of these methods is specified to induce a *copy* of the object
on which it is called, and it is the copy that reflects the intended change.
That is, the `IPropertyDictionary` interface is immutable. Since the
aforementioned methods cannot change the internal state of the objects that they
are called on, it is much easier to reason about their effects.

## Implementing `IPropertyDictionary`

A majority of the `IPropertyDictionary` interface is implemented by the
[BasePropertyDictionary](xref:Properties.BasePropertyDictionary\`1) class, and
it is recommended that you extend it rather than implement the interface from
scratch.

```csharp
public class PropertyDictionary : BasePropertyDictionary<PropertyDictionary>
{
    public int Property1 { get; }
    public int Property2 { get; }
    
    public PropertyDictionary(int property1, int property2) : this([])
    {
    }
    
    protected PropertyDictionary(
        int property1, 
        int property2, 
        ImmutableDictionary<string, Property> properties) : base(properties)
    {
        Property1 = property1;
        Property2 = property2;
    }
    
    protected PropertyDictionaryPoco WithProperties(ImmutableDictionary<string, Property> properties)
    {
        return new PropertyDictionaryPoco(Property1, Property2, properties);
    }
}
```

We've also made the statically declared properties of the class read only by
removing their setters and assigning their values from a constructor.

## Computing `IPropertyDictionary` Differences

Suppose you have references to two `IPropertyDictionary` instances, and you'd
like to know how to map from the first to the other. To achieve this, you may
call `Diff` on the first, passing the second as an argument. This results in a
[Patch](xref:Properties.Patches.Patch) object that may be
[applied](xref:Properties.Patches.Patch.Apply*) to any `IPropertyDictionary` to
induce the changes necessary to perform the aforementioned mapping.

## Specifying Property Attachment and Usage

Since attaching and querying additional properties on objects is essentially
dynamic in nature, it is helpful to make these actions explicit. That is why
the library exports a means for identifying classes that do so. The
[PropertyAttachmentAttribute](xref:Properties.Attributes.PropertyAttachmentAttribute)
indicates that a targeted class associates properties with specified names with
instances of a specified type. On the other hand, the
[PropertyUsageAttribute](xref:Properties.Attributes.PropertyUsageAttribute)
indicates
that a class queries a specified property name on a specified type. By recording
this information, you may infer unnecessary property attachments, and the much
more deleterious querying of undefined properties.

## Values

The [Value](xref:Properties.Value) class wraps values from one of a small subset
of types that are allowed to be associated with a
[Property](xref:Properties.Property).

- `bool`
- `int`
- `double`
- `string`
- `IEnumrable<string>`

Limiting `Value` instances to this domain allows for easier maintenance of the
immutability of [IPropertyDictionary](xref:Properties.IPropertyDictionary\`1)
instances. All the previously listed types are implicitly convertable to
`Value` except for `IEnumerable<string>`. To obtain a `Value` that holds the
latter, you may use the [From](xref:Properties.Value.From*) factory method. In
addition, you may use [Make](xref:Properties.Value.Make*) factory method to
create a `Value` from a raw `object?` instance provided its underlying type is
one of those allowed.

Unwrapping values may be achieved in one of two ways depending on whether you
are certain that it does in fact hold data. If you are sure that a `Value` is
not [empty](xref:Properties.Value.Empty), you may access one of several
properties that corresponds to the type of data you expect it to hold. For
example, to obtain an `int` from a `Value`, you may access the
[Integer](xref:Properties.Value.Integer). On the other hand, if you cannot know
whether a given property is actually incident on an object of interest, you
may call one of several methods to access the data if it exists, or otherwise
provide a default value that may be specified. Continuing our previous example,
[IntegerOrDefault](xref:Properties.Value.IntegerOrDefault*) obtains the `int`
data associated with a `Value`, if it is present. Accessing a property or
calling a method that attempts to read the data associated with a `Value` as a
type it does not belong to will result in an
[InvalidCastException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidcastexception).

That's the gist of it. Using the classes and constructs exported by the
Properties library, you can develop flexible interfaces that remain coherent and
manageable.