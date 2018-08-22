Sprint.Filter.OData  [![NuGet](https://img.shields.io/nuget/v/Sprint.Filter.OData.svg)](https://www.nuget.org/packages/Sprint.Filter.OData/) [![Downloads](https://img.shields.io/nuget/dt/Sprint.Filter.OData.svg)](https://www.nuget.org/packages/Sprint.Filter.OData/)
===================
# Status
[![Build status](https://ci.appveyor.com/api/projects/status/210ubfc6jn3g5hk0/branch/release?svg=true)](https://ci.appveyor.com/project/artem-sedykh/sprint-filter-odata/branch/release)

## Introduction

coming soon...

[Operators and functions](http://msdn.microsoft.com/en-us/library/hh169248(v=nav.71).aspx)
## Serialize

```csharp
Filter.Serialize<Customer>(t => t.Customers.Select(x => x.Id).Any())
//result: Customers/Select(x: x/Id)/Any()

```
## Deserialize

```csharp
Filter.Deserialize<Customer>("Numbers/Max(x: x) eq 15")
//result: t => t.Numbers.Max(x => x) == 15

//Support Enumerable,Queryable  Methods:

Filter.Deserialize<Customer>("IntArray/Average(x: x) eq 15")
//result: t => t.IntArray.Average(x => x) == 15

Filter.Deserialize<Customer>("Customers/GroupBy(x: x/Id)/Count() eq 15")
//result: t => t.Customers.GroupBy(x => x.Id).Count() == 15

Filter.Deserialize<Customer>("IntArray/Contains(Id)")
//result: t => t.IntArray.Contains(t.Id)

Filter.Deserialize<Customer>("Customers/Select(x: x/Id)/Distinct()/Count() eq 15")
//result: t => t.Customers.Select(x => x.Id).Distinct().Count() == 15

```
##User functions

```csharp
//Entity Framework StringConvert methods
var methods = typeof(SqlFunctions)
              .GetMethods()
              .Where(x => x.Name == "StringConvert").ToArray();

//Register user functions
MethodProvider.RegisterFunction("StringConvert", methods);

Filter.Deserialize<Customer>("StringConvert(Price) eq '1'")
//result: t => SqlFunctions.StringConvert(t.Price) == "1"
 
Filter.Serialize<Customer>(t => SqlFunctions.StringConvert(t.Price) == "1")
//result: StringConvert(Price) eq '1'
```

## Attributes

```csharp
//Support Attributes:
[XmlElement(ElementName = "...")]
[XmlAttributeAttribute(AttributeName = "...")]
[DataMember(Name = "...")]
 
Filter.Serialize<Customer>(t => t.CustomName1 == 1 || t.CustomName2 == 2 || t.CustomName3 == 3)
//result: cn1 eq 1 or cn2 eq 2 or cn3 eq 3

Filter.Deserialize<Customer>("cn1 eq 1 or cn2 eq 2 or cn3 eq 3")
//result: t => t.CustomName1 == 1 || t.CustomName2 == 2 || t.CustomName3 == 3
```
##MvcAttribute in Sprint.Filter.OData.Mvc

```csharp
public ActionResult List([ODataFilterBinder]Expression<Func<Customer,bool>> predicate)
{
    ....
}
```

## Bugs

Bugs should be reported through github at
[https://github.com/artem-sedykh/Sprint.Filter.OData/issues/](https://github.com/artem-sedykh/Sprint.Filter.OData/issues/).
