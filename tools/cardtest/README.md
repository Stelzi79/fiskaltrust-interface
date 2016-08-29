# fiskaltrust.cardtest  
  
start with RunMe.Bat  

| Parameter | Parameter | description|  
| ------------- |:-------------:|:-----|  
|1 | readers | get a list of all readers |  
|2 | atrustapdu | runns test on atrustapdu with the following options |  
| | | [-i -init] {[key:value]} initialize and show certificate information |  
| | | [-s -sign] {[key:value]} initialize and sign |  
| | |  [-(x)] {[key:value]} initialize and sign x-times for performance test |  
| 3 | | atrustonline | runns test on atrustapdu with the following options |  
| | | [-i -init] {[key:value]} initialize and show certificate information |  
| | |  [-s -sign] {[key:value]} initialize and sign |  
| | | [-(x)] {[key:value]} initialize and sign x-times for performance test |  
| 99 | exit| terminate programm |  
  
##usage  
| Parameter | description|  
|:-------------|:-----|  
| 1 <Enter> | get a list of readers and cards  |  
|2 -i <Enter> | initialize and show certificate information of local connected cards  |  
|atrustapdu -i <Enter> | initialize and show certificate information of local connected cards |  
| 3 -5 P9yP+kagjnwIh/eXxEbsNPxsssFQ3ZMEU4XsIR5B0LA= <Enter> | initialize and sign 5-times for performance test online using a specific key  |  