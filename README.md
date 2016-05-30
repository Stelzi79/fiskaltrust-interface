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
rest is available in both, in xml and json. there are helpers which can be loaded to keep the base service lightwight. 
### native TCP-IP and serial rs232/485/422
native stream based communication with a defined protocoll format is provided by helpers.
### user specific
with the helper topologie it is possible to solve every scenario

## hosting on linux and mac os
for usage on linux and mac os we use mono to run fiskaltrust on it.
for production use it is possible to run it as a daemon.
for test and development the command-line parameter -test can be used. see developer documentation for details.
prerequest ist beside mono-complete 3.x / 4.x also sqlite and pcsclite if you want to use an usb-based signature creation unit.
typical commands to run:
sudo apt-get update
sudo apt-get install mono-complete
sudo apt-get install sqlite
sudo apt-get install pcsclite
cd fiskaltrust-mono
sudo mono fiskaltrust.mono.exe -caschboxid= -useoffline=true -test

## hosting on windows
the launcher (fiskaltrust.exe) is constructed to act as an windows service in production environment. for this also automated installation is suported by command-line parameter, for details take a look at the developer documentation.
for test and develop the command-line parameter -test can be used to run the service.
prerequest ist .net4 only
typical commands to run: open command-line with administration permission
cd fiskaltrust-net40
fiskaltrust -cashboxid= -test

## cloud based
the same interface and servie definition is served as an cloud service. you can use SOAP and REST interface designed for local service to seamless switch over to the cloud service.

## test related informations
the launcher uses the file configuration.json from its execution directory to make up its basic configuration. in production use this is done in the fiskaltrust-portal and the launcher tries to read it from the upload-server, related to cashboxid and accesstoken. for offline use this configuration is stored in the execution directory. once the configuration is readed from the execution directory or from upload-server, it is stored localy in the service-folder. the default service-foler ist in windows %ProgramData%\fiskaltrust or in linux /usr/shared/fiskaltrust. in this folder also the database file and the executeables are stored, to completly reset the service delete tihs directory.

## feedback and bugs
fiskaltrust is under permanent development, so feel free to discuss her your wishes and our bugs with the github-issure feature.

# fiscaltrust consulting gmbh
Bauernmarkt 24, 1010 Wien
info@fiskaltrust.at
www.fiskaltrust.at 
