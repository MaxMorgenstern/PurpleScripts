PurpleScripts
=============

*Repository with several scripts for [Unity].*

This classes have been created during an evaluation of unitys networking capabilities.

If you have any suggestions for improvement either let me know or fork this repository and create a pull request.



Content
----

**PurpleLog:**

PurpleLog is a Quake3-like console you can toggle by pressing ```^``` or ``` ` ```.
Every ```Debug.Log()``` entry will be displayed there. Warnings and exceptions will be highlighted.
You also have the option to trigger events by entering several self-defined commands. 


**PurpleNetwork / PurpleNetworkQueue:**

Networking class to communicate with a master server.
You can define functions that can be called by RPC without changing the class. Extending your functionallity is pretty easy this way.


**PurpleConfig:**

Configuration class that reads settings from one or multiple config files within the application folder. This way changing variables can be done without recompiling your code.


**PurpleDatabase:**

Class to connect and talk to to a MySQL server.
*In a client-server application this should only be done on the server!*


**PurpleCountdown:**

Trigger your own functions with a predefined delay. Either with 'countdown' functionallity or without.


**PurpleStorage:**

Class to store/load data locally.


**Helper:**
 - **PurplePassword:** Script for password hasing and valindating by Taylor Hornby (modified)
 - **PurpleSerializer:** Object to XML serializer as well as deserializer
 - **PurpleMessages:** Class with several message objects for networking
 - **PurpleException:** Exception class
 - **PurpleVersion:** Version number creator in the format: *Major.Minor.State.Revision*



Version
----

0.3.46.1357 (at the time this readme was updated)



Technical information
-----------

This Repository has been tested with:

Unity 4.3.4 - Mac OS (until 2015-03-04)

Unity 5.0.0 - Mac OS (from 2015-03-04)

Unity 4.6.0 - Windows 8.1

Unity 5.0 - Windows 8.1

API Compatibility: .net 2.0



License
----

The MIT License (MIT)

Copyright (c) 2014-2015 http://github.com/MaxMorgenstern

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.



[Unity]:http://unity3d.com/
