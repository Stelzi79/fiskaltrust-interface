# fiskaltrust.interface
Examples how to use fiskaltrust.interface.
fiskaltrust offers a legal compliant cash register security mechanism.

## documentation
The detailed documentation is available from fiskaltrust-portal [https://portal.fiskaltrust.at] when after you activate the role possystem-creator (Registrierkassenhersteller).
To speed up development we also deliver a nuget-package [https://nuget.org] with the packageId fiskaltrust.interface.

## connecting to fiskaltrust.securitymechanism
As a base technology in communication wcf is used. For local internal communication between queues, signature creation units and custom modules the net.pipe protocoll is the best choice. For multi platform communication the basic http may be the best choice.
### SOAP
SOAP comes with the http protocol from wcf communication. To get the wsdl file you can use these debug-build and goto the http-address configured, here [http://localhost:8524/438BE08C-1D87-440D-A4F0-A21A337C5202] is used. Another option is to use the file from the folder tools/wsdl.
### REST
REST is available in both, in xml and json. there are helpers which can be loaded to keep the base service lightweight.
### native TCP-IP and serial rs232/485/422
Native stream based communication with a defined protocoll format is provided by helpers.
### user specific
With the helper topology it is possible to solve every scenario

## hosting on linux and mac os
For usage on linux and mac os we use mono to run fiskaltrust on it.
For production use it is possible to run it as a daemon.
For test and development, the command-line parameter -test can be used. 
(You can find details in the developer documentation.)
Prerequisites beside mono-complete 3.x / 4.x are also sqlite and pcsclite if you want to use an usb-based signature creation unit.
Typical commands to run:
sudo apt-get update
sudo apt-get install mono-complete
sudo apt-get install sqlite
sudo apt-get install pcsclite
cd fiskaltrust-mono
sudo mono fiskaltrust.mono.exe -caschboxid= -useoffline=true -test

## hosting on windows
The launcher (fiskaltrust.exe) is constructed to act as an windows service in production environment. For this also automated installation is supported by command-line parameter. ((You can find details in the developer documentation.)
For test and develop the command-line parameter -test can be used to run the service.
Only .net4 is prerequisites.
Typical commands to run: (open command-line with administration permission)
cd fiskaltrust-net40
fiskaltrust -cashboxid= -test

## cloud based
The same interface and service definition is served as an cloud service. You can use SOAP and REST interface designed for local service to seamless switch over to the cloud service.

## test related informations
The launcher uses the file configuration.json from its execution directory to make up its basic configuration. In production use this is done in the fiskaltrust-portal and the launcher tries to read it from the upload-server, related to cashboxid and accesstoken. For offline use this configuration is stored in the execution directory. Once the configuration is readed from the execution directory or from upload-server, it is stored localy in the service-folder. The default service-foler ist in windows %ProgramData%\fiskaltrust or in linux /usr/shared/fiskaltrust. In this folder also the database file and the executeables are stored, to completly reset the service delete tihs directory.

## feedback and bugs
fiskaltrust is under permanent development, so feel free to discuss here your wishes and our bugs with the github-issues feature.

# fiscaltrust consulting gmbh
Bauernmarkt 24, 1010 Wien
[info@fiskaltrust.at]
[www.fiskaltrust.at]
