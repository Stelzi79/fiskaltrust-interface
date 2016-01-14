xcopy ..\csclasslibrarycominterop\bin\x64\debug\*.dll /y
..\..\tools\regasm\regasm.exe csClassLibraryCOMInterop.dll /codebase /tlb:fiskaltrustCOMInterop.tlb
timeout /t 10

