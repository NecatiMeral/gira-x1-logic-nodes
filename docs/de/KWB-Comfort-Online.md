# KWB Comfort Online

## Kesselstatus

Der Kesselstatus wird in der UI in textueller Form dargestellt. Schaltet man das Diagramm dazu, wird der Status Numerisch als Liniendiagramm dargestellt. Nachfolgend eine Legende, um die Daten z.B. in eine Influx-DB zu speichern.

| ID | Status EN | Status DE |
|----|-----------------|----------------
| 30 | Ready (-Requ) | Bereit (-Anf)
| 31 | Ready (+Wait time) | Bereit (+Wartezeit)
| 32 | Ready (+Requ) | Bereit (+Anf)
| 11 | Ignite start suction | Zünden Start Saugzug
| 13 | Ignition feeding 1 | Zünden Einschieben 1
| 15 | Ignition heating | Zünden Heizen
| 19 | Complete ignition | Durchzünden
|  4 | operation | Betrieb
|  5 | Afterrun | Nachlauf

## Pufferstatus

Der Pufferstatus wird in der UI in textueller Form dargestellt. Schaltet man das Diagramm dazu, wird der Status Numerisch als Liniendiagramm dargestellt. Nachfolgend eine Legende, um die Daten z.B. in eine Influx-DB zu speichern.

| ID | Status EN | Status DE |
|----|-----------------|----------------
| 12 | Temperature sufficient | Temperatur ausreichend
| 5 | Charging | Laden
