xcopy ..\csclasslibrarycominterop\bin\x64\debug\*.dll /y
..\..\tools\regasm\x64\regasm.exe csClassLibraryCOMInterop.dll /codebase /tlb:fiskaltrustCOMInterop.tlb
timeout /t 10

