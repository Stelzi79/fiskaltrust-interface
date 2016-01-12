xcopy ..\csclasslibrarycominterop\bin\debug\*.dll /y
..\..\tools\regasm\regasm.exe csClassLibraryCOMInterop.dll /codebase /tlb:fiskaltrustCOMInterop.tlb
timeout /t 10

