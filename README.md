# gira-x1-logic-nodes

Custom logic nodes for personal use and other useful stuff to use with the GIRA X1.

## Documentation

GIRA's documentation of the logic node SDK can be found on the [Gira Logic nodes page](https://partner.gira.com/en/service/software-tools/developer.html).

## Logic nodes

### Necati_Meral_Yahoo_De.Logic.Ads

* Contains logic nodes to interact with [Beckhoff TwinCAT](https://www.beckhoff.com/de-de/produkte/automation/twincat/txxxxx-twincat-2-base/tabellarische-produktuebersicht/) systems by utilizing the [ADS (Automation Device Specification)](https://infosys.beckhoff.com/index.php?content=../content/1031/tcba/12269581963.html&id=8978321744740978019) protocol.
* [ADS: Read value (german)](https://necatimeral.github.io/gira-x1-logic-nodes/dotnet/src/NecatiMeral.Logic.Ads/help/ReadAdsDataNode.html)
* [ADS: Write value (german)](https://necatimeral.github.io/gira-x1-logic-nodes/dotnet/src/NecatiMeral.Logic.Ads/help/WriteAdsDataNode.html)

### Necati_Meral_Yahoo_De.Logic.ComfortOnline

* Contains a logic module to retrieve data from the Comfort online platform of the heating manufacturer KWB.
* [ComfortOnline input (german)](https://necatimeral.github.io/gira-x1-logic-nodes/dotnet/src/NecatiMeral.Logic.ComfortOnline/help/ComfortOnlineRequestNode.html)

### Necati_Meral_Yahoo_De.Logic.Common

* Contains general logic nodes
* [Key value selection (german)](https://necatimeral.github.io/gira-x1-logic-nodes/dotnet/src/NecatiMeral.Logic.Common/help/KeyValueNode.html)


## Standalone tools

### doorbird-x1-listener

Docker service which acts as a proxy to integrate a doorbird doorbell into a gira-x1 server.

* [Technical description (german)](./docs/de/Doorbird-X1-Listener.md)
* [Setup guide (german)](./docs/de/Doorbird-X1-Listener-Setup.md)

## Scripts

Contains scripts for minor automations of our home.
