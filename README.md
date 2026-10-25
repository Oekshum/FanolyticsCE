# Fanolytics
Here be weird math!
The 7 atoms in this mod obey the symmetries of the [Fano Plane](https://en.wikipedia.org/wiki/Fano_plane).

## Glyphs
### Glyph of Reflection
This glyph takes in 1 atom, uses a second atom (in the bowl) to find the third atom on that line of the Fano Plane, and outputs it.
Example: if you put Horton in the bowl, it will turn fano into Klein, and Coxeter into Knuth.
#### Full table of conversions:
(left column is input into hole, top row is on the bowl)

|Inputs |Carium|Coxeter|Fano|Horton|Klein|Knuth|Sanders|
|-|-|-|-|-|-|-|-|
|Carium |Carium|Klein|Sanders|Knuth|Coxeter|Horton|Fano|
|Coxeter|Klein|Coxeter|Knuth|Sanders|Carium|Fano|Horton|
|Fano   |Sanders|Knuth|Fano|Klein|Horton|Coxeter|Carium|
|Horton |Knuth|Sanders|Klein|Horton|Fano|Carium|Coxeter|
|Klein  |Coxeter|Carium|Horton|Fano|Klein|Sanders|Knuth|
|Knuth  |Horton|Fano|Coxeter|Carium|Sanders|Knuth|Klein|
|Sanders|fano|Horton|Carium|Coxeter|Knuth|Klein|Sanders|

### Triplex Catalyst
Placing this glyph next to a triplex will allow the triplex to fully bond 3 fanolytics at once, so long as they are on a line.
#### Full table of lines availible:

|1|2|3|
|-|-|-|
|Carium|Klein|Coxeter|
|Carium|Fano|Sanders|
|Carium|Horton|Knuth|
|Coxeter|Fano|Knuth|
|Coxeter|Horton|Sanders|
|Fano|Horton|Klein|
|Klein|Knuth|Sanders|
