# BIgNumbers
# IdleNumber: Documentation

IdleNumber - this is a class that allows you to use simple arithmetic with numbers much larger than standard types can offer (the maximum integer in C# is unsigned long.Max = 18,446,744,073,709,551,615).

With instances of the class, you can perform the simplest arithmetic operations, like addition, subtraction and multiplication, as well as apply all comparison operations:
```
IdleNumber first = new IdleNumber();
IdleNumber second = new IdleNumber();

var sum = first + second;
var sub = first - second;
var mul = first * 8;

if(first != second || first == second ||
first > second || first < second || 
first >= second || first <= second)
{
    Console.Write("It's Working!");
}
```

## Suffix

In standard form the class has the following set of suffixes:
```
public enum Suffix
{
    None, Thousand, Million, Billion, Trillion,
    Quadrillion, Quintillion, Sextillion, 
    Septillion, Octillion, Nonillion, Decillion,
    Undecillion, Duodecillion, Tredecillion
}
```
You can change them at your own choice, for example, copy all the letters of the alphabet.

The number of suffixes in the enumeration directly affects the maximum allowable value of IdleNumber. For example, if there are only three values in the enumeration, then the maximum value of IdleNumber will be 999,999,999.
> The more suffixes there are in the enumeration, the more stable arithmetic operations will work.

## Constructors

The IdleNumber class provides several constructors for initializing instances of the class:
- *IdleNumber()* - empty constructor, creates an instance with the value 0.
- *IdleNumber(long initValue)* - creates an instance equal in value to the submitted number.
- *IdleNumber(string initString)* - creates an instance equal in value to the number represented by the string. Only strings without delimiters are correctly recognized, for example 123456789 or -897567312.
- *IdleNumber(int[] number)* - creates an instance using the raw representation of a number in Idle Number. ***Not recommended for use.*** But if you still decide, then here is the instruction: take the number 3,432,877,842,389. To submit it to the constructor, you need to create an array like this: {389, 842, 877, 432, 3}.

## Properties

The properties in the class are divided into static and non-static. Let's start by considering static properties:
- *MaxValue* - returns an instance of IdleNumber that has the maximum value allowed by the class.
- *MinValue* - returns an instance of IdleNumber that has the minimum value allowed by the class.
- *Divider* - a property that allows you to set the separator used for string output of a number. Returns nothing.
- *ShowSuffix* - the property that accepts true or false determines whether the suffix will be printed when the number is output.

The following construction is used to access the properties:
```
var maxValue = IdleNumber.MaxValue;
var minValue = IdleNumber.MinValue;
IdleNumber.Divider = ",";
IdleNumber.ShowSuffix = false;
```

Now consider the non-static properties of the class:
- *SmallText* - outputs a number in a truncated form, for example, there is a number 24,324,635,001 Billion. The number 24,324 Billion will appear on the screen.
- *FullText* - outputs a number in its full form, for example, there is a number 24,324,635,001 Billion. It will also be displayed on the screen.
- *Negative* - determines whether a number is negative, can both return the current value and get a new one, so a positive number can be turned into a negative and vice versa without using multiplication.

> The output of the *SmallText* and *FullText* properties is directly affected by the use of the static *Divider* and *ShowSuffix properties*.

## Public Methods
In the Idle Number class, instances of the class have several public methods:

- *GetNumArray()* - returns an integer array, which is a raw representation of IdleNumber.
- *Assign(IdleNumber other)* - since IdleNumber is a class, the assignment of a value occurs by reference. This means that by assigning one instance the value of another via =, you will have instances repeating each other's behavior (if one is turned into a negative, the same will happen to the other). This method allows you to copy the contents of one instance to another, which will allow you to have your own behavior.
- *IsEqual(IdleNumber other)* - checks whether the value of the submitted number and the current one are equal, the equivalent of comparison ==.
- *IsGreaterThen(IdleNumber other)* - equivalent of comparison >.
- *ConvertToLong()* - returns an integer value of type long, if possible.

## Public static methods

- *ConvertFromLong(long val)* - returns an instance of the Idle Number class equal to val. Equivalent of a constructor taking long as a parameter.
- *Abs(IdleNumber other)* - returns the modulus of the received number.
