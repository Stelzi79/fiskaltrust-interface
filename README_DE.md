# fiskaltrust.Interface
Beispiel, wie man das fiskaltrust.Interface nutzt.
fiskaltrust bietet eine gesetzeskonforme Sicherheitseinrichtung für Registrierkassen.

## documentation
Die detaillierte Dokumentation ist auf dem fiskaltrust.Portal [https://portal.fiskaltrust.at] nach der Registrierung und Aktivierung der Rolle als Registrierkassenhersteller verfügbar.

Um Ihren Entwicklung zu erleichtern, stellen wir auch ein nuget-package [https://nuget.org] mit der packageId fiskaltrust.interface zur Verfügung.

## Verbindung mit der fiskaltrust.securitymechanism (Sicherheitseinrichtung)
Als Basis-Technologie zur Kommunikation wird WCF verwendet. Zur lokalen, internen Kommunikation zwischen queues, signature creation units (Signaturerstellungseinheiten) und benutzerspezifischen Modulen (Sonstigen Modulen) wird am besten das net.pipe protocoll verwendet. Zur Kommunikation zwischen verschiedenen Plattformen wird am besten basic http verwendet.
### SOAP
SOAP wird mit dem http-Protokoll der wcf-Kommunikation ausgeliefert. Um die WSDL-Datei zu erhalten, kann man diesen Debug-Build verwenden und auf die konfigurierte http-Adresse gehen. Hierbei wird [http: // localhost: 8524 / 438BE08C-1D87-440D-A4F0-A21A337C5202] verwendet. Eine weitere Option besteht darin, die Datei aus dem Ordner tools/wsdl zu verwenden.
### REST
REST steht sowohl in XML als auch in JSON zur Verfügung. Es stehen benutzerspezifische Module zur Verfügung, die geladen werden können, um die Basis-Services möglichest schlank zu halten.
### native TCP-IP und Serielle RS232/485/422
Nativen Stream-basierte Kommunikation mit einem definierten Protokoll-Format wird als benutzerspezifische Module zur Verfügung gestellt.
### Benutzerspezifisch
Mit der Topologie der benutzerspezifischen Module ist es möglich, jedes technische Szenario zu lösen.

## Hosting auf Linux und Mac OS
Um fiskaltrust auf Linux und Mac OS laufen zu lassen, wird mono verwenden.
Für den produktiven Einsatz ist es möglich, ihn mit daemon zu betreiben.
Für Test und Entwicklung kann der command-line Parameter -test verwendet werden. Details sind in der Entwickler-Dokumentation zu finden.
Neben Mono-complete 3.x / 4.x ist auch SQLite und pcsclite Voraussetzung, wenn eine USB-basierte Signaturerstellungseinheit verwendet werden soll.
Typisch ausführbare Befehle:
sudo apt-get update
sudo apt-get install mono-complete
sudo apt-get install sqlite
sudo apt-get install pcsclite
cd fiskaltrust-mono
sudo mono fiskaltrust.mono.exe -caschboxid= -useoffline=true -test

## Hosting unter Windows
Der launcher (fiskaltrust.exe) ist als Windows-Dienst für die Produktionsumgebung entwickelt. Es wird auch die Möglichkeit einer automatisierte Installation durch Befehlszeilenparameter unterstützt. Details sind in der Entwickler-Dokumentation zu finden.
Für die Entwicklung und Prüfung kann der Befehlszeilenparameter -test verwendet werden um den Dienst auszuführen.
Hierfür ist nur .net4 Voraussetzung.
Typisch ausführbare Befehle:
(command-line mit Administrationsrecht starten)
cd fiskaltrust-net40
fiskaltrust -cashboxid= -test

## Cloud-basiert
Die gleiche Schnittstellen- und service-Definitionen werden als Cloud-Service unterstützt. Das als lokales Service entwickelte SOAP- und REST-Interface kann nahtlos in einen Cloud-Service gewechselt werden.

## Informationen zu Tests
Der Launcher verwendet die Datei configuration.json von seinem Ausführungsverzeichnis um die Basiskonfiguration aufzubauen. Im Produktionsbetrieb wird diese Konfiguration im fiskaltrust-Portal vorgenommen und der launcher versucht, diese vom Upload-Server auszulesen um die cashboxid und den accesstoken zu beziehen. Zur Offline-Nutzung wird diese Konfiguration im Ausführungsverzeichnis gespeichert. Sobald die Konfiguration aus dem Ausführungsverzeichnis oder von Upload-Server ausgelesen wird, wird es im lokalen Service-Ordner gespeichert. Der Standard-Service-Ordner ist unter Windows %Programdata%\fiskaltrust oder in Linux /usr/shared/fiskaltrust. In diesem Ordner werden auch die Datenbankdatei und die ausführbaren Dateien gespeichert. Um den Dienst vollständig zurückzusetzen, kann das komplette Verzeichnis gelöscht werden.

## Feedback und Bugs
fiskaltrust wird ständig weiterentwickelt. Nutzen Sie bitte die Möglichkeit, durch Github-Fragen Ihre Wünsche und unsere Fehler zu diskutieren.

# Fiscaltrust consulting gmbh
Bauernmarkt 24, 1010 Wien
[info@fiskaltrust.at]
[www.fiskaltrust.at]