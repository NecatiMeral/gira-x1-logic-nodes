# Doorbird X1 Listener

Dieser Dienst dient als Vermittlungs- und Umsetzungsstelle für [Doorbird](https://manual.doorbird.com/app/de/) HTTP(S)-Anfragen, um Doorbird Türklingeln mit der [IOT-API](https://partner.gira.de/data3/Gira_IoT_REST_API_v2_DE.pdf) des [Gira X1](https://partner.gira.de/systeme/knx-system/knx-produkte/server/x1/features.html) zu integrieren.

## Inspiration

Die Türklingel kann mit einem Zeitplan für u.a. HTTP(S)-Anfragen konfiguriert werden. Hierbei unterstützt die Doorbird-Anlage jedoch nur `GET`-Anfragen ohne anpassbaren Anfrageinhalt. Um eine Signalisierung der Türklingel im X1 zu realisieren, muss die Aktion möglichst zeitnah an den X1 gemeldet werden können.

Um die Anfrage der Doorbird im X1 zu erkennen gibt es bereits einige Lösungen. Bisher habe ich den Logiklbaustein [`WebRequest empfangen`](https://www.smarthome-appstore.de/produkt/logikbausteine-netzwerk/) der Firma Splendid Minds genutzt. Leider kann dieser WebRequest nur min. alle fünf Sekunden ausgewertet werden. Diese Zeitspanne reicht unter Umständen schon, dass der Besucher wieder verschwindet.

## Beschreibung der Systeme

### Doorbird - HTTP(S)-Anfragen

Die Doorbird kann bei bestimmten Aktionen eine HTTP(S)-Anfrage an eine definierte Adresse zu senden. Hierbei kann jedoch nur eine `GET`-Anfrage ohne Inhalt abgesetzt werden.

### Gira X1 - IOT API

Über die Gira X1 IOT API lassen sich alle Elemente der Visualisierung (App) abfragen und auch Werte ändern. Auf diese Änderung der Werte kann man innerhalb von Logikbausteinen umgehend reagieren und eine entsprechende Signalisierung realisieren.

Um einen einfachen binären Wert über die API zu ändern, muss folgende Anfrage an den X1 erfolgen:

```
PUT https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}

{ "value": "1" }
```

Hierbei gibt es folgende Variablen:
| Variable | Beschreibung
|-----------|----------------
| `HostOrIp` | Der DNS-Hostname oder die IP-Adresse des X1
| `DataPointUid` | Die interne Datenpunkt-ID des Tasters für die Türklingel in der Visualisierung
| `ClientToken` | Das Authentifizierungstoken für den Client (die registrierte Anwendung)

## Funktionsweise

Der Doorbird X1 Listener empfängt Anfragen der Doordbird-Anlage. Diese Anfrage wird abhängig von dem bereitgestellten `INTEGRATION-KEY` (Standard: `Default`) an einen konfigurierten Gira X1 weitergeleitet. Hierbei wird eine gemäß der IOT-API erforderliche Anfrage erzeugt.

Der Trigger kann mit folgender URL aktiviert werden:

```
http://{listener-ip}/trigger/{key}/{client-token}/{datapoint-uid}?target={gira-x1-ip}
```

| Parameter | Beschreibung
|-------------|--------------------
| `listener-ip` | Die IP-Adresse (inkl. Port) des Doorbird X1 Listeners.
| `key` | Der Schlüssel der zu nutzenden Konfiguration. In den meisten Fällen reicht hier `Default`.
| `client-token` | Der Client-Token des im X1 registrierten Clients.
| `datapoint-uid` | Die ID des Tasters (`Taster (Drücken/Loslassen)`) in der Visualisierung.
| `target` | __(optional)__ Die IP des Gira X1. Muss gesetzt werden, wenn die erweiterte Konfiguration nicht genutzt wird.

## Erweiterte Konfiguration

Die Konfiguration des Listeners bietet folgendes Schema. Hierbei muss in den meisten Fällen jedoch nur `HostOrIp` gesetzt werden.

```json
{
  "Doorbirds": {
    "Default": {
      "HostOrIp": "192.168.0.5",
      "UrlTemplate": "https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}",
      "Method": "PUT",
      "Payload": "{ \"value\": \"1\" }"
    },
    "My-Second-Gira-X1": {
      "HostOrIp": "192.168.0.6",
      "UrlTemplate": "https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}",
      "Method": "PUT",
      "Payload": "{ \"value\": \"1\" }"
    }
  }
}
```

Je Integration können hier folgende Werte konfiguriert werden. Die erweiterte Konfiguration ist optional.

| Parameter | Standardwert | Beschreibung
|-----------|---------------|-----------------
| `HostOrIp` | _(leer)_ | Die IP des Gira X1. Wenn nicht konfiguriert, muss an den Aufruf aus der Doorbird der Parameter `?target={gira-x1-ip}` bereitgestellt werden.
| `UrlTemplate` | `https://${HostOrIp}/api/v2/values/${DataPointUid}?token=${ClientToken}` | Gibt die zu nutzende URL, mit entsprechenden Platzhaltern, für den Aufruf an den Gira X1 vor.
| `Method` | `PUT` | Gibt den Typen der Anfrage an die Gira X1 IOT API an.
| `Payload` | `{ "value": "1" }` | Gibt den an den Gira X1 zu sendenden Anfrageinhalt an. Der Standardwert aktiviert den konfigurierten Taster.
