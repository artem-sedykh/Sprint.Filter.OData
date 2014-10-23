Sprint.Filter.OData
===================
[Download from NuGet](http://nuget.org/packages/Sprint.Filter.OData).

## Introduction

coming soon...

## Serialize

```csharp
Filter.Serialize<Customer>(t => t.Customers.Select(x => x.Id).Any())
//result: Customers/Select(x: x/Id)/Any()
```
## Deserialize

```csharp
Filter.Deserialize<Customer>("Numbers/Max(x: x) eq 15")
//result: t => t.Numbers.Max(x => x) == 15
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