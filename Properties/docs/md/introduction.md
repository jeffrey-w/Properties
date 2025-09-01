# Introduction

The Properties library provides a facility for dynamically attaching new
attributes to objects. Attributes are included on and excluded from instances
in an immutable way. That is, specifying a new attribute to attach to an object
always results in a new copy of that object. The attributes defined on an object
may be queried using standard syntax.

## Motivation

This library was first conceived as a means for balancing the benefits of
static, strongly-typed languages, like C#, and dynamic ones, like JavaScript.
Specifically, it grew from a need to define objects that exhibited a core set
of common attributes, but that also exhibited others on an individual basis. The
primary end is to avoid interfaces for which any object exhibits a small subset
of the properties declared on it.

In addition, immutability is an important feature of the classes exposed by the
library, and this extends to the values assignable to attributes. As such,the
types of associations that may be made are limited. The authors believe that the
increased facility for making sound inferences over object state more than
offsets the somewhat reduced capacity for expression.