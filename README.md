# fiskaltrust.interface
examples how to use fiskaltrust.interface.
fiskaltrust offers a legal compliant cash register security mechanism.

## documentation
the detailed documentation is available from fiskaltrust-portal [https://portal.fiskaltrust.at] when after you activate the role possystem-creator (Registrierkassenhersteller).
to speed up development we also deliver a nuget-package [https://nuget.org] with the packageId fiskaltrust.interface.

## connecting to fiskaltrust.securitymechanism
as a base technologie in communication wcf is used. for local internal communication between queues, signature creation units and custom modules the net.pipe protocoll is the best choice. for multi plattform communication the basic http may be the best choice. 
### SOAP
soap comes with the http protocoll from wcf communication. to get the wsdl file you can use these debug-build and goto the http-address configured, here [http://localhost:8524/438BE08C-1D87-440D-A4F0-A21A337C5202] is used. another option is to use the file from the folder tools/wsdl.
### REST
rest is available in both, in xml and json. there are helpers with can be loaded to keep the base service lightwight. 
### native TCP-IP and serial rs232/485/422
native stream based communication with a defined protocoll format is provided by helpers.
### user specific
with the helper topologie it is possible to solve every scenario

## hosting on linux and mac os
for usage on linux and mac os we use mono to run fiskaltrust on it.
for production use it is possible to run it as a daemon.
for test and development the command-line parameter -test can be used.
prerequest ist beside mono-complete also sqlite and pcsclite if you want to use an usb-based signature creation unit.

## hosting on windows


## cloud based
the same interface and servie definition is used as