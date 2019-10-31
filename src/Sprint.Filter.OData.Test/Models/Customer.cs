using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Sprint.Filter.OData.Test.Models
{
    [Flags]
    public enum EnumData
    {
        TestData1=2,
        TestData2=4,
        TestData3=8
    }

    [Flags]
    public enum EnumDataLong:long
    {
        TestData1 = 2,
        TestData2 = 4,
        TestData3 = 8
    }

    [Flags]
    public enum EnumDataShort : short
    {
        TestData1 = 2,
        TestData2 = 4,
        TestData3 = 8
    }

    public enum Status
    {
        Pending = 0,

        Failed = 1,

        Completed = 2
    }

    public class TestClass
    {
        public int Id { get; set; }

        public IQueryable<Customer>Customers { get; set; } 

        public Status Status { get; set; }

        public Status? NullableStatus { get; set; }
    }

    public class Customer
    {
        public int CategoryID { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal Price { get; set; }
        public EnumData EnumData { get; set; }
        public EnumData? NullableEnumData { get; set; }

        public EnumDataShort EnumDataShort { get; set; }

        public EnumDataLong EnumDataLong { get; set; }
        public int Id { get; set; }

        public int? NullableInt { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public Customer Parent { get; set; }

        public IQueryable<TestClass> Items { get; set; }

        public IQueryable<Customer> Customers { get; set; }

        public Customer[] CustomersArray { get; set; }

        public int?[] IntArray { get; set; }

        public IEnumerable<Customer> EnumerableCustomers { get; set; }

        public List<Customer> ListCustomers { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime? NullableBirthDate { get; set; }

        public double FreightDouble { get; set; }

        public decimal FreightDecimal { get; set; }

        public double? FreightDoubleNullable { get; set; }

        public decimal? FreightDecimalNullable { get; set; }

        public decimal Salary { get; set; }

        public int Rating { get; set; }

        public bool Boolean1 { get; set; }

        public bool Boolean2 { get; set; }

        public sbyte Sbyte { get; set; }

        public sbyte? NullableSbyte { get; set; }

        public char Char { get; set; }

        public char? NullableChar { get; set; }

        public IEnumerable<int> Numbers { get; set; }

        [XmlElement(ElementName = "cn1")]
        public int CustomName1 { get; set; }

        [XmlAttributeAttribute(AttributeName = "cn2")]
        public int CustomName2 { get; set; }

        [DataMember(Name = "cn3")]
        public int CustomName3 { get; set; }
    }
}
