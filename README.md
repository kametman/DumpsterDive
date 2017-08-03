# DumpsterDive
simple application to generate garbage files in a variety of sizes.

## Overview
DumpsterDive is a .Net console application that will generate a randomized ASCII file.  A menu within the application is used to control the parameters of the file generated:

* File size (in either KB or MB)
* Characters used
  * Upper case letters
  * Lower case letters
  * Numbers
  * Symbols

### Command Line Arguments
DumpsterDive also allows for command line parameters to generate the file.  Any parameters not passed via the command line will cause a prompt to appear in console window.
* -t: Determines whether file generated is kilobytes or megabytes
* -s: Determines size of file.
* -c: Determines character sets used:
  * A - upper case letters
  * a - lower case letters
  * 1 - numbers
  * ! - symbols
* -n: Will cause the console window to close automatically after file is generated.

Example "-tk -s100 -sAa" will generate a 100 kilobyte file filled with upper and lowercase letters.

