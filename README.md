# AbstractIO using nanoFramework
A nanoFramework library for abstracting I/O to simple interfaces getting or accepting values of a certain data type, and for re-usable features above them.

To see simple examples of AbstractIO code, have a look at [source/AbstractIO.Samples](source/AbstractIO.Samples).
To see the power of AbstractIO "in action", please have a look at [source/AbstractIO.Netduino3.Samples/Netduino3SamplesMain.cs](source/AbstractIO.Netduino3.Samples/Netduino3SamplesMain.cs).

Status as of 2018-04-13: All samples except Sample07WaitForButtonEventBased are successfully tested. Sample07WaitForButtonEventBased runs fine outside of the debugger, but the debugger cannot attach to the device after deploying the program and rebooting the board.