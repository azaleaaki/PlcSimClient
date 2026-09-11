<img width="76" height="150" alt="deepseek_mermaid_20260911_61ab84" src="https://github.com/user-attachments/assets/5addd2c3-68b3-44c1-a3ef-954039038e03" /># PlcSimClient — схема работы и инструкция

## Схема обмена данными

```
flowchart TD
    A["PlcSimClient<br/>(S7.Net, TCP-клиент)"]
    B["Loopback-адаптер"]
    C["NetToPLCSim<br/>(TCP-сервер + переводчик<br/>на внутренний протокол PLCSIM)"]
    D["S7-PLCSIM<br/>(эмулятор CPU,<br/>хранит DB в своей памяти)"]

    A -->|"TCP-пакет<br/>172.22.1.1:102"| B
    B --> C
    C --> D
```
![Uploading deepseek_<?xml version="1.0" encoding="UTF-8"?>
<svg xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns="http://www.w3.org/2000/svg" id="mermaid-svg-0" width="100%" class="flowchart" style="max-width: 100%;" viewBox="-13.8 -13.8 303.6 601.4124999999999" height="100%"><style>#mermaid-svg-0{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;fill:#ccc;}@keyframes edge-animation-frame{from{stroke-dashoffset:0;}}@keyframes dash{to{stroke-dashoffset:0;}}#mermaid-svg-0 .edge-animation-slow{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 50s linear infinite;stroke-linecap:round;}#mermaid-svg-0 .edge-animation-fast{stroke-dasharray:9,5!important;stroke-dashoffset:900;animation:dash 20s linear infinite;stroke-linecap:round;}#mermaid-svg-0 .error-icon{fill:#a44141;}#mermaid-svg-0 .error-text{fill:#ddd;stroke:#ddd;}#mermaid-svg-0 .edge-thickness-normal{stroke-width:1px;}#mermaid-svg-0 .edge-thickness-thick{stroke-width:3.5px;}#mermaid-svg-0 .edge-pattern-solid{stroke-dasharray:0;}#mermaid-svg-0 .edge-thickness-invisible{stroke-width:0;fill:none;}#mermaid-svg-0 .edge-pattern-dashed{stroke-dasharray:3;}#mermaid-svg-0 .edge-pattern-dotted{stroke-dasharray:2;}#mermaid-svg-0 .marker{fill:lightgrey;stroke:lightgrey;}#mermaid-svg-0 .marker.cross{stroke:lightgrey;}#mermaid-svg-0 svg{font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:16px;}#mermaid-svg-0 p{margin:0;}#mermaid-svg-0 .label{font-family:"trebuchet ms",verdana,arial,sans-serif;color:#ccc;}#mermaid-svg-0 .cluster-label text{fill:#F9FFFE;}#mermaid-svg-0 .cluster-label span{color:#F9FFFE;}#mermaid-svg-0 .cluster-label span p{background-color:transparent;}#mermaid-svg-0 .label text,#mermaid-svg-0 span{fill:#ccc;color:#ccc;}#mermaid-svg-0 .node rect,#mermaid-svg-0 .node circle,#mermaid-svg-0 .node ellipse,#mermaid-svg-0 .node polygon,#mermaid-svg-0 .node path{fill:#1f2020;stroke:#ccc;stroke-width:1px;}#mermaid-svg-0 .rough-node .label text,#mermaid-svg-0 .node .label text,#mermaid-svg-0 .image-shape .label,#mermaid-svg-0 .icon-shape .label{text-anchor:middle;}#mermaid-svg-0 .node .katex path{fill:#000;stroke:#000;stroke-width:1px;}#mermaid-svg-0 .rough-node .label,#mermaid-svg-0 .node .label,#mermaid-svg-0 .image-shape .label,#mermaid-svg-0 .icon-shape .label{text-align:center;}#mermaid-svg-0 .node.clickable{cursor:pointer;}#mermaid-svg-0 .root .anchor path{fill:lightgrey!important;stroke-width:0;stroke:lightgrey;}#mermaid-svg-0 .arrowheadPath{fill:lightgrey;}#mermaid-svg-0 .edgePath .path{stroke:lightgrey;stroke-width:2.0px;}#mermaid-svg-0 .flowchart-link{stroke:lightgrey;fill:none;}#mermaid-svg-0 .edgeLabel{background-color:hsl(0, 0%, 34.4117647059%);text-align:center;}#mermaid-svg-0 .edgeLabel p{background-color:hsl(0, 0%, 34.4117647059%);}#mermaid-svg-0 .edgeLabel rect{opacity:0.5;background-color:hsl(0, 0%, 34.4117647059%);fill:hsl(0, 0%, 34.4117647059%);}#mermaid-svg-0 .labelBkg{background-color:rgba(87.75, 87.75, 87.75, 0.5);}#mermaid-svg-0 .cluster rect{fill:hsl(180, 1.5873015873%, 28.3529411765%);stroke:rgba(255, 255, 255, 0.25);stroke-width:1px;}#mermaid-svg-0 .cluster text{fill:#F9FFFE;}#mermaid-svg-0 .cluster span{color:#F9FFFE;}#mermaid-svg-0 div.mermaidTooltip{position:absolute;text-align:center;max-width:200px;padding:2px;font-family:"trebuchet ms",verdana,arial,sans-serif;font-size:12px;background:hsl(20, 1.5873015873%, 12.3529411765%);border:1px solid rgba(255, 255, 255, 0.25);border-radius:2px;pointer-events:none;z-index:100;}#mermaid-svg-0 .flowchartTitleText{text-anchor:middle;font-size:18px;fill:#ccc;}#mermaid-svg-0 rect.text{fill:none;stroke-width:0;}#mermaid-svg-0 .icon-shape,#mermaid-svg-0 .image-shape{background-color:hsl(0, 0%, 34.4117647059%);text-align:center;}#mermaid-svg-0 .icon-shape p,#mermaid-svg-0 .image-shape p{background-color:hsl(0, 0%, 34.4117647059%);padding:2px;}#mermaid-svg-0 .icon-shape rect,#mermaid-svg-0 .image-shape rect{opacity:0.5;background-color:hsl(0, 0%, 34.4117647059%);fill:hsl(0, 0%, 34.4117647059%);}#mermaid-svg-0 .label-icon{display:inline-block;height:1em;overflow:visible;vertical-align:-0.125em;}#mermaid-svg-0 .node .label-icon path{fill:currentColor;stroke:revert;stroke-width:revert;}#mermaid-svg-0 :root{--mermaid-font-family:"trebuchet ms",verdana,arial,sans-serif;}</style><g><marker id="mermaid-svg-0_flowchart-v2-pointEnd" class="marker flowchart-v2" viewBox="0 0 10 10" refX="5" refY="5" markerUnits="userSpaceOnUse" markerWidth="8" markerHeight="8" orient="auto"><path d="M 0 0 L 10 5 L 0 10 z" class="arrowMarkerPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"/></marker><marker id="mermaid-svg-0_flowchart-v2-pointStart" class="marker flowchart-v2" viewBox="0 0 10 10" refX="4.5" refY="5" markerUnits="userSpaceOnUse" markerWidth="8" markerHeight="8" orient="auto"><path d="M 0 5 L 10 10 L 10 0 z" class="arrowMarkerPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"/></marker><marker id="mermaid-svg-0_flowchart-v2-circleEnd" class="marker flowchart-v2" viewBox="0 0 10 10" refX="11" refY="5" markerUnits="userSpaceOnUse" markerWidth="11" markerHeight="11" orient="auto"><circle cx="5" cy="5" r="5" class="arrowMarkerPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"/></marker><marker id="mermaid-svg-0_flowchart-v2-circleStart" class="marker flowchart-v2" viewBox="0 0 10 10" refX="-1" refY="5" markerUnits="userSpaceOnUse" markerWidth="11" markerHeight="11" orient="auto"><circle cx="5" cy="5" r="5" class="arrowMarkerPath" style="stroke-width: 1; stroke-dasharray: 1, 0;"/></marker><marker id="mermaid-svg-0_flowchart-v2-crossEnd" class="marker cross flowchart-v2" viewBox="0 0 11 11" refX="12" refY="5.2" markerUnits="userSpaceOnUse" markerWidth="11" markerHeight="11" orient="auto"><path d="M 1,1 l 9,9 M 10,1 l -9,9" class="arrowMarkerPath" style="stroke-width: 2; stroke-dasharray: 1, 0;"/></marker><marker id="mermaid-svg-0_flowchart-v2-crossStart" class="marker cross flowchart-v2" viewBox="0 0 11 11" refX="-1" refY="5.2" markerUnits="userSpaceOnUse" markerWidth="11" markerHeight="11" orient="auto"><path d="M 1,1 l 9,9 M 10,1 l -9,9" class="arrowMarkerPath" style="stroke-width: 2; stroke-dasharray: 1, 0;"/></marker><g class="root"><g class="clusters"/><g class="edgePaths"><path d="M138,85.969L138,94.133C138,102.297,138,118.625,138,134.286C138,149.948,138,164.943,138,172.44L138,179.938" id="L_A_B_0" class="edge-thickness-normal edge-pattern-solid edge-thickness-normal edge-pattern-solid flowchart-link" style="" marker-end="url(#mermaid-svg-0_flowchart-v2-pointEnd)"/><path d="M138,237.922L138,242.089C138,246.255,138,254.589,138,262.255C138,269.922,138,276.922,138,280.422L138,283.922" id="L_B_C_0" class="edge-thickness-normal edge-pattern-solid edge-thickness-normal edge-pattern-solid flowchart-link" style="" marker-end="url(#mermaid-svg-0_flowchart-v2-pointEnd)"/><path d="M138,413.859L138,418.026C138,422.193,138,430.526,138,438.193C138,445.859,138,452.859,138,456.359L138,459.859" id="L_C_D_0" class="edge-thickness-normal edge-pattern-solid edge-thickness-normal edge-pattern-solid flowchart-link" style="" marker-end="url(#mermaid-svg-0_flowchart-v2-pointEnd)"/></g><g class="edgeLabels"><g class="edgeLabel" transform="translate(138, 134.953125)"><g class="label" transform="translate(-53.7109375, -23.984375)"><foreignObject width="107.421875" height="47.96875"><div xmlns="http://www.w3.org/1999/xhtml" class="labelBkg" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="edgeLabel"><p>TCP-пакет<br />172.22.1.1:102</p></span></div></foreignObject></g></g><g class="edgeLabel"><g class="label" transform="translate(0, 0)"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" class="labelBkg" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="edgeLabel"></span></div></foreignObject></g></g><g class="edgeLabel"><g class="label" transform="translate(0, 0)"><foreignObject width="0" height="0"><div xmlns="http://www.w3.org/1999/xhtml" class="labelBkg" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="edgeLabel"></span></div></foreignObject></g></g></g><g class="nodes"><g class="node default" id="flowchart-A-0" transform="translate(138, 46.984375)"><rect class="basic label-container" style="" x="-107.20703125" y="-38.984375" width="214.4140625" height="77.96875"/><g class="label" style="" transform="translate(-77.20703125, -23.984375)"><rect/><foreignObject width="154.4140625" height="47.96875"><div xmlns="http://www.w3.org/1999/xhtml" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="nodeLabel"><p>PlcSimClient<br />(S7.Net, TCP-клиент)</p></span></div></foreignObject></g></g><g class="node default" id="flowchart-B-1" transform="translate(138, 210.9296875)"><rect class="basic label-container" style="" x="-96.34765625" y="-26.9921875" width="192.6953125" height="53.984375"/><g class="label" style="" transform="translate(-66.34765625, -11.9921875)"><rect/><foreignObject width="132.6953125" height="23.984375"><div xmlns="http://www.w3.org/1999/xhtml" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="nodeLabel"><p>Loopback-адаптер</p></span></div></foreignObject></g></g><g class="node default" id="flowchart-C-2" transform="translate(138, 350.890625)"><rect class="basic label-container" style="" x="-130" y="-62.96875" width="260" height="125.9375"/><g class="label" style="" transform="translate(-100, -47.96875)"><rect/><foreignObject width="200" height="95.9375"><div xmlns="http://www.w3.org/1999/xhtml" style="display: table; white-space: break-spaces; line-height: 1.5; max-width: 200px; text-align: center; width: 200px;"><span class="nodeLabel"><p>NetToPLCSim<br />(TCP-сервер + переводчик<br />на внутренний протокол PLCSIM)</p></span></div></foreignObject></g></g><g class="node default" id="flowchart-D-3" transform="translate(138, 514.8359375)"><rect class="basic label-container" style="" x="-129.12109375" y="-50.9765625" width="258.2421875" height="101.953125"/><g class="label" style="" transform="translate(-99.12109375, -35.9765625)"><rect/><foreignObject width="198.2421875" height="71.953125"><div xmlns="http://www.w3.org/1999/xhtml" style="display: table-cell; white-space: nowrap; line-height: 1.5; max-width: 200px; text-align: center;"><span class="nodeLabel"><p>S7-PLCSIM<br />(эмулятор CPU,<br />хранит DB в своей памяти)</p></span></div></foreignObject></g></g></g></g></g></svg>mermaid_20260911_61ab84.svg…]()

---

## Требования перед подключением

Без выполнения всех трёх пунктов TCP-соединение не установится:

1. **S7-PLCSIM** запущен, CPU в режиме **RUN**.
2. **NetToPLCSim** запущен, мост активен (статус **Active**).
3. Нужные DB загружены в PLCSIM через `PLC → Download`.

---

## 0. Запуск программы

Запустить `PlcSimClient.exe`. На экране меню:

```
1 - Подключиться к PLC
2 - Прочитать значение из DB
3 - Записать значение в DB
4 - Отключиться
0 - Выйти
```

Ввод: цифра пункта + **Enter**.

---

## 1. Подключение

Инициализирует TCP-соединение с параметрами из конфигурации (IP, Rack, Slot, тип CPU). Результат — `Open()` соединения на транспортном уровне.

---

## 2. Чтение

Запрашивает параметры адресации:

| Параметр | Значение |
|---|---|
| Номер DB | целое число, номер блока (пример: `8001`) |
| Смещение | байтовый offset от начала DB |
| Тип данных | `1`=BOOL `2`=BYTE `3`=WORD `4`=DWORD `5`=INT `6`=DINT `7`=REAL `8`=STRING |
| Номер бита | только для BOOL, `0`–`7` |
| Длина строки | только для STRING, максимальная длина как в DB |

Результат — значение, прочитанное из ответного пакета PLC.

---

## 3. Запись

Используются те же параметры адресации, что при чтении, плюс значение:

| Тип | Формат значения |
|---|---|
| BOOL | `1` / `true` / `да` / `yes` = включить, любое другое = выключить |
| числа | числовое значение (пример: `123`, `3.14`) |
| STRING | текст, длина не более заданного максимума |

Значение упаковывается в байты и отправляется PLC.

---

## 4 / 0 — Отключение и выход

- **4** — закрыть TCP-соединение, программа остаётся открытой
- **0** — завершить программу

---

## 5. Логи

Все операции и ошибки пишутся в `log.txt` рядом с exe, с меткой времени.

**Типовые ошибки в логе:**

**Couldn't establish the connection / таймаут**
- NetToPLCSim не запущен или мост не активен
- PLCSIM не запущен или не в RUN
- неверный IP Loopback-адаптера

**Object does not exist**
- указанный номер DB не загружен в PLC в данный момент
- проверить фактически загруженные блоки в PLCSIM

**Ошибка протокола S7 / отказ соединения при чтении**
- неверный Rack/Slot или тип CPU не совпадает с реальным
- блок является F-блоком (Fail-safe) и требует отдельной загрузки через Safety Program, обычный Download его не переносит
