# Functional Programming in C#

### Run the decks
NOTE: you can use `yarn` or `npm` commands interchangeably but do yourself a favor and install yarn globally `npm i yarn -g` and save some time!

- clone this repo
- `cd deck`
- `yarn install`
- Start deck for Part 1 `yarn run part1`
- Start deck for Part 2 `yarn run part2`

### Export the decks as PDF
- Export deck for Part 1 `yarn run pdf deck1.pdf`
- Export deck for Part 2 `yarn run pdf deck2.pdf`

### Examples

To run each example, move into its respective folder and run `dotnet run`.

#### Part 1
- `examples/Code/FPinCSharp.Example1`
  - Employee directory application written in an imperative way
- `examples/Code/FPinCSharp.Example2`
  - Fixing Employee directory bugs by avoiding mutation
- `examples/Code/FPinCSharp.Example3`
  - Fixing Employee directory bugs caused due to having partial functions and unexpected nulls by utilizing `Option`
  
#### Part 2
- `examples/Code/FPinCSharp.Example4`
  - Leverage `Either` effect to encode application errors in a functional way
- `examples/Code/FPinCSharp.LINQSyntax`
  - Introduction to C# LINQ syntax which is a convenient syntax for chaining effectful functions
- `examples/Code/FPinCSharp.Example5`
  - Social media profile generation by composing sequential Tasks using a Monadic approach
- `examples/Code/FPinCSharp.Example6`
  - Social media profile generation by composing parallel Tasks using an Applicative approach
- `examples/Code/FPinCSharp.Traversables`
  - Working with lists of elevated values and using `Sequence` and `Traverse` functions
- `examples/Code/FPinCSharp.Example7`
  - Generating many Social media profiles in parallel leveraging `Sequence` from Traversables, the whole request fails if any fail
- `examples/Code/FPinCSharp.Example8`
  - Generating many Social media profiles in parallel and ignoring the failed requests and collected the successfully results only
  

### What to expect in Part 3 

- More Advanced, real world examples of working with `Try`, `TryAsync`, `Traversables`
  - `examples/Code/FPinCSharp.Traversables`
  - `examples/Code/FPinCSharp.Example8`

- Pattern Matching 
  - Pattern matching in C#
  - Functional ways of application development using Pattern Matching (creating your own DSL for making a small metrics service)
    - `examples/LINQPad/pattern-matching-vs-polymorphism`
  - Limitations of Pattern Matching and Problems of using PM as a replacement to Polymorphism in C#

- Ad-Hoc Polymorphism and C#
  - Types of polymorphism
    - `examples/LINQPad/subtype-polymorhism.linq`
  - Ad-Hoc Polymorphism
    - `examples/LINQPad/ad-hoc-polymorphism.linq`
    - `examples/LINQPad/ad-hoc-polymorhism-using-extention-methods-in-csharp.linq`
  - Extension methods as the closest thing in C# for achieving Ad-Hoc Polymorphism
  - Higher Kinded Types

- Lenses and ways of working with nested complex immutable types
  - `examples/Code/FPinCSharp.Lenses`
  - `examples/Code/FPinCSharp.ImmutableTypes`

- FP things to avoid in C#
  - Pattern Matching as a replacement for Polymorphism in C#
  - Currying
  - Recursion
    - How to write stack safe tail recursive functions using trampolining technique (fixing our not so stack safe Loop function in Examples 1-4)